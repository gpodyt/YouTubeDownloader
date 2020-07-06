namespace YTDownloader
{
    partial class ActivityTab
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
            this.btnStartPauseDownload = new System.Windows.Forms.Button();
            this.btnCancelDownload = new System.Windows.Forms.Button();
            this.btnDeleteActivityDownload = new System.Windows.Forms.Button();
            this.btnOpenFileLocation = new System.Windows.Forms.Button();
            this.lvActiveDownloads = new System.Windows.Forms.ListView();
            this.cmVideo = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnDeleteFileDownload = new System.Windows.Forms.Button();
            this.ttStartPause = new System.Windows.Forms.ToolTip(this.components);
            this.ttStop = new System.Windows.Forms.ToolTip(this.components);
            this.ttOpenFolder = new System.Windows.Forms.ToolTip(this.components);
            this.ttDeleteActivity = new System.Windows.Forms.ToolTip(this.components);
            this.ttDeleteFile = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // btnStartPauseDownload
            // 
            this.btnStartPauseDownload.Location = new System.Drawing.Point(13, 14);
            this.btnStartPauseDownload.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnStartPauseDownload.Name = "btnStartPauseDownload";
            this.btnStartPauseDownload.Size = new System.Drawing.Size(39, 39);
            this.btnStartPauseDownload.TabIndex = 0;
            this.btnStartPauseDownload.TabStop = false;
            this.btnStartPauseDownload.UseVisualStyleBackColor = true;
            this.btnStartPauseDownload.Click += new System.EventHandler(this.btnStartPauseDownload_Click);
            // 
            // btnCancelDownload
            // 
            this.btnCancelDownload.Location = new System.Drawing.Point(60, 14);
            this.btnCancelDownload.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancelDownload.Name = "btnCancelDownload";
            this.btnCancelDownload.Size = new System.Drawing.Size(39, 39);
            this.btnCancelDownload.TabIndex = 2;
            this.btnCancelDownload.TabStop = false;
            this.btnCancelDownload.UseVisualStyleBackColor = true;
            this.btnCancelDownload.Click += new System.EventHandler(this.btnCancelDownload_Click);
            // 
            // btnDeleteActivityDownload
            // 
            this.btnDeleteActivityDownload.Location = new System.Drawing.Point(107, 14);
            this.btnDeleteActivityDownload.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDeleteActivityDownload.Name = "btnDeleteActivityDownload";
            this.btnDeleteActivityDownload.Size = new System.Drawing.Size(39, 39);
            this.btnDeleteActivityDownload.TabIndex = 3;
            this.btnDeleteActivityDownload.TabStop = false;
            this.btnDeleteActivityDownload.UseVisualStyleBackColor = true;
            this.btnDeleteActivityDownload.Click += new System.EventHandler(this.btnDeleteActivityDownload_Click);
            // 
            // btnOpenFileLocation
            // 
            this.btnOpenFileLocation.Location = new System.Drawing.Point(201, 14);
            this.btnOpenFileLocation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOpenFileLocation.Name = "btnOpenFileLocation";
            this.btnOpenFileLocation.Size = new System.Drawing.Size(39, 39);
            this.btnOpenFileLocation.TabIndex = 4;
            this.btnOpenFileLocation.TabStop = false;
            this.btnOpenFileLocation.UseVisualStyleBackColor = true;
            this.btnOpenFileLocation.Click += new System.EventHandler(this.btnOpenFileLocation_Click);
            // 
            // lvActiveDownloads
            // 
            this.lvActiveDownloads.FullRowSelect = true;
            this.lvActiveDownloads.HideSelection = false;
            this.lvActiveDownloads.Location = new System.Drawing.Point(13, 63);
            this.lvActiveDownloads.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lvActiveDownloads.Name = "lvActiveDownloads";
            this.lvActiveDownloads.Size = new System.Drawing.Size(851, 354);
            this.lvActiveDownloads.TabIndex = 5;
            this.lvActiveDownloads.UseCompatibleStateImageBehavior = false;
            this.lvActiveDownloads.View = System.Windows.Forms.View.Details;
            this.lvActiveDownloads.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.lvActiveDownloads_ColumnWidthChanging);
            this.lvActiveDownloads.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvActiveDownloads_KeyDown);
            this.lvActiveDownloads.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvActiveDownloads_MouseClick);
            this.lvActiveDownloads.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvActiveDownloads_MouseDoubleClick);
            this.lvActiveDownloads.MouseCaptureChanged += new System.EventHandler(this.lvActiveDownloads_MouseCaptureChanged);
            this.lvActiveDownloads.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lvActiveDownloads_MouseUp);
            // 
            // cmVideo
            // 
            this.cmVideo.Name = "cmVideo";
            this.cmVideo.Size = new System.Drawing.Size(61, 4);
            this.cmVideo.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.cmVideo_Closed);
            this.cmVideo.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmVideo_ItemClicked);
            // 
            // btnDeleteFileDownload
            // 
            this.btnDeleteFileDownload.Location = new System.Drawing.Point(154, 14);
            this.btnDeleteFileDownload.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDeleteFileDownload.Name = "btnDeleteFileDownload";
            this.btnDeleteFileDownload.Size = new System.Drawing.Size(39, 39);
            this.btnDeleteFileDownload.TabIndex = 7;
            this.btnDeleteFileDownload.TabStop = false;
            this.btnDeleteFileDownload.UseVisualStyleBackColor = true;
            this.btnDeleteFileDownload.Click += new System.EventHandler(this.btnDeleteFileDownload_Click);
            // 
            // ActivityTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 431);
            this.Controls.Add(this.btnDeleteFileDownload);
            this.Controls.Add(this.lvActiveDownloads);
            this.Controls.Add(this.btnOpenFileLocation);
            this.Controls.Add(this.btnDeleteActivityDownload);
            this.Controls.Add(this.btnCancelDownload);
            this.Controls.Add(this.btnStartPauseDownload);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ActivityTab";
            this.Text = "ActivityTab";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ActivityTab_FormClosing);
            this.Load += new System.EventHandler(this.ActivityTab_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStartPauseDownload;
        private System.Windows.Forms.Button btnCancelDownload;
        private System.Windows.Forms.Button btnDeleteActivityDownload;
        private System.Windows.Forms.Button btnOpenFileLocation;
        private System.Windows.Forms.ListView lvActiveDownloads;
        private System.Windows.Forms.ContextMenuStrip cmVideo;
        private System.Windows.Forms.Button btnDeleteFileDownload;
        private System.Windows.Forms.ToolTip ttStartPause;
        private System.Windows.Forms.ToolTip ttStop;
        private System.Windows.Forms.ToolTip ttOpenFolder;
        private System.Windows.Forms.ToolTip ttDeleteActivity;
        private System.Windows.Forms.ToolTip ttDeleteFile;
    }
}