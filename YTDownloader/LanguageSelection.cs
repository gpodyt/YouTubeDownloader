using System;
using System.Windows.Forms;

namespace YTDownloader
{
    public partial class LanguageSelection : Form
    {
        public LanguageSelection()
        {
            InitializeComponent();
        }

        private void LanguageSelection_Load(object sender, EventArgs e)
        {
            Text = AllUserConfig.languageRM.GetString("LanguageSelection");
            lblLanguage.Text = AllUserConfig.languageRM.GetString("LanguageLabel");
            cbLanguage.Items.Add("English");
            cbLanguage.Items.Add("Македонски");
            cbLanguage.SelectedIndex = 0;
        }

        private void LanguageSelection_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cbLanguage.SelectedIndex == 0)
                AllUserConfig.lang = "en";
            else
                AllUserConfig.lang = "mk";
        }
    }
}
