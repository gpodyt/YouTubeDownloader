using System;
using System.Windows.Forms;

namespace YTDownloader
{
    public partial class Settings : Form
    {
        int cbSelectedIndex;
        int simDownloadsNumber;
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            lblCannotChangeSetting.Visible = false;
            lblCannotChangeSetting.Text = AllUserConfig.languageRM.GetString("lblCannotChangeSetting");
            ActivityTab temporary = Application.OpenForms["ActivityTab"] as ActivityTab;
            if(temporary.noOngoingDownloads())
            {
                lblMaxSimDownloads.Enabled = true;
                numericDownloads.Enabled = true;
            }
            else
            {
                lblMaxSimDownloads.Enabled = false;
                numericDownloads.Enabled = false;
                lblCannotChangeSetting.Visible = true;
            }
            Text = AllUserConfig.languageRM.GetString("Settings");
            lblLanguage.Text = AllUserConfig.languageRM.GetString("LanguageLabel");
            lblMaxSimDownloads.Text = AllUserConfig.languageRM.GetString("MaxSimDownLabel");
            lblRestartProgram.Text = AllUserConfig.languageRM.GetString("lblRestartProgram");
            lblDownloadsWarning1.Text = AllUserConfig.languageRM.GetString("lblDownloadsWarning1");
            lblDownloadsWarning2.Text = AllUserConfig.languageRM.GetString("lblDownloadsWarning2");
            cbLanguage.Items.Add("English");
            cbLanguage.Items.Add("Македонски");
            if(AllUserConfig.lang == "mk")
            {
                cbLanguage.SelectedIndex = 1;
                cbSelectedIndex = 1;
            }
            else
            {
                cbLanguage.SelectedIndex = 0;
                cbSelectedIndex = 0;
            }
            numericDownloads.Value = AllUserConfig.maxSimDownloads;
            simDownloadsNumber = AllUserConfig.maxSimDownloads;
            lblDownloadsWarning1.Visible = false;
            lblDownloadsWarning2.Visible = false;
            lblRestartProgram.Visible = false;
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AllUserConfig.maxSimDownloads != (int)numericDownloads.Value)
                MaxNumOfSimVideos.changeNumber((int)numericDownloads.Value);
            AllUserConfig.maxSimDownloads = (int)numericDownloads.Value;
            if (cbLanguage.SelectedIndex == 0)
                AllUserConfig.lang = "en";
            else
                AllUserConfig.lang = "mk";
        }

        private void cbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbLanguage.SelectedIndex != cbSelectedIndex)
                lblRestartProgram.Visible = true;
            else
                lblRestartProgram.Visible = false;
        }

        private void numericDownloads_ValueChanged(object sender, EventArgs e)
        {
            if(numericDownloads.Value>simDownloadsNumber)
            {
                lblDownloadsWarning1.Visible = true;
                lblDownloadsWarning2.Visible = true;
            }
            else
            {
                lblDownloadsWarning1.Visible = false;
                lblDownloadsWarning2.Visible = false;
            }
        }
    }
}
