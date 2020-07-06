namespace YTDownloader
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.lblLanguage = new System.Windows.Forms.Label();
            this.lblMaxSimDownloads = new System.Windows.Forms.Label();
            this.lblRestartProgram = new System.Windows.Forms.Label();
            this.lblDownloadsWarning1 = new System.Windows.Forms.Label();
            this.lblDownloadsWarning2 = new System.Windows.Forms.Label();
            this.cbLanguage = new System.Windows.Forms.ComboBox();
            this.numericDownloads = new System.Windows.Forms.NumericUpDown();
            this.lblCannotChangeSetting = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericDownloads)).BeginInit();
            this.SuspendLayout();
            // 
            // lblLanguage
            // 
            this.lblLanguage.Location = new System.Drawing.Point(269, 27);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(118, 25);
            this.lblLanguage.TabIndex = 0;
            this.lblLanguage.Text = "label1";
            this.lblLanguage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMaxSimDownloads
            // 
            this.lblMaxSimDownloads.Location = new System.Drawing.Point(28, 91);
            this.lblMaxSimDownloads.Name = "lblMaxSimDownloads";
            this.lblMaxSimDownloads.Size = new System.Drawing.Size(359, 30);
            this.lblMaxSimDownloads.TabIndex = 1;
            this.lblMaxSimDownloads.Text = "label1";
            this.lblMaxSimDownloads.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRestartProgram
            // 
            this.lblRestartProgram.ForeColor = System.Drawing.Color.Red;
            this.lblRestartProgram.Location = new System.Drawing.Point(16, 60);
            this.lblRestartProgram.Name = "lblRestartProgram";
            this.lblRestartProgram.Size = new System.Drawing.Size(574, 25);
            this.lblRestartProgram.TabIndex = 2;
            this.lblRestartProgram.Text = "label1";
            this.lblRestartProgram.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDownloadsWarning1
            // 
            this.lblDownloadsWarning1.ForeColor = System.Drawing.Color.Red;
            this.lblDownloadsWarning1.Location = new System.Drawing.Point(20, 128);
            this.lblDownloadsWarning1.Name = "lblDownloadsWarning1";
            this.lblDownloadsWarning1.Size = new System.Drawing.Size(570, 25);
            this.lblDownloadsWarning1.TabIndex = 3;
            this.lblDownloadsWarning1.Text = "label1";
            this.lblDownloadsWarning1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDownloadsWarning2
            // 
            this.lblDownloadsWarning2.ForeColor = System.Drawing.Color.Red;
            this.lblDownloadsWarning2.Location = new System.Drawing.Point(20, 150);
            this.lblDownloadsWarning2.Name = "lblDownloadsWarning2";
            this.lblDownloadsWarning2.Size = new System.Drawing.Size(570, 25);
            this.lblDownloadsWarning2.TabIndex = 4;
            this.lblDownloadsWarning2.Text = "label1";
            this.lblDownloadsWarning2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbLanguage
            // 
            this.cbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLanguage.FormattingEnabled = true;
            this.cbLanguage.Location = new System.Drawing.Point(393, 26);
            this.cbLanguage.Name = "cbLanguage";
            this.cbLanguage.Size = new System.Drawing.Size(170, 28);
            this.cbLanguage.TabIndex = 5;
            this.cbLanguage.TabStop = false;
            this.cbLanguage.SelectedIndexChanged += new System.EventHandler(this.cbLanguage_SelectedIndexChanged);
            // 
            // numericDownloads
            // 
            this.numericDownloads.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericDownloads.Location = new System.Drawing.Point(393, 94);
            this.numericDownloads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericDownloads.Name = "numericDownloads";
            this.numericDownloads.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.numericDownloads.Size = new System.Drawing.Size(170, 26);
            this.numericDownloads.TabIndex = 6;
            this.numericDownloads.TabStop = false;
            this.numericDownloads.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericDownloads.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericDownloads.ValueChanged += new System.EventHandler(this.numericDownloads_ValueChanged);
            // 
            // lblCannotChangeSetting
            // 
            this.lblCannotChangeSetting.ForeColor = System.Drawing.SystemColors.InfoText;
            this.lblCannotChangeSetting.Location = new System.Drawing.Point(20, 128);
            this.lblCannotChangeSetting.Name = "lblCannotChangeSetting";
            this.lblCannotChangeSetting.Size = new System.Drawing.Size(570, 25);
            this.lblCannotChangeSetting.TabIndex = 7;
            this.lblCannotChangeSetting.Text = "label1";
            this.lblCannotChangeSetting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(257, 185);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 28);
            this.button1.TabIndex = 9;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 225);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblCannotChangeSetting);
            this.Controls.Add(this.numericDownloads);
            this.Controls.Add(this.cbLanguage);
            this.Controls.Add(this.lblDownloadsWarning2);
            this.Controls.Add(this.lblDownloadsWarning1);
            this.Controls.Add(this.lblRestartProgram);
            this.Controls.Add(this.lblMaxSimDownloads);
            this.Controls.Add(this.lblLanguage);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "Settings";
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Settings_FormClosing);
            this.Load += new System.EventHandler(this.Settings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericDownloads)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.Label lblMaxSimDownloads;
        private System.Windows.Forms.Label lblRestartProgram;
        private System.Windows.Forms.Label lblDownloadsWarning1;
        private System.Windows.Forms.Label lblDownloadsWarning2;
        private System.Windows.Forms.ComboBox cbLanguage;
        private System.Windows.Forms.NumericUpDown numericDownloads;
        private System.Windows.Forms.Label lblCannotChangeSetting;
        private System.Windows.Forms.Button button1;
    }
}