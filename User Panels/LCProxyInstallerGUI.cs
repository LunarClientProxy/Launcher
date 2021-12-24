using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Management;

namespace LCProxy
{
    public partial class LCProxyInstallerGUI : UserControl
    {
        public LCProxyInstallerGUI()
        {
            InitializeComponent();
        }

        // Error for user not having JDK
        void noJava()
        {
            DialogResult result = MessageBox.Show("You are missing JDK 1.8, would you like to install it?\nThis may take up to 5 minutes.", "Missing JDK", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (var client = new WebClient())
                {
                    client.DownloadFileAsync(
                        new System.Uri("https://assets.lunarproxy.me/jdk.exe"),
                        "C:\\jdk.exe"
                    );
                    Process.Start("C:\\jdk.exe").WaitForExit();
                    client.Dispose();
                    MessageBox.Show("JDK has been installed!", "Lunar Proxy");
                }
            }
            else
            {
                MessageBox.Show("The installation cannot continue.", "Missing JDK", MessageBoxButtons.OK);
                Application.Exit();
            }
        }


        // Remove LProxy
        private void removeBtn_Click(object sender, EventArgs e)
        {
            FileSecurity fSecurity = File.GetAccessControl("C:\\Windows\\System32\\drivers\\etc\\hosts");

            fSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, AccessControlType.Allow));

            File.SetAccessControl("C:\\Windows\\System32\\drivers\\etc\\hosts", fSecurity);

            String replace = File.ReadAllText("C:\\Windows\\System32\\drivers\\etc\\hosts");
            if (replace.Contains("lunarclientprod.com"))
            {
                foreach(String line in replace.Split('\n'))
                {
                    if(line.Contains("lunarclientprod.com"))
                    {
                        replace = replace.Replace(line, "");
                    }
                }
            }
            File.WriteAllText("C:\\Windows\\System32\\drivers\\etc\\hosts", replace);

            fSecurity.RemoveAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, AccessControlType.Allow));
            File.SetAccessControl("C:\\Windows\\System32\\drivers\\etc\\hosts", fSecurity);

            removeBtn.Text = "Uninstalled!";
            removeBtn.Enabled = false;
        }

        // Install LProxy
        private void installBtn_Click(object sender, EventArgs e)
        {
            RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey key = hklm.OpenSubKey("SOFTWARE\\JavaSoft\\Java Runtime Environment\\1.8");
            if (key != null)
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile("http://165.22.69.232/assets/server.cert", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "server.cert"));
                    String javaHome = (string)key.GetValue("JavaHome");

                    string partName = "zulu";
                    DirectoryInfo dirSearch = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.lunarclient\\jre");
                    FileSystemInfo[] filesAndDirs = dirSearch.GetFileSystemInfos("*" + partName + "*");
                    foreach (FileSystemInfo foundFile in filesAndDirs)
                    {
                        string fullName = foundFile.FullName;
                        Process.Start(javaHome + "\\bin\\keytool.exe", "-keystore \"" + fullName + "\\lib\\security\\cacerts\" -trustcacerts -importcert -alias lcproxy -storepass changeit -file \"" + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "server.cert") + "\" -noprompt");
                    }

                    client.Dispose();
                }

                try
                {
                    if (!File.Exists("C:\\Windows\\System32\\drivers\\etc\\hosts")) File.Create("C:\\Windows\\System32\\drivers\\etc\\hosts");

                    FileSecurity fSecurity = File.GetAccessControl("C:\\Windows\\System32\\drivers\\etc\\hosts");

                    fSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, AccessControlType.Allow));

                    File.SetAccessControl("C:\\Windows\\System32\\drivers\\etc\\hosts", fSecurity);

                    String hostsFile = File.ReadAllText("C:\\Windows\\System32\\drivers\\etc\\hosts");
                    if (!hostsFile.Contains("165.22.69.232 assetserver.lunarclientprod.com"))
                    {
                        hostsFile += "\n165.22.69.232 assetserver.lunarclientprod.com";
                    }
                    File.WriteAllText("C:\\Windows\\System32\\drivers\\etc\\hosts", hostsFile);

                    fSecurity.RemoveAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, AccessControlType.Allow));
                    File.SetAccessControl("C:\\Windows\\System32\\drivers\\etc\\hosts", fSecurity);

                    installBtn.Text = "Successfully!";
                    installBtn.Enabled = false;
                } catch (Exception)
                {
                    MessageBox.Show("Failed to install. Try using the troubleshooter.", "Error", MessageBoxButtons.OK);
                }
            }
            else
            {
                new Thread(noJava).Start();
            }
        }

        public static void GoToSite(string url)
        {
            System.Diagnostics.Process.Start("explorer.exe", url);
        }

        private void joinDiscordBtn_Click(object sender, EventArgs e)
        {
            GoToSite("https://discord.gg/8VGGPs2wWg");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
