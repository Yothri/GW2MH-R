using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace GW2MH.Views
{
    public partial class FrmMain : Form
    {

        public bool IsSpeedhackEnabled { get; private set; }
        public bool IsFlyhackEnabled { get; private set; }

        public Process GuildWarsTwoProcess { get; private set; }

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            ttDefault.SetToolTip(numBaseSpeedMultiplier, "If Speedhack is enabled, this defines the speed in percent how fast your character is moving. (Default 100%).");
            ttDefault.SetToolTip(numExtSpeedMultiplier, "If Speedhack is enabled and Left Shift is pressed, then it multiplies your speed using this value.");
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            var processes = Process.GetProcessesByName("Gw2-64");
            if(processes.Length == 0)
            {
                MessageBox.Show("Guild Wars 2 (64 Bit) seems not to be running, please launch Guild Wars 2 first.", "Game client missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
            else
            {
                GuildWarsTwoProcess = processes[0];

                tmrUpdater.Start();
            }
        }

        private void tmrUpdater_Tick(object sender, EventArgs e)
        {
            //if(GuildWarsTwoMemory.IsRunning)
            //{

            //}
            //else
            //{
            //    MessageBox.Show("Guild Wars 2 has closed, GW2MH-R will close now.", "Bye", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    Close();
            //}
        }

        private void btnDonate_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=EHZVSBXL7X2Q6");
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmSettings().ShowDialog();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(tmrUpdater.Enabled)
                tmrUpdater.Stop();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}