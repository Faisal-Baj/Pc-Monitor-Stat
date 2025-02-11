namespace PcMonitor
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnToggleCommunication = new RJCodeAdvance.RJControls.RJToggleButton();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.TrayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.startStopMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmbPorts = new System.Windows.Forms.ComboBox();
            this.btnRefreshPorts = new RJCodeAdvance.RJControls.RJButton();
            this.treeViewStats = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TrayMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnToggleCommunication
            // 
            resources.ApplyResources(this.btnToggleCommunication, "btnToggleCommunication");
            this.btnToggleCommunication.Name = "btnToggleCommunication";
            this.btnToggleCommunication.OffBackColor = System.Drawing.Color.Brown;
            this.btnToggleCommunication.OffToggleColor = System.Drawing.Color.DimGray;
            this.btnToggleCommunication.OnBackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnToggleCommunication.OnToggleColor = System.Drawing.Color.DimGray;
            this.btnToggleCommunication.UseVisualStyleBackColor = true;
            this.btnToggleCommunication.CheckedChanged += new System.EventHandler(this.rjToggleButton1_CheckedChanged);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.TrayMenu;
            resources.ApplyResources(this.notifyIcon1, "notifyIcon1");
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick_1);
            // 
            // TrayMenu
            // 
            this.TrayMenu.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TrayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startStopMenuItem,
            this.exitMenuItem});
            this.TrayMenu.Name = "TrayMenu";
            resources.ApplyResources(this.TrayMenu, "TrayMenu");
            // 
            // startStopMenuItem
            // 
            resources.ApplyResources(this.startStopMenuItem, "startStopMenuItem");
            this.startStopMenuItem.Name = "startStopMenuItem";
            this.startStopMenuItem.Click += new System.EventHandler(this.startStopMenuItem_Click);
            // 
            // exitMenuItem
            // 
            resources.ApplyResources(this.exitMenuItem, "exitMenuItem");
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // cmbPorts
            // 
            this.cmbPorts.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.cmbPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPorts.FormattingEnabled = true;
            resources.ApplyResources(this.cmbPorts, "cmbPorts");
            this.cmbPorts.Name = "cmbPorts";
            // 
            // btnRefreshPorts
            // 
            this.btnRefreshPorts.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnRefreshPorts.BackgroundColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnRefreshPorts.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnRefreshPorts.BorderRadius = 0;
            this.btnRefreshPorts.BorderSize = 0;
            this.btnRefreshPorts.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnRefreshPorts, "btnRefreshPorts");
            this.btnRefreshPorts.ForeColor = System.Drawing.Color.White;
            this.btnRefreshPorts.Name = "btnRefreshPorts";
            this.btnRefreshPorts.TextColor = System.Drawing.Color.White;
            this.btnRefreshPorts.UseVisualStyleBackColor = false;
            this.btnRefreshPorts.Click += new System.EventHandler(this.btnRefreshPorts_Click);
            // 
            // treeViewStats
            // 
            this.treeViewStats.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.treeViewStats.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeViewStats.CheckBoxes = true;
            resources.ApplyResources(this.treeViewStats, "treeViewStats");
            this.treeViewStats.Name = "treeViewStats";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Name = "label1";
            // 
            // lblConnectionStatus
            // 
            resources.ApplyResources(this.lblConnectionStatus, "lblConnectionStatus");
            this.lblConnectionStatus.ForeColor = System.Drawing.Color.Red;
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GrayText;
            this.BackgroundImage = global::PcMonitor.Properties.Resources.icon1;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblConnectionStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeViewStats);
            this.Controls.Add(this.btnRefreshPorts);
            this.Controls.Add(this.cmbPorts);
            this.Controls.Add(this.btnToggleCommunication);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.TrayMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RJCodeAdvance.RJControls.RJToggleButton btnToggleCommunication;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip TrayMenu;
        private System.Windows.Forms.ToolStripMenuItem startStopMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ComboBox cmbPorts;
        private RJCodeAdvance.RJControls.RJButton btnRefreshPorts;
        private System.Windows.Forms.TreeView treeViewStats;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.Label label2;
    }
}

