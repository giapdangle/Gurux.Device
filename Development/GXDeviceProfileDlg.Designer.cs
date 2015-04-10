namespace Gurux.Device
{
    partial class GXDeviceProfileDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXDeviceProfileDlg));
            this.CustomCB = new System.Windows.Forms.CheckBox();
            this.DownloadCB = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ShowEarlierVersionsCB = new System.Windows.Forms.CheckBox();
            this.SearchTB = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.PublisherImage = new System.Windows.Forms.Panel();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.DeviceProfiles = new System.Windows.Forms.ListView();
            this.NameCH = new System.Windows.Forms.ColumnHeader();
            this.ProtocolCH = new System.Windows.Forms.ColumnHeader();
            this.ManufacturerCH = new System.Windows.Forms.ColumnHeader();
            this.ModelCH = new System.Windows.Forms.ColumnHeader();
            this.VersionCH = new System.Windows.Forms.ColumnHeader();
            this.TemplateVersionCH = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.RemoveMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ProgresssTimer = new System.Windows.Forms.Timer(this.components);
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CustomCB
            // 
            this.CustomCB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CustomCB.AutoSize = true;
            this.CustomCB.Checked = true;
            this.CustomCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CustomCB.Location = new System.Drawing.Point(340, 30);
            this.CustomCB.Name = "CustomCB";
            this.CustomCB.Size = new System.Drawing.Size(61, 17);
            this.CustomCB.TabIndex = 3;
            this.CustomCB.Text = "Custom";
            this.CustomCB.UseVisualStyleBackColor = true;
            this.CustomCB.CheckedChanged += new System.EventHandler(this.ShowTemplates);
            // 
            // DownloadCB
            // 
            this.DownloadCB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DownloadCB.AutoSize = true;
            this.DownloadCB.Checked = true;
            this.DownloadCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DownloadCB.Location = new System.Drawing.Point(407, 30);
            this.DownloadCB.Name = "DownloadCB";
            this.DownloadCB.Size = new System.Drawing.Size(74, 17);
            this.DownloadCB.TabIndex = 4;
            this.DownloadCB.Text = "Download";
            this.DownloadCB.UseVisualStyleBackColor = true;
            this.DownloadCB.CheckedChanged += new System.EventHandler(this.ShowTemplates);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ShowEarlierVersionsCB);
            this.panel2.Controls.Add(this.DownloadCB);
            this.panel2.Controls.Add(this.SearchTB);
            this.panel2.Controls.Add(this.CustomCB);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(493, 50);
            this.panel2.TabIndex = 7;
            // 
            // ShowEarlierVersionsCB
            // 
            this.ShowEarlierVersionsCB.AutoSize = true;
            this.ShowEarlierVersionsCB.Location = new System.Drawing.Point(8, 27);
            this.ShowEarlierVersionsCB.Name = "ShowEarlierVersionsCB";
            this.ShowEarlierVersionsCB.Size = new System.Drawing.Size(126, 17);
            this.ShowEarlierVersionsCB.TabIndex = 1;
            this.ShowEarlierVersionsCB.Text = "Show earlier versions";
            this.ShowEarlierVersionsCB.UseVisualStyleBackColor = true;
            this.ShowEarlierVersionsCB.CheckedChanged += new System.EventHandler(this.ShowTemplates);
            // 
            // SearchTB
            // 
            this.SearchTB.AcceptsReturn = true;
            this.SearchTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchTB.Location = new System.Drawing.Point(6, 3);
            this.SearchTB.Name = "SearchTB";
            this.SearchTB.Size = new System.Drawing.Size(484, 20);
            this.SearchTB.TabIndex = 0;
            this.SearchTB.Leave += new System.EventHandler(this.SearchTB_Leave);
            this.SearchTB.Enter += new System.EventHandler(this.SearchTB_Enter);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.progressBar1);
            this.panel3.Controls.Add(this.PublisherImage);
            this.panel3.Controls.Add(this.OKBtn);
            this.panel3.Controls.Add(this.CancelBtn);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 258);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(493, 44);
            this.panel3.TabIndex = 8;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(49, 9);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(238, 23);
            this.progressBar1.TabIndex = 27;
            this.progressBar1.Visible = false;
            // 
            // PublisherImage
            // 
            this.PublisherImage.BackColor = System.Drawing.Color.Transparent;
            this.PublisherImage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("PublisherImage.BackgroundImage")));
            this.PublisherImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PublisherImage.Location = new System.Drawing.Point(3, 1);
            this.PublisherImage.Name = "PublisherImage";
            this.PublisherImage.Size = new System.Drawing.Size(40, 40);
            this.PublisherImage.TabIndex = 26;
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBtn.Location = new System.Drawing.Point(305, 8);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(80, 24);
            this.OKBtn.TabIndex = 6;
            this.OKBtn.Text = "OK";
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(401, 8);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(80, 24);
            this.CancelBtn.TabIndex = 7;
            this.CancelBtn.Text = global::Gurux.Device.Properties.Resources.Cancel;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // DeviceProfiles
            // 
            this.DeviceProfiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameCH,
            this.ProtocolCH,
            this.ManufacturerCH,
            this.ModelCH,
            this.VersionCH,
            this.TemplateVersionCH});
            this.DeviceProfiles.ContextMenuStrip = this.contextMenuStrip1;
            this.DeviceProfiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DeviceProfiles.FullRowSelect = true;
            this.DeviceProfiles.HideSelection = false;
            this.DeviceProfiles.Location = new System.Drawing.Point(0, 50);
            this.DeviceProfiles.Name = "DeviceProfiles";
            this.DeviceProfiles.Size = new System.Drawing.Size(493, 208);
            this.DeviceProfiles.TabIndex = 5;
            this.DeviceProfiles.UseCompatibleStateImageBehavior = false;
            this.DeviceProfiles.View = System.Windows.Forms.View.Details;
            this.DeviceProfiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DeviceProfiles_MouseDoubleClick);
            // 
            // NameCH
            // 
            this.NameCH.Text = "Name";
            this.NameCH.Width = 111;
            // 
            // ProtocolCH
            // 
            this.ProtocolCH.Text = "Protocol";
            this.ProtocolCH.Width = 70;
            // 
            // ManufacturerCH
            // 
            this.ManufacturerCH.Text = "Manufacturer";
            this.ManufacturerCH.Width = 88;
            // 
            // ModelCH
            // 
            this.ModelCH.Text = "Model";
            // 
            // VersionCH
            // 
            this.VersionCH.Text = "Version";
            // 
            // TemplateVersionCH
            // 
            this.TemplateVersionCH.Text = "Template Version";
            this.TemplateVersionCH.Width = 96;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RemoveMenu});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(118, 26);
            // 
            // RemoveMenu
            // 
            this.RemoveMenu.Name = "RemoveMenu";
            this.RemoveMenu.Size = new System.Drawing.Size(117, 22);
            this.RemoveMenu.Text = "Remove";
            this.RemoveMenu.Click += new System.EventHandler(this.RemoveMenu_Click);
            // 
            // ProgresssTimer
            // 
            this.ProgresssTimer.Interval = 1000;
            this.ProgresssTimer.Tick += new System.EventHandler(this.ProgresssTimer_Tick);
            // 
            // GXDeviceProfileDlg
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(493, 302);
            this.ControlBox = false;
            this.Controls.Add(this.DeviceProfiles);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Name = "GXDeviceProfileDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Device Profile";
            this.Load += new System.EventHandler(this.DeviceDlg_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox DownloadCB;
        private System.Windows.Forms.CheckBox CustomCB;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox SearchTB;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        internal System.Windows.Forms.ListView DeviceProfiles;
        private System.Windows.Forms.ColumnHeader NameCH;
        private System.Windows.Forms.ColumnHeader ProtocolCH;
        private System.Windows.Forms.ColumnHeader ManufacturerCH;
        private System.Windows.Forms.ColumnHeader ModelCH;
        private System.Windows.Forms.ColumnHeader VersionCH;
        private System.Windows.Forms.ColumnHeader TemplateVersionCH;
        private System.Windows.Forms.CheckBox ShowEarlierVersionsCB;
        private System.Windows.Forms.Panel PublisherImage;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem RemoveMenu;
        private System.Windows.Forms.Timer ProgresssTimer;
    }
}