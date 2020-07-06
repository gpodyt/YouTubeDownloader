using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace YTDownloader
{
    public partial class ActivityTab : Form
    {
        Dictionary<string, Thread> playlistThreads;
        Rectangle progressBarBounds = Rectangle.Empty;
        ImageList progressBars;
        ImageList controlIcons;
        private delegate void UpdateItemDelegate(VideoListItem item);
        private delegate void RemovePlaylistDelegate(VideoListItem item);
        private delegate void startAddingVideoFromPlaylistDelegate(Dictionary<string, string> videos, string Location, string Format);
        private void UpdateItemSafe(VideoListItem item)
        {
            if (lvActiveDownloads.InvokeRequired)
            {
                var d = new UpdateItemDelegate(UpdateItemSafe);
                lvActiveDownloads.Invoke(d, new object[] { item });
            }
            else
            {
                item.SubItems[1].Text = ((Video)item.Tag).getName();
                item.SubItems[2].Text = ((Video)item.Tag).getSizeForUpdate();
                item.SubItems[4].Text = ((Video)item.Tag).getSpeedForUpdate();
                item.SubItems[5].Text = AllUserConfig.languageRM.GetString("status" + ((Video)item.Tag).getStatus());
                item.SubItems[6].Text = ((Video)item.Tag).getETAForUpdate();
                item.PB.changeValue(((Video)item.Tag).getProgress(), ((Video)item.Tag).getStatus());
                item.PB.DrawToBitmap(item.PBBitmap, progressBarBounds);
                progressBars.Images[progressBars.Images.IndexOfKey(item.ImageKey)] = item.PBBitmap;
            }
        }
        private void RemovePlaylistSafe(VideoListItem item)
        {
            if (lvActiveDownloads.InvokeRequired)
            {
                var d = new RemovePlaylistDelegate(RemovePlaylistSafe);
                lvActiveDownloads.Invoke(d, new object[] { item });
            }
            else
            {
                item.Remove();
            }
        }
        private void startAddingVideosFromPlaylistSafe(Dictionary<string, string> videos, string Location, string Format)
        {
            if (lvActiveDownloads.InvokeRequired)
            {
                var d = new startAddingVideoFromPlaylistDelegate(startAddingVideosFromPlaylistSafe);
                lvActiveDownloads.Invoke(d, new object[] { videos, Location, Format });
            }
            else
            {
                foreach (KeyValuePair<string, string> video in videos) //the key is the id, and the value is the name of the video
                {
                    if (!videoExists(video.Key))
                    {
                        if (Format == "mp3")
                            addNewVideoToList(new Video(video.Key, video.Value, Location, Format, null, null));
                        else
                            addNewVideoToList(new Video(video.Key, video.Value, Location, Format, null));
                    }
                    else
                    {
                        MessageBox.Show(AllUserConfig.languageRM.GetString("msgVideoAlreadyInActivityTab1") + video.Value + AllUserConfig.languageRM.GetString("msgVideoAlreadyInActivityTab2")); ;
                    }
                }
            }
        }
        public ActivityTab()
        {
            InitializeComponent();
        }

        private void ActivityTab_Load(object sender, EventArgs e)
        {
            playlistThreads = new Dictionary<string, Thread>();
            lvActiveDownloads.Columns.Add(AllUserConfig.languageRM.GetString("progressColumn"), 120);
            lvActiveDownloads.Columns.Add(AllUserConfig.languageRM.GetString("videoColumn"), 255);
            lvActiveDownloads.Columns.Add(AllUserConfig.languageRM.GetString("sizeColumn"), 93);
            lvActiveDownloads.Columns.Add(AllUserConfig.languageRM.GetString("formatColumn"), 75);
            lvActiveDownloads.Columns.Add(AllUserConfig.languageRM.GetString("speedColumn"), 92);
            lvActiveDownloads.Columns.Add(AllUserConfig.languageRM.GetString("statusColumn"), 148);
            lvActiveDownloads.Columns.Add(AllUserConfig.languageRM.GetString("etaColumn"), 64);
            lvActiveDownloads.Columns[0].DisplayIndex = 2;
            lvActiveDownloads.Font = new Font(lvActiveDownloads.Font, FontStyle.Bold);

            progressBars = new ImageList
            {
                ImageSize = new System.Drawing.Size(111, 22)
            };
            progressBarBounds.Size = progressBars.ImageSize;
            lvActiveDownloads.SmallImageList = progressBars;

            controlIcons = new ImageList();

            controlIcons.Images.Add("resumePause", Properties.Resources.resumeStartDownload);
            controlIcons.Images.Add("pause", Properties.Resources.pauseDownload);
            controlIcons.Images.Add("stop", Properties.Resources.stopDownload);
            controlIcons.Images.Add("openFolder", Properties.Resources.openContainingFolder);
            controlIcons.Images.Add("deleteActivity", Properties.Resources.deleteFromActivity);
            controlIcons.Images.Add("deleteFile", Properties.Resources.deleteFromComputer);

            cmVideo.ImageList = controlIcons;

            cmVideo.Items.Add(AllUserConfig.languageRM.GetString("cmPause"));
            cmVideo.Items.Add(AllUserConfig.languageRM.GetString("cmStop"));
            cmVideo.Items.Add(AllUserConfig.languageRM.GetString("cmOpenFolder"));
            cmVideo.Items.Add(AllUserConfig.languageRM.GetString("cmDeleteActivity"));
            cmVideo.Items.Add(AllUserConfig.languageRM.GetString("cmDeleteFile"));

            ttStartPause.SetToolTip(btnStartPauseDownload, AllUserConfig.languageRM.GetString("cmPause"));
            ttStop.SetToolTip(btnCancelDownload, AllUserConfig.languageRM.GetString("cmStop"));
            ttOpenFolder.SetToolTip(btnOpenFileLocation, AllUserConfig.languageRM.GetString("cmOpenFolder"));
            ttDeleteActivity.SetToolTip(btnDeleteActivityDownload, AllUserConfig.languageRM.GetString("cmDeleteActivity"));
            ttDeleteFile.SetToolTip(btnDeleteFileDownload, AllUserConfig.languageRM.GetString("cmDeleteFile"));

            btnStartPauseDownload.Image = controlIcons.Images["resumePause"];
            btnCancelDownload.Image = controlIcons.Images["stop"];
            btnOpenFileLocation.Image = controlIcons.Images["openFolder"];
            btnDeleteActivityDownload.Image = controlIcons.Images["deleteActivity"];
            btnDeleteFileDownload.Image = controlIcons.Images["deleteFile"];
            
            disableAllButtons();

            cmVideo.Items[0].ImageKey = "resumePause";
            cmVideo.Items[1].ImageKey = "stop";
            cmVideo.Items[2].ImageKey = "openFolder";
            cmVideo.Items[3].ImageKey = "deleteActivity";
            cmVideo.Items[4].ImageKey = "deleteFile";
        }

        private void addNewVideoToList(Video video)
        {
            MyProgressBar progressBar = new MyProgressBar();
            progressBar.MinimumSize = progressBars.ImageSize;
            progressBar.MaximumSize = progressBars.ImageSize;
            progressBar.Size = progressBars.ImageSize;
            if (progressBars.Images.IndexOfKey(video.getID()) == -1)
            {
                Bitmap progressBarBitmap = new Bitmap(progressBars.ImageSize.Width, progressBars.ImageSize.Height);
                progressBar.DrawToBitmap(progressBarBitmap, progressBarBounds);
                VideoListItem item = new VideoListItem(video, ref progressBarBitmap, ref progressBar);
                item.Font = new Font(lvActiveDownloads.Font, FontStyle.Regular);
                lvActiveDownloads.Items.Add(item);
                progressBars.Images.Add(video.getID(), progressBarBitmap);
                item.ImageKey = video.getID();
                Thread update = new Thread(() => updateInfo(item));
                update.IsBackground = true;
                update.Start();
            }
            else
            {
                Bitmap progressBarBitmap = (Bitmap)progressBars.Images[video.getID()];
                progressBar.DrawToBitmap(progressBarBitmap, progressBarBounds);
                VideoListItem item = new VideoListItem(video, ref progressBarBitmap, ref progressBar);
                item.Font = new Font(lvActiveDownloads.Font, FontStyle.Regular);
                lvActiveDownloads.Items.Add(item);
                progressBars.Images.Add(video.getID(), progressBarBitmap);
                item.ImageKey = video.getID();
                Thread update = new Thread(() => updateInfo(item));
                update.IsBackground = true;
                update.Start();
            }
        }
        private void addNewPlaylistToList(string playlistID, string Location, string Format)
        {
            if (!videoExists(playlistID))
            {
                VideoListItem item = new VideoListItem(playlistID);
                item.Font = new Font(lvActiveDownloads.Font, FontStyle.Regular);
                item.Tag = new Video(playlistID, Location);
                lvActiveDownloads.Items.Add(item);
                Thread updatePlaylistThread = new Thread(() => updatePlaylist(item, playlistID, Location, Format));
                updatePlaylistThread.IsBackground = true;
                updatePlaylistThread.Start();
                playlistThreads.Add(playlistID, updatePlaylistThread);
            }
        }
        private void updateInfo(VideoListItem item)
        {
            int status = -1;
            while (true)
            {
                UpdateItemSafe(item);
                Thread.Sleep(1000);
                status = ((Video)item.Tag).getStatus();
                if (status == 0 || status == 6 || status >= 8)
                    break;
            }
            string ID = ((Video)item.Tag).getID();
            while (((Video)item.Tag).getName() == ID && ((Video)item.Tag).getStatus()!=6 && ((Video)item.Tag).getStatus()<8)
            {
                UpdateItemSafe(item);
                Thread.Sleep(1000);
            }
            UpdateItemSafe(item);
        }
        private void updateInfoOnce(VideoListItem item)
        {
            UpdateItemSafe(item);
        }
        private void updatePlaylist(VideoListItem item, string playlistId, string Location, string Format)
        {
            Dictionary<string, string> videos = IdAndNameExtractorFromPlaylist.GetIdsAndNames(playlistId);
            RemovePlaylistSafe(item);
            startAddingVideosFromPlaylistSafe(videos, Location, Format);
            if (playlistThreads.ContainsKey(((Video)item.Tag).getID()))
            {
                playlistThreads.Remove(((Video)item.Tag).getID());
            }
        }

        private void lvActiveDownloads_MouseClick(object sender, MouseEventArgs e)
        {
            if (lvActiveDownloads.FocusedItem!=null && lvActiveDownloads.FocusedItem.Bounds.Contains(e.Location))
            {
                btnDeleteActivityDownload.Enabled = true;
                btnDeleteFileDownload.Enabled = true;
                bool pauseEnabled = true;
                bool resumeEnabled = true;
                string constantLocation = ((Video)lvActiveDownloads.SelectedItems[0].Tag).getLocation();
                int constantLocationCounter = 0;
                int stopCounter = 0;
                int restartDownloadCounter = 0;
                foreach (VideoListItem video in lvActiveDownloads.SelectedItems)
                {
                    int videoStatus = ((Video)video.Tag).getStatus();
                    if (videoStatus > 4 || videoStatus == 0)
                    {
                        pauseEnabled = false;
                    }
                    if (videoStatus != 9)
                    {
                        resumeEnabled = false;
                    }
                    if (((Video)video.Tag).getLocation() != constantLocation)
                    {
                        cmVideo.Items[2].Enabled = false;
                        btnOpenFileLocation.Enabled = false;
                        constantLocationCounter++;
                    }
                    if (videoStatus == 0 || videoStatus == 6 || videoStatus == 8 || videoStatus == 10)
                    {
                        stopCounter++;
                    }
                    if (videoStatus == 8)
                    {
                        restartDownloadCounter++;
                    }
                }
                if (constantLocationCounter == 0)
                {
                    cmVideo.Items[2].Enabled = true;
                    btnOpenFileLocation.Enabled = true;
                }
                if (pauseEnabled)
                {
                    cmVideo.Items[0].Enabled = true;
                    cmVideo.Items[0].Text = AllUserConfig.languageRM.GetString("cmPause");
                    cmVideo.Items[0].ImageKey = "pause";
                    btnStartPauseDownload.Image = controlIcons.Images["pause"];
                    btnStartPauseDownload.Enabled = true;
                    ttStartPause.SetToolTip(btnStartPauseDownload, AllUserConfig.languageRM.GetString("cmPause"));
                }
                else if (resumeEnabled)
                {
                    cmVideo.Items[0].Enabled = true;
                    btnStartPauseDownload.Enabled = true;
                    cmVideo.Items[0].Text = AllUserConfig.languageRM.GetString("cmResume");
                    cmVideo.Items[0].ImageKey = "resumePause";
                    btnStartPauseDownload.Image = controlIcons.Images["resumePause"];
                    ttStartPause.SetToolTip(btnStartPauseDownload, AllUserConfig.languageRM.GetString("cmResume"));
                }
                else if (restartDownloadCounter == lvActiveDownloads.SelectedItems.Count)
                {
                    cmVideo.Items[0].Enabled = true;
                    cmVideo.Items[0].Text = AllUserConfig.languageRM.GetString("cmStart");
                    btnStartPauseDownload.Enabled = true;
                    cmVideo.Items[0].ImageKey = "resumePause";
                    btnStartPauseDownload.Image = controlIcons.Images["resumePause"];
                    ttStartPause.SetToolTip(btnStartPauseDownload, AllUserConfig.languageRM.GetString("cmStart"));
                    btnStartPauseDownload.Enabled = true;
                }
                else
                {
                    cmVideo.Items[0].Enabled = false;
                    btnStartPauseDownload.Enabled = false;
                    cmVideo.Items[0].ImageKey = "pause";
                    btnStartPauseDownload.Image = controlIcons.Images["pause"];
                    cmVideo.Items[0].Text = AllUserConfig.languageRM.GetString("cmPause");
                    ttStartPause.SetToolTip(btnStartPauseDownload, AllUserConfig.languageRM.GetString("cmPause"));
                }
                if (stopCounter > 0)
                {
                    cmVideo.Items[1].Enabled = false;
                    btnCancelDownload.Enabled = false;
                }
                else
                {
                    cmVideo.Items[1].Enabled = true;
                    btnCancelDownload.Enabled = true;
                }
                if (e.Button == MouseButtons.Right)
                {
                    cmVideo.Show(Cursor.Position);
                }
            }
        }

        private void cmVideo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            takeActionOnVideo(e.ClickedItem.ImageKey);
        }

        private void takeActionOnVideo(string actionId)
        {
            switch(actionId)
            {
                case "pause":
                case "resumePause":
                    {
                        int videoStatus = ((Video)lvActiveDownloads.SelectedItems[0].Tag).getStatus();
                        if (videoStatus > 7)
                        {
                            string ID = null;
                            string Location = null;
                            string Format = null;
                            string Type = null;
                            Trim trim = null;
                            foreach (VideoListItem video in lvActiveDownloads.SelectedItems)
                            {
                                ID = ((Video)video.Tag).getID();
                                Location = ((Video)video.Tag).getLocation();
                                Format = ((Video)video.Tag).getFormat();
                                Type = ((Video)video.Tag).getType();
                                trim = ((Video)video.Tag).getTrim();
                                video.Remove();
                                if (Type == "Video")
                                    addNewVideoToList(new Video(ID, Location, Format));
                                else
                                    addNewVideoToList(new Video(ID, Location, Format, trim));
                            }
                        }
                        else
                        {
                            foreach (VideoListItem video in lvActiveDownloads.SelectedItems)
                            {
                                ((Video)video.Tag).queueForStop();
                            }
                            foreach (VideoListItem video in lvActiveDownloads.SelectedItems)
                            {
                                ((Video)video.Tag).pauseDownload();
                            }
                        }
                        break;
                    }
                case "stop":
                    {
                        foreach (VideoListItem video in lvActiveDownloads.SelectedItems)
                        {
                            ((Video)video.Tag).queueForStop();
                        }
                        foreach (VideoListItem video in lvActiveDownloads.SelectedItems)
                        {
                            ((Video)video.Tag).cancelDownload();
                            updateInfoOnce(video);
                        }
                        break;
                    }
                case "openFolder":
                    {
                        Process.Start("explorer.exe", ((Video)lvActiveDownloads.SelectedItems[0].Tag).getLocation());
                        break;
                    }
                case "deleteActivity":
                    {
                        foreach (VideoListItem video in lvActiveDownloads.SelectedItems)
                        {
                            ((Video)video.Tag).queueForStop();
                        }
                        foreach (VideoListItem video in lvActiveDownloads.SelectedItems)
                        {
                            ((Video)video.Tag).pauseDownload();
                            video.Remove();
                            if (playlistThreads.ContainsKey(((Video)video.Tag).getID()))
                            {
                                playlistThreads[((Video)video.Tag).getID()].Abort();
                                playlistThreads.Remove(((Video)video.Tag).getID());
                            }
                        }
                        break;
                    }
                case "deleteFile":
                    {
                        foreach (VideoListItem video in lvActiveDownloads.SelectedItems)
                        {
                            ((Video)video.Tag).queueForStop();
                        }
                        foreach (VideoListItem video in lvActiveDownloads.SelectedItems)
                        {
                            ((Video)video.Tag).cancelDownload();
                            video.Remove();
                            if (playlistThreads.ContainsKey(((Video)video.Tag).getID()))
                            {
                                playlistThreads[((Video)video.Tag).getID()].Abort();
                                playlistThreads.Remove(((Video)video.Tag).getID());
                            }
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        private void lvActiveDownloads_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e) //can't resize progress bar, so disable column resizing
        {
            if (e.ColumnIndex == 0)
            {
                e.NewWidth = lvActiveDownloads.Columns[e.ColumnIndex].Width;
                e.Cancel = true;
            }
        }

        private void lvActiveDownloads_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                takeActionOnVideo("deleteActivity");
                disableAllButtons();
            }
            if (e.KeyCode == Keys.A && e.Control)
            {
                foreach (VideoListItem item in lvActiveDownloads.Items)
                {
                    item.Selected = true;
                }
                updateButtonsAvailability();
            }
        }

        private void disableAllButtons()
        {
            btnStartPauseDownload.Enabled = false;
            btnCancelDownload.Enabled = false;
            btnOpenFileLocation.Enabled = false;
            btnDeleteActivityDownload.Enabled = false;
            btnDeleteFileDownload.Enabled = false;
        }


        private void lvActiveDownloads_MouseUp(object sender, MouseEventArgs e)
        {
            if (lvActiveDownloads.SelectedItems.Count == 0)
                disableAllButtons();
        }
        private void updateButtonsAvailability()
        {
            if (lvActiveDownloads.SelectedItems.Count == 0)
                disableAllButtons();
            else
            {
                btnDeleteActivityDownload.Enabled = true;
                btnDeleteFileDownload.Enabled = true;
                bool pauseEnabled = true;
                bool resumeEnabled = true;
                string constantLocation = ((Video)lvActiveDownloads.SelectedItems[0].Tag).getLocation();
                int constantLocationCounter = 0;
                int stopCounter = 0;
                int restartDownloadCounter = 0;
                foreach (VideoListItem video in lvActiveDownloads.SelectedItems)
                {
                    int videoStatus = ((Video)video.Tag).getStatus();
                    if (videoStatus > 4 || videoStatus == 0)
                    {
                        pauseEnabled = false;
                    }
                    if (videoStatus != 9)
                    {
                        resumeEnabled = false;
                    }
                    if (((Video)video.Tag).getLocation() != constantLocation)
                    {
                        btnOpenFileLocation.Enabled = false;
                        constantLocationCounter++;
                    }
                    if (videoStatus == 0 || videoStatus == 6 || videoStatus == 8 || videoStatus == 10)
                    {
                        stopCounter++;
                    }
                    if (videoStatus == 8)
                    {
                        restartDownloadCounter++;
                    }
                }
                if (constantLocationCounter == 0)
                {
                    btnOpenFileLocation.Enabled = true;
                }
                if (pauseEnabled)
                {
                    btnStartPauseDownload.Image = controlIcons.Images["pause"];
                    btnStartPauseDownload.Enabled = true;
                    ttStartPause.SetToolTip(btnStartPauseDownload, AllUserConfig.languageRM.GetString("cmPause"));
                }
                else if (resumeEnabled)
                {
                    btnStartPauseDownload.Enabled = true;
                    btnStartPauseDownload.Image = controlIcons.Images["resumePause"];
                    ttStartPause.SetToolTip(btnStartPauseDownload, AllUserConfig.languageRM.GetString("cmResume"));
                }
                else if (restartDownloadCounter == lvActiveDownloads.SelectedItems.Count)
                {
                    btnStartPauseDownload.Enabled = true;
                    btnStartPauseDownload.Image = controlIcons.Images["resumePause"];
                    ttStartPause.SetToolTip(btnStartPauseDownload, AllUserConfig.languageRM.GetString("cmStart"));
                    btnStartPauseDownload.Enabled = true;
                }
                else
                {
                    btnStartPauseDownload.Enabled = false;
                    btnStartPauseDownload.Image = controlIcons.Images["pause"];
                    ttStartPause.SetToolTip(btnStartPauseDownload, AllUserConfig.languageRM.GetString("cmPause"));
                }
                if (stopCounter > 0)
                {
                    btnCancelDownload.Enabled = false;
                }
                else
                {
                    btnCancelDownload.Enabled = true;
                }
            }
        }
        private void cmVideo_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            updateButtonsAvailability();
        }
        private void btnStartPauseDownload_Click(object sender, EventArgs e)
        {
            takeActionOnVideo("resumePause");
            updateButtonsAvailability();
        }

        private void btnCancelDownload_Click(object sender, EventArgs e)
        {
            takeActionOnVideo("stop");
            updateButtonsAvailability();
        }

        private void btnDeleteActivityDownload_Click(object sender, EventArgs e)
        {

            takeActionOnVideo("deleteActivity");
            updateButtonsAvailability();
        }

        private void btnDeleteFileDownload_Click(object sender, EventArgs e)
        {

            takeActionOnVideo("deleteFile");
            updateButtonsAvailability();
        }

        private void btnOpenFileLocation_Click(object sender, EventArgs e)
        {
            takeActionOnVideo("openFolder");
            updateButtonsAvailability();
        }

        private void lvActiveDownloads_MouseCaptureChanged(object sender, EventArgs e)
        {
            updateButtonsAvailability();
        }

        private void lvActiveDownloads_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvActiveDownloads.FocusedItem.Bounds.Contains(e.Location))
                takeActionOnVideo("openFolder");
        }

        public void startAddingVideos(Dictionary<string, string> videos, string Location, string Format, Trim trim)
        {
            bool downloadAsPlaylist = true;
            bool askForChoice = true;
            foreach (KeyValuePair<string, string> video in videos) //the key is the id, and the value is the type (playlist, video or playlistId)
            {
                if (!videoExists(video.Key))
                {
                    if (video.Value == "Video")
                    {
                        if (Format == "mp3")
                            addNewVideoToList(new Video(video.Key, Location, Format, trim));
                        else
                            addNewVideoToList(new Video(video.Key, Location, Format));
                    }
                    else if (video.Value == "Playlist")
                    {
                        addNewPlaylistToList(video.Key, Location, Format);
                    }
                    else
                    {
                        if (askForChoice)
                        {
                            PlaylistAndVideoQuestionForm form = new PlaylistAndVideoQuestionForm();
                            if (form.ShowDialog() == DialogResult.OK)
                                addNewPlaylistToList(video.Value, Location, Format);
                            else
                            {
                                downloadAsPlaylist = false;
                                if (Format == "mp3")
                                    addNewVideoToList(new Video(video.Key, Location, Format, trim));
                                else
                                    addNewVideoToList(new Video(video.Key, Location, Format));
                            }
                            askForChoice = false;
                        }
                        else
                        {
                            if (downloadAsPlaylist)
                                addNewPlaylistToList(video.Value, Location, Format);
                            else
                            {
                                if (Format == "mp3")
                                    addNewVideoToList(new Video(video.Key, Location, Format, trim));
                                else
                                    addNewVideoToList(new Video(video.Key, Location, Format));
                            }
                        }
                    }
                }
                else
                {
                    string alreadyAddedVideoTitle = video.Key;
                    foreach(VideoListItem videoItem in lvActiveDownloads.Items)
                    {
                        if(((Video)videoItem.Tag).getID()==video.Key)
                        {
                            alreadyAddedVideoTitle = ((Video)videoItem.Tag).getName();
                            break;
                        }
                    }
                    MessageBox.Show(AllUserConfig.languageRM.GetString("msgVideoAlreadyInActivityTab1") + alreadyAddedVideoTitle + AllUserConfig.languageRM.GetString("msgVideoAlreadyInActivityTab2"));
                }
            }
        }

        private bool videoExists(string videoID)
        {
            foreach (VideoListItem video in lvActiveDownloads.Items)
            {
                if (((Video)video.Tag).getID() == videoID)
                    return true;
            }
            return false;
        }

        private void ActivityTab_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (VideoListItem video in lvActiveDownloads.Items)
            {
                ((Video)video.Tag).queueForStop();
            }
            foreach (VideoListItem video in lvActiveDownloads.Items)
            {
                if(((Video)video.Tag).getStatus()!=0 && ((Video)video.Tag).getStatus()!=10)
                    ((Video)video.Tag).cancelDownload();
                if (playlistThreads.ContainsKey(((Video)video.Tag).getID()))
                {
                    playlistThreads[((Video)video.Tag).getID()].Abort();
                }
            }
        }
        public bool noOngoingDownloads()
        {
            bool noOngoingDownloads = true;
            foreach(VideoListItem video in lvActiveDownloads.Items)
            {
                int status = ((Video)video.Tag).getStatus();
                if((status != 0 && status != 6 && status < 8) || ((Video)video.Tag).getType() == "playlist")
                {
                    noOngoingDownloads = false;
                    break;
                }
            }
            return noOngoingDownloads;
        }
    }
}
