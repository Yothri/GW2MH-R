using GW2MH.Core.Settings;
using System;
using System.Windows.Forms;

namespace GW2MH.Views
{
    public partial class FrmSettings : Form
    {

        private KeyAssignmentManager KeyAssignmentManager;

        public FrmSettings()
        {
            InitializeComponent();
        }

        private void FrmSettings_Load(object sender, EventArgs e)
        {
            KeyAssignmentManager = new KeyAssignmentManager();

            foreach(var btn in gbKeyAssignments.Controls)
            {
                if(btn.GetType() == typeof(Button))
                    KeyAssignmentManager.RegisterButton(btn as Button);
            }
        }

        private void FrmSettings_KeyUp(object sender, KeyEventArgs e)
        {
            KeyAssignmentManager?.KeyUpTriggered(e);
        }
    }
}