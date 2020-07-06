namespace YTDownloader
{
    partial class AutoUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoUpdate));
            this.lblAutoUpdating = new System.Windows.Forms.Label();
            this.lblDoNotClose = new System.Windows.Forms.Label();
            this.lblItWillCloseAuto = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblAutoUpdating
            // 
            this.lblAutoUpdating.Location = new System.Drawing.Point(12, 32);
            this.lblAutoUpdating.Name = "lblAutoUpdating";
            this.lblAutoUpdating.Size = new System.Drawing.Size(438, 40);
            this.lblAutoUpdating.TabIndex = 0;
            this.lblAutoUpdating.Text = "label1";
            this.lblAutoUpdating.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDoNotClose
            // 
            this.lblDoNotClose.Location = new System.Drawing.Point(12, 121);
            this.lblDoNotClose.Name = "lblDoNotClose";
            this.lblDoNotClose.Size = new System.Drawing.Size(438, 40);
            this.lblDoNotClose.TabIndex = 1;
            this.lblDoNotClose.Text = "label1";
            this.lblDoNotClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblItWillCloseAuto
            // 
            this.lblItWillCloseAuto.Location = new System.Drawing.Point(12, 161);
            this.lblItWillCloseAuto.Name = "lblItWillCloseAuto";
            this.lblItWillCloseAuto.Size = new System.Drawing.Size(438, 40);
            this.lblItWillCloseAuto.TabIndex = 2;
            this.lblItWillCloseAuto.Text = "label1";
            this.lblItWillCloseAuto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AutoUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 210);
            this.ControlBox = false;
            this.Controls.Add(this.lblItWillCloseAuto);
            this.Controls.Add(this.lblDoNotClose);
            this.Controls.Add(this.lblAutoUpdating);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "AutoUpdate";
            this.Text = "AutoUpdate";
            this.Load += new System.EventHandler(this.AutoUpdate_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblAutoUpdating;
        private System.Windows.Forms.Label lblDoNotClose;
        private System.Windows.Forms.Label lblItWillCloseAuto;
    }
}