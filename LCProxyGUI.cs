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
using System.Drawing.Drawing2D;
using System.Drawing;

namespace LCProxy
{

    // Please visit the User Classes to see the code

    public partial class LCProxyGUI : Form
    {
        // Rounded Corners
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
        int nLeftRect,
        int nTopRect,
        int nRightRect,
        int nBottomRect,
        int nWidthEllipse,
        int nHeightEllipse
        );

        // Shit for dragable panel
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public LCProxyGUI()
        {
            InitializeComponent();

            // Round the form corners
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 5, 5));

            installerPanel.Hide();
            troubleShootPanel.Hide();
            aboutPanel.Hide();
            SetRoundedShape(panel10, 20);
            SetRoundedShape(homeTabBtn, 20);
            SetRoundedShape(button1, 20);
            SetRoundedShape(troubleshootTabBtn, 20);
            SetRoundedShape(installerTabBtn, 20);
        }

        static void SetRoundedShape(Control control, int radius)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddLine(radius, 0, control.Width - radius, 0);
            path.AddArc(control.Width - radius, 0, radius, radius, 270, 90);
            path.AddLine(control.Width, radius, control.Width, control.Height - radius);
            path.AddArc(control.Width - radius, control.Height - radius, radius, radius, 0, 90);
            path.AddLine(control.Width - radius, control.Height, radius, control.Height);
            path.AddArc(0, control.Height - radius, radius, radius, 90, 90);
            path.AddLine(0, control.Height - radius, 0, radius);
            path.AddArc(0, 0, radius, radius, 180, 90);
            control.Region = new Region(path);
        }
        private void exitApplicationBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void homeTabBtn_Click(object sender, EventArgs e)
        {
            homeTabBtn.ForeColor = Color.White;

            installerTabBtn.ForeColor = Color.Gray;
            button1.ForeColor = Color.Gray;
            troubleshootTabBtn.ForeColor = Color.Gray;

            installerPanel.Hide();
            aboutPanel.Hide();
            troubleShootPanel.Hide();
        }

        private void installerTabBtn_Click(object sender, EventArgs e)
        {
            installerTabBtn.ForeColor = Color.White;

            homeTabBtn.ForeColor = Color.Gray;
            button1.ForeColor = Color.Gray;
            troubleshootTabBtn.ForeColor = Color.Gray;

            installerPanel.Show();
            aboutPanel.Hide();
            troubleShootPanel.Hide();
        }
        private void aboutTabBtn_Click(object sender, EventArgs e)
        {
            installerTabBtn.ForeColor = Color.Gray;
            button1.ForeColor = Color.White;

            homeTabBtn.ForeColor = Color.Gray;
            troubleshootTabBtn.ForeColor = Color.Gray;

            installerPanel.Hide();
            troubleShootPanel.Hide();
            aboutPanel.Show();
        }

        private void play_MouseEnter(object sender, EventArgs e)
        {
            panel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(176)))), ((int)(((byte)(11)))));
        }

        private void play_MouseLeave(object sender, EventArgs e)
        {
            panel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(194)))), ((int)(((byte)(23)))));
        }

        private void troubleshootTabBtn_Click(object sender, EventArgs e)
        {
            troubleshootTabBtn.ForeColor = Color.White;

            homeTabBtn.ForeColor = Color.Gray;
            button1.ForeColor = Color.Gray;
            installerTabBtn.ForeColor = Color.Gray;

            installerPanel.Hide();
            aboutPanel.Hide();
            troubleShootPanel.Show();
        }

        private void panel10_Click(object sender, EventArgs e)
        {
            if (!File.Exists("C:\\Windows\\System32\\drivers\\etc\\hosts")) File.Create("C:\\Windows\\System32\\drivers\\etc\\hosts");

            FileSecurity fSecurity = File.GetAccessControl("C:\\Windows\\System32\\drivers\\etc\\hosts");

            fSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, AccessControlType.Allow));

            File.SetAccessControl("C:\\Windows\\System32\\drivers\\etc\\hosts", fSecurity);

            DirectoryInfo profile = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

            String hostsFile = File.ReadAllText("C:\\Windows\\System32\\drivers\\etc\\hosts");
            if (hostsFile.Contains("165.22.69.232 assetserver.lunarclientprod.com"))
            {
                string partName = "zulu";
                DirectoryInfo dirSearch = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.lunarclient\\jre");
                FileSystemInfo[] filesAndDirs = dirSearch.GetFileSystemInfos("*" + partName + "*");
                foreach (FileSystemInfo file in filesAndDirs)
                {
                    string zulu = file.FullName;

                    string[] comands = { "@echo off", "echo This windows will closed a few seconds!", zulu + "\\bin\\javaw.exe --add-modules jdk.naming.dns --add-exports jdk.naming.dns/com.sun.jndi.dns=java.naming -Djna.boot.library.path=" + profile + "\\.lunarclient\\offline\\1.8\\natives --add-opens java.base/java.io=ALL-UNNAMED -Xms3G -Xmx3G -Xmn1G -XX:+UnlockExperimentalVMOptions -XX:+UseG1GC -XX:G1NewSizePercent=20 -XX:G1ReservePercent=20 -XX:MaxGCPauseMillis=50 -XX:G1HeapRegionSize=32M -Djava.library.path=" + profile + "\\.lunarclient\\offline\\1.8\\natives -XX:+DisableAttachMechanism -cp " + profile + "\\.lunarclient\\offline\\1.8\\lunar-assets-prod-1-optifine.jar;" + profile + "\\.lunarclient\\offline\\1.8\\lunar-assets-prod-2-optifine.jar;"+profile+"\\.lunarclient\\offline\\1.8\\lunar-assets-prod-3-optifine.jar;"+profile+"\\.lunarclient\\offline\\1.8\\lunar-libs.jar;"+profile+"\\.lunarclient\\offline\\1.8\\lunar-prod-optifine.jar;"+profile+"\\.lunarclient\\offline\\1.8\\OptiFine.jar;"+profile+"\\.lunarclient\\offline\\1.8\\vpatcher-prod.jar com.moonsworth.lunar.patcher.LunarMain --version 1.8 --accessToken 0 --assetIndex 1.8 --userProperties {} --gameDir "+profile+"\\AppData\\Roaming\\.minecraft --width 854 --height 480 --texturesDir "+profile+"\\.lunarclient\\textures --assetsDir "+profile+"\\AppData\\Roaming\\.minecraft\\assets", "echo You can close this window after Lunar Client started!" };
                    File.WriteAllLines(profile + "\\Desktop\\coms.bat", comands);
                    Process.Start(profile + "\\Desktop\\coms.bat");
                    Thread.Sleep(2000);
                    File.Delete(profile + "\\Desktop\\coms.bat");
                    foreach (Process proc in Process.GetProcessesByName("cmd"))
                    {
                        proc.Kill();
                    }
                }
                }
            else
            {
                MessageBox.Show("You don't have installed Lunar Proxy!" +
                "\n\nHow to install?" +
                "\n1. Click Installer button in tab" +
                "\n2. Click Install LP and wait few seconds." +
                "\n3. Done."+
                "\n\nAfter install just click this button again!"
                    );
            }
            File.WriteAllText("C:\\Windows\\System32\\drivers\\etc\\hosts", hostsFile);

            fSecurity.RemoveAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, AccessControlType.Allow));
            File.SetAccessControl("C:\\Windows\\System32\\drivers\\etc\\hosts", fSecurity);
        }

        private void havingIssuesBtn_Click(object sender, EventArgs e)
        {
            installerPanel.Hide();
            aboutPanel.Hide();
            troubleShootPanel.Show();

            troubleshootTabBtn.ForeColor = Color.White;
            homeTabBtn.ForeColor = Color.Gray;
            button1.ForeColor = Color.Gray;
            installerTabBtn.ForeColor = Color.Gray;
        }

        private void LCProxyGUI_Load(object sender, EventArgs e)
        {
            
        }

        private void installerPanel_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void panel12_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "https://lunarclient.com");
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void troubleShootPanel_Load(object sender, EventArgs e)
        {

        }

        private void aboutPanel_Load(object sender, EventArgs e)
        {
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel12_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
