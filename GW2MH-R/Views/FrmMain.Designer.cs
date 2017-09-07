namespace GW2MH.Views
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.tmrUpdater = new System.Windows.Forms.Timer(this.components);
            this.ttDefault = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DevTools = new System.Windows.Forms.ToolStripDropDownButton();
            this.clipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.agentPointerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.characterPointerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextPointerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBuyUnlimited = new System.Windows.Forms.ToolStripButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cbFlyhack = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numExtSpeedMultiplier = new System.Windows.Forms.NumericUpDown();
            this.numBaseSpeedMultiplier = new System.Windows.Forms.NumericUpDown();
            this.cbSpeedhack = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lbStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numExtSpeedMultiplier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBaseSpeedMultiplier)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrUpdater
            // 
            this.tmrUpdater.Interval = 50;
            this.tmrUpdater.Tick += new System.EventHandler(this.tmrUpdater_Tick);
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.CornflowerBlue;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 239);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(299, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.DevTools,
            this.btnBuyUnlimited});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(299, 25);
            this.toolStrip.TabIndex = 5;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(51, 22);
            this.toolStripDropDownButton1.Text = "Menu";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Enabled = false;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.settingsToolStripMenuItem.Text = "Settings (Comes Later)";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // DevTools
            // 
            this.DevTools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.DevTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clipboardToolStripMenuItem});
            this.DevTools.Image = ((System.Drawing.Image)(resources.GetObject("DevTools.Image")));
            this.DevTools.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DevTools.Name = "DevTools";
            this.DevTools.Size = new System.Drawing.Size(71, 22);
            this.DevTools.Text = "Dev Tools";
            this.DevTools.Visible = false;
            // 
            // clipboardToolStripMenuItem
            // 
            this.clipboardToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.agentPointerToolStripMenuItem,
            this.characterPointerToolStripMenuItem,
            this.contextToolStripMenuItem,
            this.contextPointerToolStripMenuItem});
            this.clipboardToolStripMenuItem.Name = "clipboardToolStripMenuItem";
            this.clipboardToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.clipboardToolStripMenuItem.Text = "Clipboard";
            // 
            // agentPointerToolStripMenuItem
            // 
            this.agentPointerToolStripMenuItem.Name = "agentPointerToolStripMenuItem";
            this.agentPointerToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.agentPointerToolStripMenuItem.Text = "Agent Pointer";
            // 
            // characterPointerToolStripMenuItem
            // 
            this.characterPointerToolStripMenuItem.Name = "characterPointerToolStripMenuItem";
            this.characterPointerToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.characterPointerToolStripMenuItem.Text = "Character Pointer";
            // 
            // contextToolStripMenuItem
            // 
            this.contextToolStripMenuItem.Name = "contextToolStripMenuItem";
            this.contextToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.contextToolStripMenuItem.Text = "Transform Pointer";
            // 
            // contextPointerToolStripMenuItem
            // 
            this.contextPointerToolStripMenuItem.Name = "contextPointerToolStripMenuItem";
            this.contextPointerToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.contextPointerToolStripMenuItem.Text = "Context Pointer";
            // 
            // btnBuyUnlimited
            // 
            this.btnBuyUnlimited.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnBuyUnlimited.Image = ((System.Drawing.Image)(resources.GetObject("btnBuyUnlimited.Image")));
            this.btnBuyUnlimited.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBuyUnlimited.Name = "btnBuyUnlimited";
            this.btnBuyUnlimited.Size = new System.Drawing.Size(125, 22);
            this.btnBuyUnlimited.Text = "Unlock Feature Limits";
            this.btnBuyUnlimited.Click += new System.EventHandler(this.btnBuyUnlimited_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(299, 214);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cbFlyhack);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.numExtSpeedMultiplier);
            this.tabPage1.Controls.Add(this.numBaseSpeedMultiplier);
            this.tabPage1.Controls.Add(this.cbSpeedhack);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(291, 188);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Character";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // cbFlyhack
            // 
            this.cbFlyhack.AutoSize = true;
            this.cbFlyhack.Location = new System.Drawing.Point(8, 32);
            this.cbFlyhack.Name = "cbFlyhack";
            this.cbFlyhack.Size = new System.Drawing.Size(63, 17);
            this.cbFlyhack.TabIndex = 5;
            this.cbFlyhack.Text = "Flyhack";
            this.cbFlyhack.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(270, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "%";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(174, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "%";
            // 
            // numExtSpeedMultiplier
            // 
            this.numExtSpeedMultiplier.Location = new System.Drawing.Point(195, 6);
            this.numExtSpeedMultiplier.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.numExtSpeedMultiplier.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numExtSpeedMultiplier.Name = "numExtSpeedMultiplier";
            this.numExtSpeedMultiplier.Size = new System.Drawing.Size(69, 20);
            this.numExtSpeedMultiplier.TabIndex = 2;
            this.numExtSpeedMultiplier.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numExtSpeedMultiplier.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            // 
            // numBaseSpeedMultiplier
            // 
            this.numBaseSpeedMultiplier.Location = new System.Drawing.Point(99, 6);
            this.numBaseSpeedMultiplier.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numBaseSpeedMultiplier.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numBaseSpeedMultiplier.Name = "numBaseSpeedMultiplier";
            this.numBaseSpeedMultiplier.Size = new System.Drawing.Size(69, 20);
            this.numBaseSpeedMultiplier.TabIndex = 1;
            this.numBaseSpeedMultiplier.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numBaseSpeedMultiplier.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            // 
            // cbSpeedhack
            // 
            this.cbSpeedhack.AutoSize = true;
            this.cbSpeedhack.Location = new System.Drawing.Point(8, 7);
            this.cbSpeedhack.Name = "cbSpeedhack";
            this.cbSpeedhack.Size = new System.Drawing.Size(81, 17);
            this.cbSpeedhack.TabIndex = 0;
            this.cbSpeedhack.Text = "Speedhack";
            this.cbSpeedhack.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(291, 188);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "World";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lbStatus
            // 
            this.lbStatus.ForeColor = System.Drawing.Color.White;
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(108, 17);
            this.lbStatus.Text = "Status: Not Ingame";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 261);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GW2MH-R";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numExtSpeedMultiplier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBaseSpeedMultiplier)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer tmrUpdater;
        private System.Windows.Forms.ToolTip ttDefault;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton DevTools;
        private System.Windows.Forms.ToolStripMenuItem clipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem agentPointerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem characterPointerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contextPointerToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnBuyUnlimited;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox cbFlyhack;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numExtSpeedMultiplier;
        private System.Windows.Forms.NumericUpDown numBaseSpeedMultiplier;
        private System.Windows.Forms.CheckBox cbSpeedhack;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStripStatusLabel lbStatus;
    }
}