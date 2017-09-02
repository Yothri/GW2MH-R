using GW2MH.Views;
using System;
using System.Windows.Forms;

namespace GW2MH_R
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if DEBUG
            Application.Run(new FrmMain());
#else
            Application.Run(new FrmLogin());
#endif
        }
    }
}