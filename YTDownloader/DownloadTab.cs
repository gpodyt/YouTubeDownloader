using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace YTDownloader
{
    public partial class DownloadTab : Form
    {
        private delegate void enableDownloadButtonDelegate(bool enable);
        public void enableDownloadButtonSafe(bool enable)
        {
            if (InvokeRequired)
            {
                var d = new enableDownloadButtonDelegate(enableDownloadButtonSafe);
                Invoke(d, new object[] { enable });
            }
            else
            {
                btnDownload.Enabled = enable;
            }
        }
        Font boldFont = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
        //for quality selection custom draw
        public DownloadTab()
        {
            InitializeComponent();
        }

        private void DownloadTab_Load(object sender, EventArgs e)
        {
            lblEnterUrl.Text = AllUserConfig.languageRM.GetString("lblEnterUrl");
            lblDownloadQuality.Text = AllUserConfig.languageRM.GetString("lblDownloadQuality");
            lblSaveTo.Text = AllUserConfig.languageRM.GetString("lblSaveTo");
            gbTrim.Text = AllUserConfig.languageRM.GetString("gbTrim");
            btnPaste.Text = AllUserConfig.languageRM.GetString("btnPaste");
            llDownloadLocation.Text = AllUserConfig.downloadLocation;
            lblStartTime.Text = AllUserConfig.languageRM.GetString("lblStartTime");
            lblEndTime.Text = AllUserConfig.languageRM.GetString("lblEndTime");
            lblTrimInfo.Text = AllUserConfig.languageRM.GetString("lblTrimInfo");
            cb60fps.Text = AllUserConfig.languageRM.GetString("cb60fps");

            btnDownload.Text = AllUserConfig.languageRM.GetString("btnDownload");
            btnDownload.BackColor = Color.FromArgb(205, 32, 31);
            btnDownload.FlatStyle = FlatStyle.Flat;
            btnDownload.FlatAppearance.BorderSize = 0;
            btnDownload.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);

            
            ddQualitySelection.Items.Add(AllUserConfig.languageRM.GetString("video"));
            ddQualitySelection.Items.Add(AllUserConfig.languageRM.GetString("best"));
            ddQualitySelection.Items.Add("2160p (4K)");
            ddQualitySelection.Items.Add("1440p");
            ddQualitySelection.Items.Add("1080p (Full HD)");
            ddQualitySelection.Items.Add("720p");
            ddQualitySelection.Items.Add("480p");
            ddQualitySelection.Items.Add("360p");
            ddQualitySelection.Items.Add("240p");
            ddQualitySelection.Items.Add("144p");
            ddQualitySelection.Items.Add(AllUserConfig.languageRM.GetString("audio"));
            ddQualitySelection.Items.Add("mp3");

            ddQualitySelection.SelectedIndex = 1;

            ttForce60fps.SetToolTip(cb60fps, AllUserConfig.languageRM.GetString("ttForce60fps"));
        }

        private void tbLinks_MouseEnter(object sender, EventArgs e)
        {
            pasteLinkIntoTextBox();
        }

        private void pasteLinkIntoTextBox()
        {
            string link = Clipboard.GetText();
            if (link.Contains("youtube.com/watch") || link.Contains("youtube.com/playlist") || link.Contains("youtu.be/"))
            {
                if (tbLinks.Text.Contains(link))
                    return;
                if (tbLinks.Text.Length != 0)
                    tbLinks.AppendText("\r\n" + link);
                else
                    tbLinks.AppendText(link);
            }
        }
        private void tbLinks_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')
                e.Handled = true;
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            pasteLinkIntoTextBox();
        }

        private void ddQualitySelection_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                if (e.Index == 0 || e.Index == 10)
                {
                    e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
                    e.Graphics.DrawString(ddQualitySelection.Items[e.Index].ToString(), boldFont, Brushes.Black, e.Bounds);
                }
                else
                {
                    e.DrawBackground();
                    Brush br = ((e.State & DrawItemState.Selected) > 0) ? SystemBrushes.HighlightText : SystemBrushes.ControlText;
                    e.Graphics.DrawString(ddQualitySelection.Items[e.Index].ToString(), ddQualitySelection.Font, br, e.Bounds);
                    e.DrawFocusRectangle();
                }
            }
        }

        private void ddQualitySelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddQualitySelection.SelectedIndex == 0 || ddQualitySelection.SelectedIndex == 10)
                ddQualitySelection.SelectedIndex = AllUserConfig.lastSelectedFormatIndex;
            btnDownload.Focus();
            AllUserConfig.lastSelectedFormatIndex = ddQualitySelection.SelectedIndex;
            if (ddQualitySelection.SelectedIndex == 11)
            {
                gbTrim.Enabled = true;
                cb60fps.Enabled = false;
            }
            else
            {
                gbTrim.Enabled = false;
                cb60fps.Enabled = true;
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(AllUserConfig.downloadLocation))
            {
                MessageBox.Show(AllUserConfig.languageRM.GetString("directoryDoesntExist"));
                return;
            }
            Button activityTabButton = Application.OpenForms["Main"].Controls["btnActivityTab"] as Button;
            Button downloadTabButton = Application.OpenForms["Main"].Controls["btnDownloadTab"] as Button;
            activityTabButton.BackColor = DefaultBackColor;
            activityTabButton.FlatAppearance.MouseOverBackColor = DefaultBackColor;
            downloadTabButton.BackColor = Color.DarkGray;
            downloadTabButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(152, 152, 152);
            ActivityTab activityTab = Application.OpenForms["ActivityTab"] as ActivityTab;
            activityTab.Activate();
            ActivityTab tempForm = Application.OpenForms["ActivityTab"] as ActivityTab;
            Trim trim = new Trim(tbStartTime.Text, tbEndTime.Text);
            string format;
            switch(ddQualitySelection.SelectedIndex)
            {
                case 1: format = "best"; break;
                case 2: format = "2160p"; break;
                case 3: format = "1440p"; break;
                case 4: format = "1080p"; break;
                case 5: format = "720p"; break;
                case 6: format = "480p"; break;
                case 7: format = "360p"; break;
                case 8: format = "240p"; break;
                case 9: format = "144p"; break;
                case 11: format = "mp3"; break;
                default: format = "best"; break;
            }
            if (cb60fps.Enabled && cb60fps.Checked && format != "best")
                format = format + "60";
            else if ((cb60fps.Enabled && cb60fps.Checked && format == "best"))
                format = "p60";
            if (trim.isValid())
                tempForm.startAddingVideos(IdExtractor.GetIdsAndTypes(tbLinks.Text), llDownloadLocation.Text, format, trim);
            else
                tempForm.startAddingVideos(IdExtractor.GetIdsAndTypes(tbLinks.Text), llDownloadLocation.Text, format, null);
            tbLinks.Text = "";
            tbStartTime.Text = "00:00:00";
            tbEndTime.Text = "00:00:00";
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", AllUserConfig.downloadLocation);
            btnDownload.Focus();
        }

        private void llDownloadLocation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                llDownloadLocation.Text = dialog.SelectedPath;
                AllUserConfig.downloadLocation = dialog.SelectedPath;
            }
        }
    }
}
