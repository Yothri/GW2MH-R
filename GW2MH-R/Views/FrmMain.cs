using GW2MH.Core.Network;
using System.Windows.Forms;

namespace GW2MH.Views
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        internal FrmMain(LoginResponse loginResponse) : this()
        {

        }
    }
}