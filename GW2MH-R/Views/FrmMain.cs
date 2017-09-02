using System.Diagnostics;
using System.Windows.Forms;

namespace GW2MH.Views
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnDonate_Click(object sender, System.EventArgs e)
        {
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=EHZVSBXL7X2Q6");
        }

        private void settingsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            new FrmSettings().ShowDialog();
        }
    }
}