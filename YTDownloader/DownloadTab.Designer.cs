namespace YTDownloader
{
    partial class DownloadTab
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
            this.btnDownload = new System.Windows.Forms.Button();
            this.lblEnterUrl = new System.Windows.Forms.Label();
            this.tbLinks = new System.Windows.Forms.TextBox();
            this.btnPaste = new System.Windows.Forms.Button();
            this.ddQualitySelection = new System.Windows.Forms.ComboBox();
            this.lblDownloadQuality = new System.Windows.Forms.Label();
            this.lblSaveTo = new System.Windows.Forms.Label();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.llDownloadLocation = new System.Windows.Forms.LinkLabel();
            this.gbTrim = new System.Windows.Forms.GroupBox();
            this.lblTrimInfo = new System.Windows.Forms.Label();
            this.tbEndTime = new System.Windows.Forms.MaskedTextBox();
            this.lblEndTime = new System.Windows.Forms.Label();
            this.tbStartTime = new System.Windows.Forms.MaskedTextBox();
            this.lblStartTime = new System.Windows.Forms.Label();
            this.cb60fps = new System.Windows.Forms.CheckBox();
            this.ttForce60fps = new System.Windows.Forms.ToolTip(this.components);
            this.gbTrim.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDownload
            // 
            this.btnDownload.ForeColor = System.Drawing.SystemColors.Control;
            this.btnDownload.Location = new System.Drawing.Point(694, 388);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(171, 31);
            this.btnDownload.TabIndex = 0;
            this.btnDownload.Text = "button1";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // lblEnterUrl
            // 
            this.lblEnterUrl.AutoSize = true;
            this.lblEnterUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEnterUrl.Location = new System.Drawing.Point(12, 21);
            this.lblEnterUrl.Name = "lblEnterUrl";
            this.lblEnterUrl.Size = new System.Drawing.Size(51, 20);
            this.lblEnterUrl.TabIndex = 1;
            this.lblEnterUrl.Text = "label1";
            // 
            // tbLinks
            // 
            this.tbLinks.Location = new System.Drawing.Point(16, 47);
            this.tbLinks.Multiline = true;
            this.tbLinks.Name = "tbLinks";
            this.tbLinks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLinks.Size = new System.Drawing.Size(724, 92);
            this.tbLinks.TabIndex = 2;
            this.tbLinks.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbLinks_KeyPress);
            this.tbLinks.MouseEnter += new System.EventHandler(this.tbLinks_MouseEnter);
            // 
            // btnPaste
            // 
            this.btnPaste.Location = new System.Drawing.Point(746, 49);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(119, 90);
            this.btnPaste.TabIndex = 3;
            this.btnPaste.Text = "button1";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // ddQualitySelection
            // 
            this.ddQualitySelection.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ddQualitySelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddQualitySelection.FormattingEnabled = true;
            this.ddQualitySelection.Location = new System.Drawing.Point(389, 157);
            this.ddQualitySelection.Name = "ddQualitySelection";
            this.ddQualitySelection.Size = new System.Drawing.Size(180, 27);
            this.ddQualitySelection.TabIndex = 4;
            this.ddQualitySelection.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ddQualitySelection_DrawItem);
            this.ddQualitySelection.SelectedIndexChanged += new System.EventHandler(this.ddQualitySelection_SelectedIndexChanged);
            // 
            // lblDownloadQuality
            // 
            this.lblDownloadQuality.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDownloadQuality.Location = new System.Drawing.Point(202, 160);
            this.lblDownloadQuality.Name = "lblDownloadQuality";
            this.lblDownloadQuality.Size = new System.Drawing.Size(181, 27);
            this.lblDownloadQuality.TabIndex = 5;
            this.lblDownloadQuality.Text = "label1";
            this.lblDownloadQuality.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSaveTo
            // 
            this.lblSaveTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSaveTo.Location = new System.Drawing.Point(158, 197);
            this.lblSaveTo.Name = "lblSaveTo";
            this.lblSaveTo.Size = new System.Drawing.Size(181, 27);
            this.lblSaveTo.TabIndex = 6;
            this.lblSaveTo.Text = "label1";
            this.lblSaveTo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Image = global::YTDownloader.Properties.Resources.openContainingFolder;
            this.btnOpenFolder.Location = new System.Drawing.Point(345, 190);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(34, 34);
            this.btnOpenFolder.TabIndex = 7;
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // llDownloadLocation
            // 
            this.llDownloadLocation.AutoSize = true;
            this.llDownloadLocation.Location = new System.Drawing.Point(385, 197);
            this.llDownloadLocation.Name = "llDownloadLocation";
            this.llDownloadLocation.Size = new System.Drawing.Size(80, 20);
            this.llDownloadLocation.TabIndex = 8;
            this.llDownloadLocation.TabStop = true;
            this.llDownloadLocation.Text = "linkLabel1";
            this.llDownloadLocation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llDownloadLocation_LinkClicked);
            // 
            // gbTrim
            // 
            this.gbTrim.Controls.Add(this.lblTrimInfo);
            this.gbTrim.Controls.Add(this.tbEndTime);
            this.gbTrim.Controls.Add(this.lblEndTime);
            this.gbTrim.Controls.Add(this.tbStartTime);
            this.gbTrim.Controls.Add(this.lblStartTime);
            this.gbTrim.Location = new System.Drawing.Point(16, 230);
            this.gbTrim.Name = "gbTrim";
            this.gbTrim.Size = new System.Drawing.Size(849, 152);
            this.gbTrim.TabIndex = 9;
            this.gbTrim.TabStop = false;
            this.gbTrim.Text = "groupBox1";
            // 
            // lblTrimInfo
            // 
            this.lblTrimInfo.Location = new System.Drawing.Point(0, 117);
            this.lblTrimInfo.Name = "lblTrimInfo";
            this.lblTrimInfo.Size = new System.Drawing.Size(843, 32);
            this.lblTrimInfo.TabIndex = 6;
            this.lblTrimInfo.Text = "label1";
            this.lblTrimInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbEndTime
            // 
            this.tbEndTime.Location = new System.Drawing.Point(373, 69);
            this.tbEndTime.Mask = "00:00:00";
            this.tbEndTime.Name = "tbEndTime";
            this.tbEndTime.Size = new System.Drawing.Size(180, 26);
            this.tbEndTime.TabIndex = 5;
            this.tbEndTime.Text = "000000";
            this.tbEndTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbEndTime.ValidatingType = typeof(System.DateTime);
            // 
            // lblEndTime
            // 
            this.lblEndTime.Location = new System.Drawing.Point(225, 71);
            this.lblEndTime.Name = "lblEndTime";
            this.lblEndTime.Size = new System.Drawing.Size(142, 24);
            this.lblEndTime.TabIndex = 4;
            this.lblEndTime.Text = "label1";
            this.lblEndTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbStartTime
            // 
            this.tbStartTime.Location = new System.Drawing.Point(373, 32);
            this.tbStartTime.Mask = "00:00:00";
            this.tbStartTime.Name = "tbStartTime";
            this.tbStartTime.Size = new System.Drawing.Size(180, 26);
            this.tbStartTime.TabIndex = 3;
            this.tbStartTime.Text = "000000";
            this.tbStartTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbStartTime.ValidatingType = typeof(System.DateTime);
            // 
            // lblStartTime
            // 
            this.lblStartTime.Location = new System.Drawing.Point(221, 34);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.Size = new System.Drawing.Size(146, 24);
            this.lblStartTime.TabIndex = 2;
            this.lblStartTime.Text = "label1";
            this.lblStartTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cb60fps
            // 
            this.cb60fps.AutoSize = true;
            this.cb60fps.Location = new System.Drawing.Point(584, 159);
            this.cb60fps.Name = "cb60fps";
            this.cb60fps.Size = new System.Drawing.Size(106, 24);
            this.cb60fps.TabIndex = 10;
            this.cb60fps.Text = "checkBox1";
            this.cb60fps.UseVisualStyleBackColor = true;
            // 
            // ttForce60fps
            // 
            this.ttForce60fps.AutoPopDelay = 7000;
            this.ttForce60fps.InitialDelay = 500;
            this.ttForce60fps.ReshowDelay = 100;
            // 
            // DownloadTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 431);
            this.Controls.Add(this.cb60fps);
            this.Controls.Add(this.gbTrim);
            this.Controls.Add(this.llDownloadLocation);
            this.Controls.Add(this.btnOpenFolder);
            this.Controls.Add(this.lblSaveTo);
            this.Controls.Add(this.lblDownloadQuality);
            this.Controls.Add(this.ddQualitySelection);
            this.Controls.Add(this.btnPaste);
            this.Controls.Add(this.tbLinks);
            this.Controls.Add(this.lblEnterUrl);
            this.Controls.Add(this.btnDownload);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "DownloadTab";
            this.Text = "DownloadTab";
            this.Load += new System.EventHandler(this.DownloadTab_Load);
            this.gbTrim.ResumeLayout(false);
            this.gbTrim.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Label lblEnterUrl;
        private System.Windows.Forms.TextBox tbLinks;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.ComboBox ddQualitySelection;
        private System.Windows.Forms.Label lblDownloadQuality;
        private System.Windows.Forms.Label lblSaveTo;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.LinkLabel llDownloadLocation;
        private System.Windows.Forms.GroupBox gbTrim;
        private System.Windows.Forms.MaskedTextBox tbStartTime;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.MaskedTextBox tbEndTime;
        private System.Windows.Forms.Label lblEndTime;
        private System.Windows.Forms.Label lblTrimInfo;
        private System.Windows.Forms.CheckBox cb60fps;
        private System.Windows.Forms.ToolTip ttForce60fps;
    }
}