using GW2MH.Core.Data;
using GW2MH.Core.Memory;
using GW2MH.Core.Network;
using System;
using System.Diagnostics;
using System.Threading;
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

        private Stopwatch stopwatch;

        public FrmMain()
        {
            InitializeComponent();
            stopwatch = new Stopwatch();

            LoginResponse = new LoginResponse()
            {
                name = "Yothri"
            };
        }

        public FrmMain(LoginResponse loginResponse) : this()
        {
            LoginResponse = loginResponse;
        }

        private async void FrmMain_Load(object sender, EventArgs e)
        {
            ttDefault.SetToolTip(numBaseSpeedMultiplier, "If Speedhack is enabled, this defines the speed in percent how fast your character is moving.");
            ttDefault.SetToolTip(numExtSpeedMultiplier, "If Speedhack is enabled and Left Shift is pressed, then it multiplies your speed using this value.");
            ttDefault.SetToolTip(cbAntiKick, "Every 5 seconds, W and S is being sent to the game to keep you ingame.");

#if DEBUG
            DevTools.Visible = true;
#endif

            if (await PaymentApi.PaymentDetails(LoginResponse.name))
            {
                numBaseSpeedMultiplier.Maximum = 999999;
                numExtSpeedMultiplier.Maximum = 999999;
                btnRemoteTP.Enabled = true;
                ttDefault.SetToolTip(btnRemoteTP, "Get your items and money remotely from trading post.");
            }
            else
            {
                btnBuyUnlimited.Visible = true;
                btnRemoteTP.Enabled = false;
                ttDefault.SetToolTip(btnRemoteTP, "Buy GW2MH-R Feature Limit Unlock to unlock this feature.");
            }
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

                MemoryData.RemoteTradingPostAddress = await Task.Factory.StartNew(() =>
                {
                    return Memory.Pattern(Memory.TargetProcess.MainModule, MemoryData.RemoteTradingPostPattern);
                });

                if (MemoryData.RemoteTradingPostAddress == IntPtr.Zero)
                    btnRemoteTP.Enabled = false;

                MemoryData.SetFOVAddress = await Task.Factory.StartNew(() =>
                {
                    return Memory.Pattern(Memory.TargetProcess.MainModule, MemoryData.SetFOVPattern);
                });

                if (MemoryData.SetFOVAddress == IntPtr.Zero)
                    cbFOV.Enabled = false;

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
                while (!CharacterData.IsCharacterIngame)
                {
                    if(Memory == null || !Memory.IsRunning)
                        break;
                }
            });

            if (Memory != null && Memory.IsRunning && CharacterData.IsCharacterIngame)
            {
                CharacterData.DefaultMoveSpeed = Memory.Read<float>(MemoryData.ContextPtr, MemoryData.MoveSpeedOffsets);
                CharacterData.DefaultGravity = Memory.Read<float>(MemoryData.ContextPtr, MemoryData.GravityOffsets);

                tmrUpdater.Start();
                lbStatus.Text = string.Format("Status: Ingame");
            }
            else
            {
                MessageBox.Show("Guild Wars 2 has closed, GW2MH-R will close now.", "Bye", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
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

                    if(cbAutoLoot.Checked)
                    {
                        if(Memory.Read<byte>(MemoryData.LootOffset, true) == 64)
                        {
                            Native.PostMessage(Memory.TargetProcess.MainWindowHandle, 256u, 70, 2162689);
                            Thread.Sleep(50);
                            Native.PostMessage(Memory.TargetProcess.MainWindowHandle, 257u, 70, 2162689);
                        }
                    }

                    if(cbFOV.Checked)
                    {
                        Memory.Write(Memory.Read<IntPtr>((IntPtr)(Memory.TargetProcess.MainModule.BaseAddress.ToInt64() + MemoryData.CameraPtr.ToInt64())), MemoryData.FOVOffsets, (float)numFOV.Value);
                    }

                    if (cbAntiKick.Checked && !stopwatch.IsRunning)
                        stopwatch.Start();
                    else if (cbAntiKick.Checked && stopwatch.IsRunning)
                    {
                        if (stopwatch.ElapsedMilliseconds >= 480000) // 8 mins
                        {

                            var windowHandle = Native.FindWindow(null, "Guild Wars 2");
                            if(windowHandle != IntPtr.Zero)
                            {
                                Native.PostMessage(windowHandle, 256u, 38, 289931265);
                                Thread.Sleep(320);
                                Native.PostMessage(windowHandle, 257u, 38, 289931265);
                                Thread.Sleep(100);
                                Native.PostMessage(windowHandle, 256u, 40, 22020097);
                                Thread.Sleep(900);
                                Native.PostMessage(windowHandle, 257u, 40, 22020097);
                            }

                            stopwatch.Restart();
                        }
                    }
                    else
                        stopwatch.Stop();
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
            var dr = MessageBox.Show("To unlock the feature limitation of GW2MH-R you need to pay 5€, do you want to buy now?", "Unlock Feature Limits", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(dr == DialogResult.Yes)
            {
                Cursor = Cursors.WaitCursor;

                var paymentCreateResponse = await PaymentApi.CreatePayment(LoginResponse.name);
                if (paymentCreateResponse.success)
                {
                    Process.Start(paymentCreateResponse.approvalLink);
                }
                else
                    MessageBox.Show(paymentCreateResponse.error_message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Cursor = Cursors.Default;
            }
        }

        private void transformPointerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Memory.IsRunning)
            {
                var contextOffsets = new int[] { MemoryData.MoveSpeedOffsets[0], MemoryData.MoveSpeedOffsets[1], MemoryData.MoveSpeedOffsets[2], MemoryData.MoveSpeedOffsets[3] };

                Clipboard.SetText(Memory.ReadMultiLevelPointer(MemoryData.ContextPtr, contextOffsets).ToString("X8"));
            }
        }

        private void btnRemoteTP_Click(object sender, EventArgs e)
        {
            IntPtr outPtr;
            if(Native.CreateRemoteThread(Memory.ElevatedHandle, IntPtr.Zero, 0u, MemoryData.RemoteTradingPostAddress, IntPtr.Zero, 0u, out outPtr) == IntPtr.Zero)
            {
                MessageBox.Show("That did not work properly, please contact the administrator to fix this problem.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cbFOV_CheckedChanged(object sender, EventArgs e)
        {
            if(Memory.IsRunning)
            {
                if (cbFOV.Checked)
                    Memory.Write(MemoryData.SetFOVAddress, MemoryData.DisableSetFOV);
                else
                    Memory.Write(MemoryData.SetFOVAddress, MemoryData.EnableSetFOV);
            }
        }
    }
}