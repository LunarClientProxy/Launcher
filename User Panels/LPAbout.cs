using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;


namespace LCProxy
{
    public partial class LPAbout : UserControl
    {
        public LPAbout()
        {
            InitializeComponent();
        }


        // Goes through every fix in attempt to fix the users issues.
        private void discordBtn_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "https://discord.gg/8VGGPs2wWg");
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
