using System;
using System.Text.RegularExpressions;

namespace YTDownloader
{
    //Temporary Size class for DownloadThread
    class Size
    {
        string SizeToReturn;
        double SizeInKB;
        public Size()
        {
            SizeToReturn = null;
            SizeInKB = 0;
        }
        public void AddToSize(string ToAdd)
        {
            double KBToAdd = Double.Parse(Regex.Replace(ToAdd, @"[a-zA-Z]*", ""));
            if (ToAdd.Contains("KiB"))
                SizeInKB += KBToAdd;
            if (ToAdd.Contains("MiB"))
                SizeInKB += KBToAdd * 1024;
            if (ToAdd.Contains("GiB"))
                SizeInKB += KBToAdd * 1024 * 1024;
        }
        public string GetSize()
        {
            if (SizeInKB < 1024)
                SizeToReturn = SizeInKB + "KB";
            else if (SizeInKB < 1048576)
                SizeToReturn = Math.Round(SizeInKB / 1024, 2) + "MB";
            else
                SizeToReturn = Math.Round(SizeInKB / 1024 / 1024, 2) + "GB";
            if (SizeInKB == 0)
                return "Unknown";
            else
                return SizeToReturn;
        }
    }
}
