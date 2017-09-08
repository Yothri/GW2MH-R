using GW2MH.Core.Data;
using GW2MH.Core.Memory;
using GW2MH.Core.Network;
using PayPal.Api;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GW2MH.Views
{
    public partial class FrmMain : Form
    {

        public bool IsSpeedhackEnabled { get; private set; }
        public bool IsFlyhackEnabled { get; private set; }

        public Process TargetProcess { get; private set; }
        public MemSharp Memory { get; private set; }

        internal CharacterData CharacterData { get; private set; }

        internal LoginResponse LoginResponse { get; private set; }

        public FrmMain()
        {
            InitializeComponent();

            LoginResponse = new LoginResponse()
            {
                name = "Yothri"
            };
        }

        public FrmMain(LoginResponse loginResponse) : this()
        {
            LoginResponse = loginResponse;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            ttDefault.SetToolTip(numBaseSpeedMultiplier, "If Speedhack is enabled, this defines the speed in percent how fast your character is moving.");
            ttDefault.SetToolTip(numExtSpeedMultiplier, "If Speedhack is enabled and Left Shift is pressed, then it multiplies your speed using this value.");

#if DEBUG
            DevTools.Visible = true;
#endif
        }

        private async void FrmMain_Shown(object sender, EventArgs e)
        {
            var processes = Process.GetProcessesByName("Gw2-64");
            if (processes.Length == 0)
            {
#if !DEBUG
                MessageBox.Show("Guild Wars 2 (64 Bit) seems not to be running, please launch Guild Wars 2 first.", "Game client missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
#endif
            }
            else
            {
                TargetProcess = processes[0];
                Memory = new MemSharp(TargetProcess);

                MemoryData.ContextPtr = await Task.Factory.StartNew(() =>
                {
                    var contextPtr = IntPtr.Zero;
                    var contextCalcPtr = Memory.Pattern(Memory.TargetProcess.MainModule, MemoryData.ContextCalcPattern);

                    if (contextCalcPtr != IntPtr.Zero)
                    {
                        var jumpSize = (uint)MemoryData.ContextCalcJumpPatch(IntPtr.Zero).Length;

                        IntPtr jumpLocation = Native.VirtualAllocEx(Memory.TargetProcess.Handle, IntPtr.Zero, jumpSize, Native.AllocationTypeFlags.MEM_COMMIT, Native.MemoryProtectionFlags.PAGE_EXECUTE_READ_WRITE);
                        IntPtr pointerLocation = Native.VirtualAllocEx(Memory.TargetProcess.Handle, IntPtr.Zero, (uint)IntPtr.Size, Native.AllocationTypeFlags.MEM_COMMIT, Native.MemoryProtectionFlags.PAGE_READ_WRITE);
                        if (jumpLocation != IntPtr.Zero && pointerLocation != IntPtr.Zero)
                        {
                            Memory.Write(jumpLocation, MemoryData.ContextCalcJumpShellCode(pointerLocation, contextCalcPtr + MemoryData.ContextCalcJumpPatchOffset + 13));
                            Memory.Write(contextCalcPtr + MemoryData.ContextCalcJumpPatchOffset, MemoryData.ContextCalcJumpPatch(jumpLocation));

                            while (contextPtr == IntPtr.Zero) { contextPtr = new IntPtr(Memory.Read<long>(pointerLocation)); }

                            Memory.Write(contextCalcPtr + MemoryData.ContextCalcJumpPatchOffset, MemoryData.ContextCalcRestore);
                        }
                    }

                    return contextPtr;
                });

                if (MemoryData.ContextPtr != IntPtr.Zero)
                {
                    await InitialTick();
                }
                else
                {
                    MessageBox.Show("Unable to find Context Pointer, please contact the administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
            }
        }

        private async Task InitialTick()
        {
            CharacterData = new CharacterData(Memory);

            await Task.Factory.StartNew(() =>
            {
                while (!CharacterData.IsCharacterIngame) { }
            });

            if (Memory != null && Memory.IsRunning)
            {
                CharacterData.DefaultMoveSpeed = Memory.Read<float>(MemoryData.ContextPtr, MemoryData.MoveSpeedOffsets);
                CharacterData.DefaultGravity = Memory.Read<float>(MemoryData.ContextPtr, MemoryData.GravityOffsets);
                
                tmrUpdater.Start();
                lbStatus.Text = string.Format("Status: Ingame");
            }
        }

        private void FinalTick()
        {
            if(Memory != null && Memory.IsRunning && CharacterData != null)
            {
                // Reset Move Speed
                Memory.Write(MemoryData.ContextPtr, MemoryData.MoveSpeedOffsets, CharacterData.DefaultMoveSpeed);

                // Reset Gravity
                Memory.Write(MemoryData.ContextPtr, MemoryData.GravityOffsets, CharacterData.DefaultGravity);
            }
        }

        private async void tmrUpdater_Tick(object sender, EventArgs e)
        {
            if (Memory != null && Memory.IsRunning)
            {
                if(CharacterData.IsCharacterIngame)
                {
                    if (cbSpeedhack.Checked)
                    {
                        if (Convert.ToBoolean(Native.GetAsyncKeyState(Keys.LShiftKey) & 0x8000))
                            Memory.Write(MemoryData.ContextPtr, MemoryData.MoveSpeedOffsets, CharacterData.DefaultMoveSpeed * ((float)numExtSpeedMultiplier.Value / 100f));
                        else
                            Memory.Write(MemoryData.ContextPtr, MemoryData.MoveSpeedOffsets, CharacterData.DefaultMoveSpeed * ((float)numBaseSpeedMultiplier.Value / 100f));
                    }
                    else
                        Memory.Write(MemoryData.ContextPtr, MemoryData.MoveSpeedOffsets, CharacterData.DefaultMoveSpeed);

                    if (cbFlyhack.Checked)
                    {
                        if (Convert.ToBoolean(Native.GetAsyncKeyState(Keys.Menu) & 0x8000))
                            Memory.Write(MemoryData.ContextPtr, MemoryData.GravityOffsets, 15f);
                        else
                            Memory.Write(MemoryData.ContextPtr, MemoryData.GravityOffsets, CharacterData.DefaultGravity);
                    }
                    else
                        Memory.Write(MemoryData.ContextPtr, MemoryData.GravityOffsets, CharacterData.DefaultGravity);
                }
                else
                {
                    tmrUpdater.Stop();
                    lbStatus.Text = string.Format("Status: Not Ingame");
                    await InitialTick();
                }
            }
            else
            {
                tmrUpdater.Stop();
                MessageBox.Show("Guild Wars 2 has closed, GW2MH-R will close now.", "Bye", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmSettings().ShowDialog();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            FinalTick();

            if (tmrUpdater.Enabled)
                tmrUpdater.Stop();

            if (Memory != null && Memory.IsRunning)
                Memory.Dispose();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void characterPointerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Memory.IsRunning)
            {
                var characterOffsets = new int[] { MemoryData.MoveSpeedOffsets[0], MemoryData.MoveSpeedOffsets[1] };

                Clipboard.SetText(Memory.ReadMultiLevelPointer(MemoryData.ContextPtr, characterOffsets).ToString("X8"));
            }
        }

        private void agentPointerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Memory.IsRunning)
            {
                var agentOffsets = new int[] { MemoryData.MoveSpeedOffsets[0], MemoryData.MoveSpeedOffsets[1], MemoryData.MoveSpeedOffsets[2] };

                Clipboard.SetText(Memory.ReadMultiLevelPointer(MemoryData.ContextPtr, agentOffsets).ToString("X8"));
            }
        }

        private void contextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Memory.IsRunning)
            {
                var transformOffsets = new int[] { MemoryData.MoveSpeedOffsets[0], MemoryData.MoveSpeedOffsets[1], MemoryData.MoveSpeedOffsets[2], MemoryData.MoveSpeedOffsets[3] };

                Clipboard.SetText(Memory.ReadMultiLevelPointer(MemoryData.ContextPtr, transformOffsets).ToString("X8"));
            }
        }

        private void contextPointerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Memory.IsRunning)
            {
                var contextOffsets = new int[] { MemoryData.MoveSpeedOffsets[0] };

                Clipboard.SetText(Memory.ReadMultiLevelPointer(MemoryData.ContextPtr, contextOffsets).ToString("X8"));
            }
        }

        private async void btnBuyUnlimited_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show("Unlocking the feature limits costs 5€, press Yes if you want to unlock the features now or No to keep everything as it is.", "Feature Unlock", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(dr == DialogResult.Yes)
            {
                MessageBox.Show("We are creating your payment now, please approve the payment in the browser window.", "PayPal Approval", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Cursor = Cursors.WaitCursor;
                var payment = await Task.Factory.StartNew(() =>
                {
                    var accessToken = new OAuthTokenCredential(
                    "AYxsS5oBF7qELt4oHiK57cR3XJEZSr2P0oXqiuMS0NOuTSnmbyvALA_x50uB8tVDG6C4IncWPjpROVJu",
                    "EEK-YEerLc8JDAaSgv9fKCyon78oPJycUbMUTMQX-zjoed3RlgNQsyDya7AQSVHcrcLQdVSphKVRgvzY").GetAccessToken();

                    var apiContext = new APIContext(accessToken);
                    return new Payment()
                    {
                        intent = "sale",
                        payer = new Payer()
                        {
                            payment_method = "paypal"
                        },
                        transactions = new System.Collections.Generic.List<Transaction>()
                {
                    {
                        new Transaction()
                        {
                            amount = new Amount()
                            {
                                currency = "EUR",
                                total = "5"
                            },
                            description = "Feature Limitation Unlock for GW2MH-R.",
                            custom = LoginResponse.name,
                            payment_options = new PaymentOptions()
                            {
                                 allowed_payment_method= "INSTANT_FUNDING_SOURCE"
                            }
                        }
                    },
                },
                        redirect_urls = new RedirectUrls()
                        {
                            return_url = "http://api.yothri.com/paypal/test.php",
                            cancel_url = "https://www.paypal.com/cancel"
                        }
                    }.Create(apiContext);
                });

                Cursor = Cursors.Default;
                Process.Start(payment.GetApprovalUrl());

                lbStatus.Text = "Status: Waiting for payment approval...";
            }
        }
    }
}