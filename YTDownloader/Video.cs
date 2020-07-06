namespace YTDownloader
{
    class Video
    {
        private DownloadThread downloadThread;
        private string Type;
        private string ID;
        private string Format;
        private string Location;
        private Trim trim;
        public Video(string ID, string Location, string Format) //video download
        {
            downloadThread = new DownloadThread(ID, Location, Format); //start download
            this.ID = ID;
            this.Location = Location;
            this.Format = Format;
            this.trim = null;
            Type = "Video";
        }
        public Video(string ID, string Location, string Format, Trim trim) //audio download
        {
            downloadThread = new DownloadThread(ID, Location, Format, trim, null); //start download
            this.ID = ID;
            this.Location = Location;
            this.Format = Format;
            this.trim = trim;
            Type = "Audio";
        }

        public Video(string ID, string Name, string Location, string Format, string VideoFromPlaylist) //video from playlist download
        {
            downloadThread = new DownloadThread(ID, Name, Location, Format); //start download
            this.ID = ID;
            this.Location = Location;
            this.Format = Format;
            this.trim = null;
            Type = "Video";
        }
        public Video(string ID, string Name, string Location, string Format, Trim trim, string AudioFromPlaylist) //audio from playlist download
        {
            downloadThread = new DownloadThread(ID, Name, Location, Format, trim, null); //start download
            this.ID = ID;
            this.Location = Location;
            this.Format = Format;
            this.trim = trim;
            Type = "Audio";
        }
        //methods for restarting download
        public string getType()
        {
            return Type;
        }
        public string getID()
        {
            return ID;
        }
        public string getLocation()
        {
            return Location;
        }
        public string getFormat()
        {
            return Format;
        }
        public Trim getTrim()
        {
            return trim;
        }
        //end of methods for restarting download

        //pause and cancel methods
        public void pauseDownload()
        {
            if(downloadThread!=null)
                downloadThread.PauseDownload();
        }
        public void cancelDownload()
        {
            if(downloadThread!=null)
                downloadThread.CancelDownload();
        }

        //methods to be called periodically (once a second)
        public string getName()
        {
            return downloadThread.getName();
        }
        public string getSize()
        {
            return downloadThread.getSize();
        }
        public int getProgress()
        {
            return downloadThread.getProgress();
        }
        public string getDownloadSpeed()
        {
            return downloadThread.getDownloadSpeed();
        }
        public int getStatus()
        {
            if (Type != "playlist")
                return downloadThread.getStatus();
            else
                return 0;
        }
        public string getETA()
        {
            return downloadThread.getETA();
        }
        //all of these should be ordered in columns in list view just like in the order here
        public Video(string playlistID, string Location)//playlist null object
        {
            ID = playlistID;
            Type = "playlist";
            this.Location = Location;
            downloadThread = null;
        }
        public string getFormatForUpdate()
        {
            if (Format == "best")
                return AllUserConfig.languageRM.GetString("best");
            else if (Format == "p60")
                return AllUserConfig.languageRM.GetString("p60");
            else
                return Format;
        }
        public string getSizeForUpdate()
        {
            if (getSize() == "Unknown")
                return AllUserConfig.languageRM.GetString("Unknown");
            else
                return getSize();
        }
        public string getSpeedForUpdate()
        {
            if (getDownloadSpeed() == "Unknown")
                return AllUserConfig.languageRM.GetString("Unknown");
            else
                return getDownloadSpeed();
        }
        public string getETAForUpdate()
        {
            if (getETA() == "Unknown")
                return AllUserConfig.languageRM.GetString("Unknown");
            else
                return getETA();

        }
        public void queueForStop()
        {
            downloadThread.queueForStop();
        }
    }
}
