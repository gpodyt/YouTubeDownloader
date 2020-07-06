using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace YTDownloader
{
    public partial class AutoUpdate : Form
    {
        private delegate void closeFormDelegate();
        private void closeFormSafe()
        {
            if (InvokeRequired)
            {
                var d = new closeFormDelegate(closeFormSafe);
                Invoke(d);
            }
            else
            {
                Close();
            }
        }
        public AutoUpdate()
        {
            InitializeComponent();
        }

        private void AutoUpdate_Load(object sender, EventArgs e)
        {
            Text = AllUserConfig.languageRM.GetString("AutoUpdateFormText");
            lblAutoUpdating.Text = AllUserConfig.languageRM.GetString("lblAutoUpdating");
            lblDoNotClose.Text = AllUserConfig.languageRM.GetString("lblDoNotClose");
            lblItWillCloseAuto.Text = AllUserConfig.languageRM.GetString("lblItWillCloseAuto");
            Thread updateThread = new Thread(() => update());
            updateThread.Start();
        }
        private void update()
        {
            Process update = new Process();
            update.StartInfo.FileName = "youtube-dl";
            update.StartInfo.Verb = "runas";
            update.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            update.StartInfo.Arguments = "-U";
            while (AllUserConfig.updateStatus == 0)
                continue;
            if (AllUserConfig.updateStatus == 1)
            {
                closeFormSafe();
                return;
            }
            else
            {
                try
                {
                    update.Start();
                    update.WaitForExit();
                }
                catch(Exception ex)
                {
                    closeFormSafe();
                    return;
                }
            }
            closeFormSafe();
        }
    }
}
