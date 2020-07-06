using System;
using System.Windows.Forms;

namespace YTDownloader
{
    public partial class PlaylistAndVideoQuestionForm : Form
    {
        public PlaylistAndVideoQuestionForm()
        {
            InitializeComponent();
        }

        private void PlaylistAndVideoQuestionForm_Load(object sender, EventArgs e)
        {
            tbText.Text = AllUserConfig.languageRM.GetString("playlistAndVideoText");
            btnPlaylist.Text = AllUserConfig.languageRM.GetString("btnWholePlaylist");
            btnVideo.Text = AllUserConfig.languageRM.GetString("btnVideoOnly");
            Text = AllUserConfig.languageRM.GetString("PlaylistVideoForm");
        }
    }
}
