using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace YTDownloader
{
    class DownloadThread
    {
        Thread PrepareAndDownload;
        Thread NameSetter;
        Thread FormatAndSizeSetter;
        Thread downloadThread;
        Thread DownloadAudio;


        private Process download;
        private string Name;
        private string Size;
        private string Format;
        private string Location;
        private int Progress;
        private bool didIdeleteFilesOnce;
        private string DownloadSpeed;
        private int Status; //0 = Done, 1 = Loading metadata, 2 = Downloading video, 3 = Downloading audio, 4 = Format error, downloading best quality, 5 = Muxing, 6 = Error, 7 = Converting, 8 = Cancelled, 9 = Paused, 10 = Already Downloaded
        private string ETA;
        private bool UpdateSizePeriodically;
        private bool queuedForStop;
        private int FormatErrorCounter; //Brojach za kolku pati procentot se namalil namesto da se zgolemi, da se znae dali e nastanat problem so DASH formatite, ili se simnuva audioto
        private Trim trim;
        public DownloadThread(string ID, string Location, string Format) //video
        {
            queuedForStop = false;
            PrepareAndDownload = null;
            NameSetter = null;
            FormatAndSizeSetter = null;
            downloadThread = null;
            DownloadAudio = null;
            didIdeleteFilesOnce = false;
            download = null;
            trim = null;
            UpdateSizePeriodically = false; //if size remains Unknown, this is true
            FormatErrorCounter = 0; //if it becomes 2, set status to 4
            Name = ID;
            Size = "Unknown";
            this.Format = "Unknown";
            Progress = 0;
            DownloadSpeed = "Unknown";
            ETA = "Unknown";
            this.Location = Location.Replace('\\', '/');
            Status = 1;
            //Vo format dozvoleno e: 144p, 240p, 360p, 480p, 720p, 1080p, 1440p, 2160p i best (ili 720p60, 1080p60, 1440p60, 2160p60 i p60 (za best vo 60p))
            PrepareAndDownload = new Thread(() => PrepareDownload(ID, Format)); //Main thread so that object is not blocked
            PrepareAndDownload.IsBackground = true;
            PrepareAndDownload.Start();
        }
        public DownloadThread(string ID, string Name, string Location, string Format) //video from playlist
        {
            queuedForStop = false;
            PrepareAndDownload = null;
            NameSetter = null;
            FormatAndSizeSetter = null;
            downloadThread = null;
            DownloadAudio = null;
            didIdeleteFilesOnce = false;
            download = null;
            trim = null;
            UpdateSizePeriodically = false; //if size remains Unknown, this is true
            FormatErrorCounter = 0; //if it becomes 2, set status to 4
            this.Name = Name;
            Size = "Unknown";
            this.Format = "Unknown";
            Progress = 0;
            DownloadSpeed = "Unknown";
            ETA = "Unknown";
            this.Location = Location.Replace('\\', '/');
            Status = 1;
            //Vo format dozvoleno e: 144p, 240p, 360p, 480p, 720p, 1080p, 1440p, 2160p i best (ili 720p60, 1080p60, 1440p60, 2160p60 i p60 (za best vo 60p))
            PrepareAndDownload = new Thread(() => PrepareDownload(ID, Format)); //Main thread so that object is not blocked
            PrepareAndDownload.IsBackground = true;
            PrepareAndDownload.Start();
        }
        public void CancelDownload() //make sure to call this for each video at FormClosing event
        {
            bool deleteFiles = true;
            if (Status == 1)
                deleteFiles = false;
            if (FormatAndSizeSetter != null && FormatAndSizeSetter.IsAlive)
            {
                FormatAndSizeSetter.Abort();
                MaxNumOfSimVideos.maxN.Release();
            }
            else if (download != null && !download.HasExited)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "taskkill",
                    Arguments = "/F /T /PID " + download.Id,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }).WaitForExit();
            }
            Status = 8;
            Progress = 0;
            DownloadSpeed = "";
            ETA = "";
            int deletedFiles = 0;
            if (deleteFiles)
            {
                foreach (string file in Directory.GetFiles(Location))
                {
                    if (file.Contains(Name) && File.Exists(file))
                    {
                        try
                        {
                            File.Delete(file);
                            deletedFiles++;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(AllUserConfig.languageRM.GetString("msgCouldntDeleteFile1") + Name + AllUserConfig.languageRM.GetString("msgCouldntDeleteFile2"));
                        }
                    }
                }
                if (deletedFiles == 0 && didIdeleteFilesOnce == false) //for cyrillic videos unfortunately
                    MessageBox.Show(AllUserConfig.languageRM.GetString("msgFilesNotDeleted1") + Name + AllUserConfig.languageRM.GetString("msgFilesNotDeleted2"));
                else
                    didIdeleteFilesOnce = true;
            }
        }
        public void PauseDownload()
        {
            if (FormatAndSizeSetter != null && FormatAndSizeSetter.IsAlive)
            {
                FormatAndSizeSetter.Abort();
                MaxNumOfSimVideos.maxN.Release();
            }
            else if (download!=null && !download.HasExited)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "taskkill",
                    Arguments = "/F /T /PID " + download.Id,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }).WaitForExit();
            }
            Status = 9;
            DownloadSpeed = "";
            ETA = "";
        }
        void PrepareDownload(string ID, string Format)
        {
            MaxNumOfSimVideos.maxN.WaitOne();
            if(queuedForStop)
            {
                MaxNumOfSimVideos.maxN.Release();
                return;
            }
            NameSetter = new Thread(() => setName(ID)); //Get the name
            FormatAndSizeSetter = null;
            if (Format != "best")
            {
                FormatAndSizeSetter = new Thread(() => setFormatAndSize(ID, Format)); //Get format and size if quality is specified
            }
            else
            {
                FormatAndSizeSetter = new Thread(() => setFormatAndSize(ID)); //Set format to bestvideo+bestaudio and size to Unknown, obtain size while downloading
            }
            NameSetter.IsBackground = true;
            FormatAndSizeSetter.IsBackground = true;
            NameSetter.Start();
            FormatAndSizeSetter.Start();
            FormatAndSizeSetter.Join();
            if (queuedForStop)
            {
                return;
            }
            if (Size == "Unknown")
                UpdateSizePeriodically = true; //Obtain size while downloading is true
            downloadThread = new Thread(() => StartDownload(ID, Location));
            downloadThread.IsBackground = true;
            downloadThread.Start();
        }
        void StartDownload(string ID, string Location)
        {
            //Creating the process for download
            download = new Process();
            download.StartInfo.FileName = "youtube-dl";
            download.StartInfo.UseShellExecute = false;
            download.StartInfo.RedirectStandardOutput = true;
            download.StartInfo.RedirectStandardError = true;
            download.StartInfo.CreateNoWindow = true;
            download.StartInfo.Arguments = "-f " + Format + " -o \"" + Location + "/%(title)s.%(ext)s\" " + ID;
            //Console.WriteLine(download.StartInfo.Arguments);
            download.Start();
            while (!download.StandardOutput.EndOfStream)
            {
                if(Status!=10)
                    UpdateDataFromDownload(download.StandardOutput.ReadLine());
                //Console.WriteLine("Status: " + Status + ", Name: " + Name + ", Size: " + Size + ", Progress: " + Progress + ", DownloadSpeed: " + DownloadSpeed + ", ETA: " + ETA);
            }
            while (Status != 8 && Status != 9 && !download.StandardError.EndOfStream) //If the download is not cancelled, not paused and there is an error, set status to error
            {
                if (download.StandardError.ReadLine().ToLower().Contains("error"))
                    Status = 6;
            }
            if (Status != 6 && Status != 8 && Status != 9 && Status != 10) //If the download is not cancelled, not paused and there was no error, set the status to done
                Status = 0;
            DownloadSpeed = "";
            ETA = "";
            MaxNumOfSimVideos.maxN.Release();
            //Console.WriteLine("Status: " + Status + ", Name: " + Name + ", Size: " + Size + ", Progress: " + Progress + ", DownloadSpeed: " + DownloadSpeed + ", ETA: " + ETA);
        }
        void UpdateDataFromDownload(String Line)
        {
            string[] data = null;
            if (Line.Contains("has already been downloaded"))
            {
                Status = 10;
                Progress = 100;
            }
            else if (Line.Contains("[download]")) //Downloading
            {
                if(Status<2)
                    Status = 2; //Change status to downloading
                data = Line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); //Get all elements from downloading line
                foreach(string element in data)
                {
                    if (UpdateSizePeriodically) //Obtain the size if not obtained previously
                    {
                        if(FormatErrorCounter>1) //If percentage changed twice, size cannot be obtained, so set status that an error was found but it is downloading the best quality
                            Status = 4;
                        if (element.Contains("iB") && !element.Contains("iB/s")) //estetsko
                        {
                            if(Status == 2)
                                Size = element.Replace("iB", "B (video)");
                            else if(Status == 3)
                                Size = element.Replace("iB", "B (audio)");
                            else
                                Size = element.Replace("i", "");
                        }
                    }
                    if (element.Contains("%"))
                    {
                        int NewProgress = (int)(Math.Round(Double.Parse(element.Replace("%", ""))));
                        if (NewProgress < Progress) //If progress goes backwards, either audio is being downloaded or size cannot be obtained (DASH problem)
                        {
                            FormatErrorCounter++; //If this becomes more than 1, there is a DASH problem
                            if (Status<3) //If status was downloading video, change it to downloading audio
                                Status = 3;
                        }
                        Progress = NewProgress;
                    }
                    if(element.Contains("iB/s")) //Convert MiB to MB za estetski pricini
                    {
                        DownloadSpeed = element.Replace("i", "");
                    }
                    if(Regex.IsMatch(element,@"\d:\d")) //Isto i so ETA
                    {
                        ETA = element;
                    }
                }
            }
            else if(Line.Contains("[ffmpeg]"))
            {
                Status = 5;
                ETA = "";
                DownloadSpeed = "";
            }
        }
        public DownloadThread(string ID, string Location, string Format, Trim Trim, string NullForAudioDifferantiation) //audio
        {
            queuedForStop = false;
            PrepareAndDownload = null;
            NameSetter = null;
            FormatAndSizeSetter = null;
            downloadThread = null;
            DownloadAudio = null;
            didIdeleteFilesOnce = false;
            download = null;
            trim = Trim;
            UpdateSizePeriodically = false; //doesn't matter
            FormatErrorCounter = 0; //doesn't matter
            Format = null; //doesn't matter
            Name = ID;
            Size = "Unknown";
            this.Format = "Unknown";
            Progress = 0;
            DownloadSpeed = "Unknown";
            ETA = "Unknown";
            this.Location = Location.Replace('\\', '/');
            Status = 1;
            DownloadAudio = new Thread(() => StartAudioDownload(ID, this.Location));
            DownloadAudio.IsBackground = true;
            DownloadAudio.Start();
        }
        public DownloadThread(string ID, string Name, string Location, string Format, Trim Trim, string NullForAudioDifferantiation) //audio from playlist
        {
            queuedForStop = false;
            PrepareAndDownload = null;
            NameSetter = null;
            FormatAndSizeSetter = null;
            downloadThread = null;
            DownloadAudio = null;
            didIdeleteFilesOnce = false;
            download = null;
            trim = Trim;
            UpdateSizePeriodically = false; //doesn't matter
            FormatErrorCounter = 0; //doesn't matter
            Format = null; //doesn't matter
            this.Name = Name;
            Size = "Unknown";
            this.Format = "Unknown";
            Progress = 0;
            DownloadSpeed = "Unknown";
            ETA = "Unknown";
            this.Location = Location.Replace('\\', '/');
            Status = 1;
            DownloadAudio = new Thread(() => StartAudioDownload(ID, this.Location));
            DownloadAudio.IsBackground = true;
            DownloadAudio.Start();
        }
        void StartAudioDownload(string ID, string Location)
        {
            //Creating the process for download
            MaxNumOfSimVideos.maxN.WaitOne();
            if (queuedForStop)
            {
                MaxNumOfSimVideos.maxN.Release();
                return;
            }
            NameSetter = new Thread(() => setName(ID));
            NameSetter.IsBackground = true;
            NameSetter.Start();
            download = new Process();
            download.StartInfo.FileName = "youtube-dl";
            download.StartInfo.UseShellExecute = false;
            download.StartInfo.RedirectStandardOutput = true;
            download.StartInfo.RedirectStandardError = true;
            download.StartInfo.CreateNoWindow = true;
            if(trim==null)
            {
                download.StartInfo.Arguments = "-f bestaudio --extract-audio --audio-format mp3 --audio-quality 320K -o \"" + Location + "/%(title)s.%(ext)s\" " + ID;
            }
            else
            {
                download.StartInfo.Arguments = "-f bestaudio --extract-audio --audio-format mp3 --audio-quality 320K --postprocessor-args \"-ss " + trim.GetStartTime() + " -to "
                    + trim.GetEndTime() + "\" -o \"" + Location + "/%(title)s.%(ext)s\" " + ID;
            }
            //Console.WriteLine(download.StartInfo.Arguments);
            download.Start();
            while (!download.StandardOutput.EndOfStream)
            {
                if(Status != 10)
                    UpdateDataFromAudioDownload(download.StandardOutput.ReadLine());
                //Console.WriteLine("Status: " + Status + ", Name: " + Name + ", Size: " + Size + ", Progress: " + Progress + ", DownloadSpeed: " + DownloadSpeed + ", ETA: " + ETA);
            }
            while (Status != 8 && Status != 9 && !download.StandardError.EndOfStream) //If the download is not cancelled, not paused and there is an error, set status to error
            {
                if (download.StandardError.ReadLine().ToLower().Contains("error"))
                    Status = 6;
            }
            if (Status != 6 && Status != 8 && Status != 9 && Status != 10) //If the download is not cancelled, not paused and there was no error, set the status to done
                Status = 0;
            DownloadSpeed = "";
            ETA = "";
            MaxNumOfSimVideos.maxN.Release();
            //Console.WriteLine("Status: " + Status + ", Name: " + Name + ", Size: " + Size + ", Progress: " + Progress + ", DownloadSpeed: " + DownloadSpeed + ", ETA: " + ETA);
        }
        void UpdateDataFromAudioDownload(String Line)
        {
            string[] data = null;
            if (Line.Contains("has already been downloaded"))
            {
                Status = 10;
                Progress = 100;
            }
            else if (Line.Contains("[download]")) //Downloading
            {
                if (Status < 2)
                    Status = 3; //Change status to downloading audio
                data = Line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); //Get all elements from downloading line
                foreach (string element in data)
                {
                    if (element.Contains("iB") && !element.Contains("iB/s")) //estetsko, update size
                    {
                        Size = element.Replace("i", "");
                    }
                    if (element.Contains("%"))
                    {
                        Progress = (int)(Math.Round(Double.Parse(element.Replace("%", ""))));
                    }
                    if (element.Contains("iB/s")) //Convert MiB to MB za estetski pricini
                    {
                        DownloadSpeed = element.Replace("i", "");
                    }
                    if (Regex.IsMatch(element, @"\d:\d")) //Isto i so ETA
                    {
                        ETA = element;
                    }
                }
            }
            else if (Line.Contains("[ffmpeg]"))
            {
                Status = 7;
                ETA = "";
                DownloadSpeed = "";
            }
        }
        void setName(string ID)
        {
            if (Name==ID)
                Name = getName(ID);
        }
        void setFormatAndSize(string ID, string Format)
        {
            string[] FormatAndSize = getFormatAndSizeForVideoDownload(ID, Format);
            this.Format = FormatAndSize[0];
            Size = FormatAndSize[1];
        }
        void setFormatAndSize(string ID)
        {
            Format = "bestvideo+bestaudio";
            Size = "Unknown";
        }
        static string getName(string ID)
        {
            Process nameProcess = new Process();
            nameProcess.StartInfo.FileName = "youtube-dl";
            nameProcess.StartInfo.Arguments = "-j " + ID;
            nameProcess.StartInfo.UseShellExecute = false;
            nameProcess.StartInfo.RedirectStandardOutput = true;
            nameProcess.StartInfo.CreateNoWindow = true;
            nameProcess.Start();
            string Name = ID;
            while (!nameProcess.StandardOutput.EndOfStream)
            {
                Name = nameProcess.StandardOutput.ReadLine();
            }
            dynamic jsonData;
            try
            {
                jsonData = JsonConvert.DeserializeObject(Name);
            }
            catch (Exception ex)
            {
                return ID;
            }
            return jsonData.fulltitle;
        }
        static string[] getFormatAndSizeForVideoDownload(string ID, string Format)
        {
            string FormatToReturn = null;
            Size Size = new Size(); //Temporary Size class for easier byte manipulation
            List<string[]> Formats = new List<string[]>();
            Process formatsProcess = new Process();
            formatsProcess.StartInfo.FileName = "youtube-dl";
            formatsProcess.StartInfo.Arguments = "-F " + ID;
            formatsProcess.StartInfo.UseShellExecute = false;
            formatsProcess.StartInfo.RedirectStandardOutput = true;
            formatsProcess.StartInfo.CreateNoWindow = true;
            formatsProcess.Start();
            bool startReading = false;
            string Line = null;
            while(!formatsProcess.StandardOutput.EndOfStream) //read all formats
            {
                Line = formatsProcess.StandardOutput.ReadLine();
                if(startReading)
                {
                    Formats.Add(Line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                }
                if (Line.StartsWith("format"))
                    startReading = true;
            }
            string [] bestWithoutSize = { "bestvideo+bestaudio", "Unknown" };
            int HighestBitrate = 0;
            int HighestAudioBitrate = 0;
            string SizeToAdd = "0";
            string AudioSizeToAdd = "0";
            bool SkipVideoFormat = false;
            foreach(string[] FormatLine in Formats)
            {
                for(int i=0; i<FormatLine.Length; i++)
                {
                    if (FormatLine[i].Contains("DASH"))
                    {
                        return bestWithoutSize;
                    }
                    if (FormatLine[i].Contains(Format)) //find the wanted video format
                    {
                        for(int j=i; j<FormatLine.Length; j++)
                        {
                            if (FormatLine[j].Contains("Hz")) //to avoid the last formats that are video and audio
                            {
                                SkipVideoFormat = true;
                                break;
                            }
                        }
                        if (!SkipVideoFormat)
                        {
                            int Bitrate = 0;
                            if (FormatLine[i + 1].Contains("k"))
                                Bitrate = Int32.Parse(Regex.Replace(FormatLine[i + 1], @"[a-zA-Z]*", ""));
                            else //for HDR videos
                                Bitrate = Int32.Parse(Regex.Replace(FormatLine[i + 2], @"[a-zA-Z]*", ""));
                            if (Bitrate > HighestBitrate)
                            {
                                HighestBitrate = Bitrate;
                                FormatToReturn = FormatLine[0];
                                SizeToAdd = FormatLine[FormatLine.Length - 1];
                            }
                        }
                        SkipVideoFormat = false;
                    }
                    if (FormatLine[i]=="tiny") //find the highest audio format for size
                    {
                        int AudioBitrate = Int32.Parse(Regex.Replace(FormatLine[i + 1], @"[a-zA-Z]*", ""));
                        if (AudioBitrate > HighestAudioBitrate)
                        {
                            HighestAudioBitrate = AudioBitrate;
                            AudioSizeToAdd = FormatLine[FormatLine.Length - 1];
                        }
                    }
                }
            }
            if(FormatToReturn==null && Format!="p60" && Format.Contains("p60"))
            {
                foreach (string[] FormatLine in Formats)
                {
                    for (int i = 0; i < FormatLine.Length; i++)
                    {
                        if (FormatLine[i].Contains(Format.Remove(Format.Length - 2, 2))) //find the format without 60fps if "force 60 was checked", so that if a user downloads a huge playlist and wants all the 60fps videos to be 60, but the ones that are not to keep the same resolution, we must check again for all formats
                        {
                            for (int j = i; j < FormatLine.Length; j++)
                            {
                                if (FormatLine[j].Contains("Hz")) //to avoid the last formats that are video and audio
                                {
                                    SkipVideoFormat = true;
                                    break;
                                }
                            }
                            if (!SkipVideoFormat)
                            {
                                int Bitrate = 0;
                                if (FormatLine[i + 1].Contains("k")) //for HDR videos
                                    Bitrate = Int32.Parse(Regex.Replace(FormatLine[i + 1], @"[a-zA-Z]*", ""));
                                else
                                    Bitrate = Int32.Parse(Regex.Replace(FormatLine[i + 2], @"[a-zA-Z]*", ""));
                                if (Bitrate > HighestBitrate)
                                {
                                    HighestBitrate = Bitrate;
                                    FormatToReturn = FormatLine[0];
                                    SizeToAdd = FormatLine[FormatLine.Length - 1];
                                }
                            }
                            SkipVideoFormat = false;
                        }
                        if (FormatLine[i] == "tiny") //find the highest audio format for size
                        {
                            int AudioBitrate = Int32.Parse(Regex.Replace(FormatLine[i + 1], @"[a-zA-Z]*", ""));
                            if (AudioBitrate > HighestAudioBitrate)
                            {
                                HighestAudioBitrate = AudioBitrate;
                                AudioSizeToAdd = FormatLine[FormatLine.Length - 1];
                            }
                        }
                    }
                }
            }
            Size.AddToSize(SizeToAdd);
            Size.AddToSize(AudioSizeToAdd);
            if (AudioSizeToAdd!="0" && FormatToReturn == null) //if audio size is found, but video format not, then a higher quality was provided than available, so find the highest quality video format
            {
                HighestBitrate = 0;
                foreach (string[] FormatLine in Formats)
                {
                    for (int i = 0; i < FormatLine.Length; i++)
                    {
                        if(FormatLine[i].Contains("2160p")) //this will only happen if best60 was selected, but the video is higher than 4K and has no 60fps
                        {
                            return bestWithoutSize;
                        }
                        if (FormatLine[i].Contains("144p") || FormatLine[i].Contains("240p") || FormatLine[i].Contains("360p") || FormatLine[i].Contains("480p")
                            || FormatLine[i].Contains("720p") || FormatLine[i].Contains("1080p") || FormatLine[i].Contains("1440p"))
                        {
                            for (int j = i; j < FormatLine.Length; j++)
                            {
                                if (FormatLine[j].Contains("Hz")) //to avoid the last formats that are video and audio
                                {
                                    SkipVideoFormat = true;
                                    break;
                                }
                            }
                            if (!SkipVideoFormat)
                            {
                                int Bitrate = 0;
                                if (FormatLine[i + 1].Contains("k")) //for HDR videos
                                    Bitrate = Int32.Parse(Regex.Replace(FormatLine[i + 1], @"[a-zA-Z]*", ""));
                                else
                                    Bitrate = Int32.Parse(Regex.Replace(FormatLine[i + 2], @"[a-zA-Z]*", ""));
                                if (Bitrate > HighestBitrate)
                                {
                                    HighestBitrate = Bitrate;
                                    FormatToReturn = FormatLine[0];
                                    SizeToAdd = FormatLine[FormatLine.Length - 1];
                                }
                            }
                            SkipVideoFormat = false;
                        }
                    }
                }
                Size.AddToSize(SizeToAdd);
            }
            if (FormatToReturn == null) //if video format is still null, there was an error with quality selection, so download highest quality
                FormatToReturn = "bestvideo";
            string[] returnData = { FormatToReturn + "+bestaudio", Size.GetSize() };
            return returnData;
        } //returns an array of two strings, one is format ex: 312+bestaudio, and the second one is size




        public string getName()
        {
            return Name;
        }
        public string getSize()
        {
            return Size;
        }
        public int getProgress()
        {
            return Progress;
        }
        public string getDownloadSpeed()
        {
            return DownloadSpeed;
        }
        public int getStatus()
        {
            return Status;
        }
        public string getETA()
        {
            return ETA;
        }

        public void queueForStop()
        {
            queuedForStop = true;
        }
    }
}
