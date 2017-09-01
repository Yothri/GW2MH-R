using nUpdate.Updating;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;

namespace GW2MH.Design
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnCreateAccount_Click(object sender, System.EventArgs e)
        {
            Process.Start("http://forum.yothri.com/index.php?terms/&aboutToRegister=1");
        }

        private void btnLogin_Click(object sender, System.EventArgs e)
        {
            Hide();
            new FrmMain().ShowDialog();
            Close();
        }

        private void FrmLogin_Shown(object sender, System.EventArgs e)
        {
            UpdateManager manager = new UpdateManager(new Uri("http://api.yothri.com/gw2mh/updates/updates.json"), "<RSAKeyValue><Modulus>v3+k9Sr/lNuu/0aQr0/mXKniHPWe0K+6celr+F8TZDg2A/balGI81B3i0lE4SqBqJTcUrpX4yUKZWYl9jMSQatgRUv0Hj19XcfzVfX0lfeZFwJmgb4KzM3zPPvTxY7BXtV9VLeHhLR4XVl1uExYFjdf4CmJYOFZkWcN62EPRpxySSHLFeKw5kucPZJ90NK4XEvAl6vdN5AAnpByh7HORpLri+wgMG5RlF3myNpnzpbC7b3Kc4AXZqcGZ6G1/h54C5gFwAe/BMb/rTxgnWIXuriVnQEREyrBe9qy17U8nAPXaK6Eqz+mN0aNNfacyLX2vs3+5cmy1bK4+SAmhdV2przDQDS+HHZMkvkW26ukclNVX8/mN3NsHxDpMI9YYn2WQDehKz8TZ/s4zmvEKJBs8UNIuAgsMOxWo13ea1l+kf6Oc8kKr3aYTsrflzjwmWPS0/aAyuvBP4eBWtu5EE3mf6AZHt5StP5URGWjMZQpGljk+iqYRe4MmOUa/32ReF+WDJaSAmBZfLtFCnWa1u859ll9wNt29/hPrHpJU08+IszfjWNj6Sswyjr7pEe8QLeqoPbMi/EBmzAd2esXX8kAa49c72li0j8SI8gxytteahTh0LMaLF9UV46ko09A9MfFGAzEH3l5XPITN0uBjinjnYY/99rAw2aYfa1NzI08Kcmh9vAejI4/YoUcMTK9dD6IzQ0xTcJNiSvoBEAeEQ4W/Tc9DEUfFi/tY9YaF/e7PFiuIa4wddZKxmYpUo2jZsCFkqcmf406ATAP6HwHgtV+6PqSr/9g1LaywVnYAYmmGwWipBxnXBgtTGorrVnxlAwepNZ9ENAIlXpaI9KdAPwpt/6zMmz1K5pvaI4K5isxUrDVaur3pV/xPk6Lem7XIW+o23Ue6OQJ5Ok92Sk2jXeyIEUHKxMX16d2/KPPPmXOZdgpNS/pUfSUO26oPb2dSDNBaV748WFBkK4u6umAcNRfSuZOcQqp/T/DbKy8p0D1v3HhYaUoZsP2iNYM1gwOEOO+Ca5f89g3IYOLJRhJ8bqmKfYG8jT7zd+xPubPRcyw5PFoaQ0QznkXJEaKGyHzqMoXFCRxQpUSz/yIz5t/uymGHuOfOxEDBRvaNrG+mWxdVR3NlP3jPc03wVgNjPJqed+je/yyyJmQXbbyDtLmmeNL037fSnSccTLPv7c4eIdz6LjzivucY3+Woz2ZtFc6h7TmUPrUm75Ib15QMgNpDRumb3HU+d9PHzqjOEivcnSfvaV4w6k4YXNiSilpOhne662W6NW+MNnuncq3i456jZYo5iQbCv3ghMyFL4uRMm71fsEc8/2cqMEedBVJr/xTUipvaE7OeHolxDY+R8P50zifMIQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", new CultureInfo("en"));
            manager.UpdateSearchFinished += Manager_UpdateSearchFinished;
            manager.SearchForUpdatesAsync();
        }

        private void Manager_UpdateSearchFinished(object sender, nUpdate.UpdateEventArgs.UpdateSearchFinishedEventArgs e)
        {
            MessageBox.Show(e.UpdatesAvailable.ToString());
        }
    }
}