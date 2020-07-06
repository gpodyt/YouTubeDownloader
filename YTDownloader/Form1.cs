using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

namespace YTDownloader
{
    public partial class Main : Form
    {
        ActivityTab activityTab;
        DownloadTab downloadTab;
        public Main()
        {
            InitializeComponent();
        }
        private void Main_Load(object sender, EventArgs e)
        {
            Thread autoUpdateThread = new Thread(() => autoUpdate());
            if(AllUserConfig.firstTimeAppRunning)
            {
                LanguageSelection languageSelectionForm = new LanguageSelection();
                languageSelectionForm.ShowDialog();
            }
            if (AllUserConfig.lang == "mk")
            {
                AllUserConfig.languageRM = new ResourceManager("YTDownloader.language_mk", Assembly.GetExecutingAssembly());
                menuPicture.Image = Properties.Resources.MenuMkd;
            }
            else
            {
                menuPicture.Image = Properties.Resources.MenuEng;
            }
            Text = AllUserConfig.languageRM.GetString("ProgramTitle");
            MaxNumOfSimVideos.changeNumber(AllUserConfig.maxSimDownloads);
            activityTab = new ActivityTab();
            downloadTab = new DownloadTab();
            activityTab.MdiParent = this;
            downloadTab.MdiParent = this;
            activityTab.Show();
            activityTab.Location = new Point(0, 161);
            downloadTab.Show();
            downloadTab.Location = new Point(0, 161);
            btnDownloadTab.TabStop = false;
            btnDownloadTab.FlatStyle = FlatStyle.Flat;
            btnDownloadTab.FlatAppearance.BorderSize = 0;
            btnDownloadTab.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            btnDownloadTab.Text = AllUserConfig.languageRM.GetString("downloadTab");
            btnActivityTab.TabStop = false;
            btnActivityTab.FlatStyle = FlatStyle.Flat;
            btnActivityTab.FlatAppearance.BorderSize = 0;
            btnActivityTab.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            btnActivityTab.BackColor = Color.DarkGray;
            btnActivityTab.Text = AllUserConfig.languageRM.GetString("activityTab");
            btnDownloadTab.FlatAppearance.MouseOverBackColor = DefaultBackColor;
            btnActivityTab.FlatAppearance.MouseOverBackColor = Color.FromArgb(152, 152, 152);

            autoUpdateThread.Start();
        }

        private void btnActivityTab_Click(object sender, EventArgs e)
        {
            if(ActiveMdiChild != activityTab)
            {
                btnActivityTab.BackColor = DefaultBackColor;
                btnActivityTab.FlatAppearance.MouseOverBackColor = DefaultBackColor;
                btnDownloadTab.BackColor = Color.DarkGray;
                btnDownloadTab.FlatAppearance.MouseOverBackColor = Color.FromArgb(152, 152, 152);
                activityTab.Activate();
            }
        }

        private void btnDownloadTab_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != downloadTab)
            {
                btnActivityTab.BackColor = Color.DarkGray;
                btnActivityTab.FlatAppearance.MouseOverBackColor = Color.FromArgb(152, 152, 152);
                btnDownloadTab.BackColor = DefaultBackColor;
                btnDownloadTab.FlatAppearance.MouseOverBackColor = DefaultBackColor;
                downloadTab.Activate();
            }
        }

        private void pbSettings_MouseClick(object sender, MouseEventArgs e)
        {
            Settings settingsForm = new Settings();
            settingsForm.ShowDialog();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            string ytdData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MarkosYTD";
            if (File.Exists(ytdData + "\\lang.txt"))
                File.WriteAllText(ytdData + "\\lang.txt", AllUserConfig.lang);
            if (File.Exists(ytdData + "\\location.txt"))
                File.WriteAllText(ytdData + "\\location.txt", AllUserConfig.downloadLocation);
            if (File.Exists(ytdData + "\\sim.txt"))
                File.WriteAllText(ytdData + "\\sim.txt", AllUserConfig.maxSimDownloads.ToString());
        }

        private void autoUpdate()
        {
            downloadTab.enableDownloadButtonSafe(false);
            Process update = new Process();
            Thread updateForm = new Thread(() => createUpdateForm());
            update.StartInfo.FileName = "youtube-dl";
            update.StartInfo.UseShellExecute = false;
            update.StartInfo.RedirectStandardOutput = true;
            update.StartInfo.CreateNoWindow = true;
            update.StartInfo.Arguments = "-U";
            update.Start();
            bool updateStarted = false;
            string line;
            while (!update.StandardOutput.EndOfStream)
            {
                line = update.StandardOutput.ReadLine();
                if (line.Contains("up-to-date"))
                    continue;
                else if (!updateStarted)
                {
                    updateForm.Start();
                    updateStarted = true;
                }
                if(line.Contains("ERROR"))
                    AllUserConfig.updateStatus = 2;
            }
            if (AllUserConfig.updateStatus == 0)
                AllUserConfig.updateStatus = 1;
            if(updateForm.IsAlive)
                updateForm.Join();
            downloadTab.enableDownloadButtonSafe(true);
        }

        private void createUpdateForm()
        {
            AutoUpdate updateForm = new AutoUpdate();
            updateForm.ShowDialog();
        }

        private void pbSettings_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void pbSettings_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }
    }
}
