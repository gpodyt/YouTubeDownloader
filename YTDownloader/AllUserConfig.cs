using System;
using System.Reflection;
using System.Resources;

namespace YTDownloader
{
    static class AllUserConfig
    {
        public static int updateStatus = 0; //0 - no info, 1 - no error occured, 2 - error occured
        public static int lastSelectedFormatIndex = 1;
        public static int maxSimDownloads = 3;
        public static string downloadLocation = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        public static string lang = "en";
        public static bool firstTimeAppRunning = false;
        public static ResourceManager languageRM = new ResourceManager("YTDownloader.language_en", Assembly.GetExecutingAssembly());
    }
}
