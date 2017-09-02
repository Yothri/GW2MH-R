using GW2MH.Core.Network;
using nUpdate.Updating;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace GW2MH.Views
{
    public partial class FrmLogin : Form
    {

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            cbRemember.Checked = Properties.Settings.Default.Remember;
            if(cbRemember.Checked)
            {
                txtUsername.Text = Properties.Settings.Default.Username;
                txtPassword.Text = Properties.Settings.Default.Password;
            }
        }

        private void FrmLogin_Shown(object sender, EventArgs e)
        {
            UpdateManager manager = new UpdateManager(new Uri("http://api.yothri.com/gw2mh-r/updates/updates.json"), "<RSAKeyValue><Modulus>4X3eMxw8ZHdXIsDsYHJFYkTiy96ghtLeFMSeDU2wWhWnYn6JR2IDCy1S/eo680tZXHZ4dHXgNZFi+5zaQKg6gN6jC99jyzeLzdnMCIOU95nndD/DeslsQdATlWIbWhaeyoX4m8f6d3/hU4wdb/ECzMJ03AoDs7OFbP3wAnl2oJ3o6ul47D8Gfd8lgjh6m/9Hq9zDbxQVb6XxYLMFixjiNQeXX5pZNrPFY2TEemek2d0bTuZWYadf/T1phdkA2VvBDJ+K+o1rlwhCmKvXLF+JwEm3Z/8uHYHJyxUit4goojBZnYQyZtIqDnlTPjctpBIY52IsYKWPBgmiWFHgUesufeWvs654pBId92pnN2fjawmQQOHfcgByjxo/RSLpanxYcUZEivz0O2W0JDtKrcWRi3o1r4W1egWMrQQ6odc0TRVO5erb0UZiW07lrxqYTTw0uLFxLAVmsr2Fub7OG9Qu3Hs7CnuTeyxkco1w7MsojDSL2xb1CPoQCTlHq7y5c2ZEpnExHUE8VV4f7Wgl5pzElME66yPnW4SxZecRDahfxnPhnJVL9Z3Q0TY0y8Fo6XE6Pt5RHrTHgx2JNVa+HoHbdUMcOWRg7/i1gvEDlZfYl/G8pDos3R8quxFHeXT1OYnZcp5jpkWD9MRW33ms6YJ4U82EPDYiMi1xa4+2o+M8ikci3wE1pa6GOyWRuADZj7RmWggTR+yy+qhM3es0/C2s0BDk92qg7EZC5XAiysXBuVuh9gKEM3gAdzO5RiGYD2z/SdWMMmWjBNCDGh+r05sl8+SDDJ3IcMCplgGuU6uPhAavp5GAKAGJSc9lsqe3N/z3s8buJUF0RTe8PZMq4gP+sdOTV0VLSKcBwfkZT19apT+EZr4iGF2/WyhXeVS+h1oc0+bqEpeQXU1yglH0QjwrI+a2qYKiOTw3nvsGN6fMYWcs2uBJ/KOuFx17ZRUQZTX210fGAFKcVyIkDOaXePr4dlB79J4ABoN+7befa1GygMoBmWM6wXl843eAZWXTa+EPal2vVM40ncG/JRzgdCzfqwaPK7vQ7MjAJNHIaupLFqAbZvbRYGCMEK0F3osADKp795px0t+7qBKVeOMBkcBbmQMXDBPludaA2QmHZp4vEfs+AgNaFsqxB4r7dSFspA3TtdFRONFtCaxPeYQRpFBIXzFTp0Ec1ZPfGwKrEkcm4u3s0ceFs5eLt6HVYmLlOCSPyEZGHjSry5+v8rSRpS3dis0wMY7HU5o2ZOpPzbtzNNwv/+MWV+RAMTTXbOGNKzvBfd0xAZFY4bM1EWLCcNNCBdZa0/levFZio1DJrNiLsm4RHj++4gXM3d4cDkGnMiSl1a8QAMvxqvvYPe/pHysdDQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", new CultureInfo("en"));
            new UpdaterUI(manager, SynchronizationContext.Current) { UseHiddenSearch = true }.ShowUserInterface();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text != string.Empty && txtPassword.Text != string.Empty)
            {
                txtUsername.Enabled = false;
                txtPassword.Enabled = false;
                btnLogin.Enabled = false;
                lbSignUp.Enabled = false;
                cbRemember.Enabled = false;

                var response = await BoardApi.LoginAsync(txtUsername.Text, txtPassword.Text);
                if (response != null && response.status == "1")
                {
                    Properties.Settings.Default.Remember = cbRemember.Checked;
                    if (cbRemember.Checked)
                    {                        
                        Properties.Settings.Default.Username = txtUsername.Text;
                        Properties.Settings.Default.Password = txtPassword.Text;
                    }
                    Properties.Settings.Default.Save();

                    Hide();
                    new FrmMain(response).ShowDialog();
                    Close();
                }
                else if (response == null)
                {
                    txtUsername.Enabled = true;
                    txtPassword.Enabled = true;
                    btnLogin.Enabled = true;
                    lbSignUp.Enabled = true;
                    cbRemember.Enabled = true;
                    MessageBox.Show("An unknown error occured, please try again later.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    txtUsername.Enabled = true;
                    txtPassword.Enabled = true;
                    btnLogin.Enabled = true;
                    lbSignUp.Enabled = true;
                    cbRemember.Enabled = true;
                    MessageBox.Show(response.msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }                    
            }
            else
                MessageBox.Show("Please enter your forum details to proceed.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void lbSignUp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://forum.yothri.com/index.php?terms/&aboutToRegister=1");
        }
    }
}