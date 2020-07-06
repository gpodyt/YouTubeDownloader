namespace YTDownloader
{
    partial class PlaylistAndVideoQuestionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlaylistAndVideoQuestionForm));
            this.btnPlaylist = new System.Windows.Forms.Button();
            this.btnVideo = new System.Windows.Forms.Button();
            this.tbText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnPlaylist
            // 
            this.btnPlaylist.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnPlaylist.Location = new System.Drawing.Point(23, 144);
            this.btnPlaylist.Name = "btnPlaylist";
            this.btnPlaylist.Size = new System.Drawing.Size(207, 38);
            this.btnPlaylist.TabIndex = 0;
            this.btnPlaylist.UseVisualStyleBackColor = true;
            // 
            // btnVideo
            // 
            this.btnVideo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnVideo.Location = new System.Drawing.Point(258, 144);
            this.btnVideo.Name = "btnVideo";
            this.btnVideo.Size = new System.Drawing.Size(208, 38);
            this.btnVideo.TabIndex = 1;
            this.btnVideo.Text = "button1";
            this.btnVideo.UseVisualStyleBackColor = true;
            // 
            // tbText
            // 
            this.tbText.BackColor = System.Drawing.SystemColors.Control;
            this.tbText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbText.Location = new System.Drawing.Point(23, 12);
            this.tbText.Multiline = true;
            this.tbText.Name = "tbText";
            this.tbText.ReadOnly = true;
            this.tbText.Size = new System.Drawing.Size(443, 126);
            this.tbText.TabIndex = 2;
            this.tbText.Text = "asdasd";
            this.tbText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // PlaylistAndVideoQuestionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 194);
            this.Controls.Add(this.tbText);
            this.Controls.Add(this.btnVideo);
            this.Controls.Add(this.btnPlaylist);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "PlaylistAndVideoQuestionForm";
            this.Text = "PlaylistAndVideoQuestionForm";
            this.Load += new System.EventHandler(this.PlaylistAndVideoQuestionForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPlaylist;
        private System.Windows.Forms.Button btnVideo;
        private System.Windows.Forms.TextBox tbText;
    }
}