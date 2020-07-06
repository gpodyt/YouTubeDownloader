using System;
using System.IO;
using System.Windows.Forms;

namespace YTDownloader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string ytdData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MarkosYTD";
            if(Directory.Exists(ytdData))
            {
                loadOrCreateFiles(ytdData);
            }
            else
            {
                AllUserConfig.firstTimeAppRunning = true;
                Directory.CreateDirectory(ytdData);
                loadOrCreateFiles(ytdData);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
        static void loadOrCreateFiles(string location)
        {
            string language;
            string defaultLocation;
            string simultaenousDownloads;
            int downloadsInteger = 3;
            if(File.Exists(location+"\\lang.txt"))
            {
                language = File.ReadAllText(location + "\\lang.txt");
                if (language.Length != 0)
                    AllUserConfig.lang = language;
            }
            else
            {
                File.WriteAllText(location + "\\lang.txt", "");
            }
            if (File.Exists(location + "\\location.txt"))
            {
                defaultLocation = File.ReadAllText(location + "\\location.txt");
                if (defaultLocation.Length != 0)
                    AllUserConfig.downloadLocation = defaultLocation;
            }
            else
            {
                File.WriteAllText(location + "\\location.txt", "");
            }
            if (File.Exists(location + "\\sim.txt"))
            {
                simultaenousDownloads = File.ReadAllText(location + "\\sim.txt");
                if (Int32.TryParse(simultaenousDownloads, out downloadsInteger))
                    AllUserConfig.maxSimDownloads = downloadsInteger;
            }
            else
            {
                File.WriteAllText(location + "\\sim.txt", "");
            }
        }
    }
}
