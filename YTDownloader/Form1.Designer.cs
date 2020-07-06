namespace YTDownloader
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.btnDownloadTab = new System.Windows.Forms.Button();
            this.btnActivityTab = new System.Windows.Forms.Button();
            this.menuPicture = new System.Windows.Forms.PictureBox();
            this.pbSettings = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.menuPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSettings)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDownloadTab
            // 
            this.btnDownloadTab.Location = new System.Drawing.Point(7, 135);
            this.btnDownloadTab.Name = "btnDownloadTab";
            this.btnDownloadTab.Size = new System.Drawing.Size(108, 28);
            this.btnDownloadTab.TabIndex = 1;
            this.btnDownloadTab.TabStop = false;
            this.btnDownloadTab.Text = "button1";
            this.btnDownloadTab.UseVisualStyleBackColor = true;
            this.btnDownloadTab.Click += new System.EventHandler(this.btnDownloadTab_Click);
            // 
            // btnActivityTab
            // 
            this.btnActivityTab.Location = new System.Drawing.Point(121, 135);
            this.btnActivityTab.Name = "btnActivityTab";
            this.btnActivityTab.Size = new System.Drawing.Size(108, 28);
            this.btnActivityTab.TabIndex = 3;
            this.btnActivityTab.TabStop = false;
            this.btnActivityTab.Text = "button1";
            this.btnActivityTab.UseVisualStyleBackColor = true;
            this.btnActivityTab.Click += new System.EventHandler(this.btnActivityTab_Click);
            // 
            // menuPicture
            // 
            this.menuPicture.Location = new System.Drawing.Point(2, 2);
            this.menuPicture.Name = "menuPicture";
            this.menuPicture.Size = new System.Drawing.Size(883, 161);
            this.menuPicture.TabIndex = 5;
            this.menuPicture.TabStop = false;
            // 
            // pbSettings
            // 
            this.pbSettings.Image = global::YTDownloader.Properties.Resources.settingsIcon;
            this.pbSettings.Location = new System.Drawing.Point(751, 48);
            this.pbSettings.Name = "pbSettings";
            this.pbSettings.Size = new System.Drawing.Size(50, 50);
            this.pbSettings.TabIndex = 9;
            this.pbSettings.TabStop = false;
            this.pbSettings.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbSettings_MouseClick);
            this.pbSettings.MouseEnter += new System.EventHandler(this.pbSettings_MouseEnter);
            this.pbSettings.MouseLeave += new System.EventHandler(this.pbSettings_MouseLeave);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(881, 596);
            this.Controls.Add(this.pbSettings);
            this.Controls.Add(this.btnActivityTab);
            this.Controls.Add(this.btnDownloadTab);
            this.Controls.Add(this.menuPicture);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.menuPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSettings)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDownloadTab;
        private System.Windows.Forms.Button btnActivityTab;
        private System.Windows.Forms.PictureBox menuPicture;
        private System.Windows.Forms.PictureBox pbSettings;
    }
}

