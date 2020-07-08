# ВАЖНО: За да функционира правилно програмата, потребно е [ова](https://yt-dl.org/downloads/2020.06.16.1/youtube-dl) и ffmpeg.exe од [овде](https://ffmpeg.zeranoe.com/builds/win64/static/ffmpeg-20200628-4cfcfb3-win64-static.zip) да се стават во истиот фолдер каде што ќе биде искомпајлирана програмата!
![](http://gp.mk/ytdEng.jpg)
#### [Македонски](#english)
YouTube Downloader is a GUI application which allows downloading YouTube videos in various formats, from **MP3 audio-only** to the highest video quality that YouTube provides **(8K 60fps)**. It does so by using the [youtube-dl](https://youtube-dl.org/) API, as well as [ffmpeg](https://ffmpeg.org/) for conversion to MP3.
## **1. Key Features**
  - Virtually **every format** that YouTube provides is supported and selectable
  - **Download multiple videos simultaneously**
  - **Playlist links** are also supported, which automatically enqueues all videos for download
  - **Pausing and resuming downloads**
  - **Trimming** for audio (MP3) downloads

## **2. Main Idea**
This application was heavily inspired by [YTD Video Downloader](https://www.ytddownloader.com/), which is a paid YouTube downloader, but often does not work, especially for music videos on YouTube because of the way they are hosted on YouTube. [Youtube-dl](https://youtube-dl.org/) however, overcomes this problem and it is updated very frequently to make sure it always works with YouTube's newest "ways of providing video to users". The only downside to it is that it is a command-line program, which is not so appealing to all end-users. And this is why I made my GUI version of it.

## **3. Problem solution**
The application is implemented using the standard data structures in C# and Windows Forms (such as Lists, Dictionaries, Arrays etc.), as well as [Json.NET](https://www.newtonsoft.com/json) for retrieving video and playlist metadata from [youtube-dl](https://youtube-dl.org/). At the heart of my application is the class **DownloadThread**. 
```csharp
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
        private int Status;
        private string ETA;
        private bool UpdateSizePeriodically;
        private bool queuedForStop;
        private int FormatErrorCounter;
        private Trim trim;
        public DownloadThread(string ID, string Location, string Format) //video
        public DownloadThread(string ID, string Name, string Location, string Format) //video from playlist
        public void CancelDownload()
        public void PauseDownload()
        void PrepareDownload(string ID, string Format)
        void StartDownload(string ID, string Location)
        void UpdateDataFromDownload(String Line)
        public DownloadThread(string ID, string Location, string Format, Trim Trim, string NullForAudioDifferantiation) //audio
        public DownloadThread(string ID, string Name, string Location, string Format, Trim Trim, string NullForAudioDifferantiation) //audio from playlist
        void StartAudioDownload(string ID, string Location)
        void UpdateDataFromAudioDownload(String Line)
        void setName(string ID)
        void setFormatAndSize(string ID, string Format)
        void setFormatAndSize(string ID)
        static string getName(string ID)
        static string[] getFormatAndSizeForVideoDownload(string ID, string Format) //returns an array of two strings, one is format ex: 312+bestaudio, and the second one is size

        public string getName()
        public string getSize()
        public int getProgress()
        public string getDownloadSpeed()
        public int getStatus()
        public string getETA()
        public void queueForStop()
    }
```
This is where most of the work happens in YouTube Downloader. Basically, the input for the DownloadThread consists of the following arguments:
- Video ID
- Location (where to download)
- Format
- Trim information (if the selected format is MP3)

The **Video ID** is exactly what it says it is: it's the ID of the YouTube video, from which [youtube-dl](https://youtube-dl.org/) can extract metadata, formats, as well as download the video. It is the bolded part (https[]()://www[]().youtube.com/watch?v=**dQw4w9WgXcQ**) of every YouTube video link. We will see later down in the document how this ID goes into [youtube-dl](https://youtube-dl.org/)'s arguments for fetching data and downloading the video.

The **Location** is a local directory link which the user chooses, which represents where the video/videos will be downloaded.

The **Format** is the video/audio format which again, the user selects before downloading the video, and the video is downloaded in that format. If the video has a lower quality than the user selected one, then the highest quality available is downloaded.

Finally, the **Trim** information are the timings that the user provides, for trimming the MP3 audio download.

### Now, let's get into more details!
We will briefly discuss how the downloading happens. We will focus on the video part and video constructor of the DownloadThread class, since the audio downloading is simillar and simpler. Let's look at the code first:
```csharp
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
```
Quite basic, fetches the arguments and sets the class data accordingly. At the very last steps, it starts the **PrepareAndDownload** thread. This is where all the magic happens! :) This thread exists only so that the constructor does not have to wait for all processes to finish in order to initialize the object. So, let's see the PrepareAndDownload thread then:
```csharp
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
```
[Youtube-dl](https://youtube-dl.org/) provides many commands for retreiving video info, such as title, thumbnail, number of likes and dislikes, format id's, size information etc. Unfortunately, not all of them can fetched in a single command, which means youtube-dl should be called (created as a process) multiple times. This is where the **NameSetter**, **FormatAndSizeSetter** and **downloadThread** threads come into play. They are self-explanatory, but in short, this is how they work:
#### NameSetter
This thread creates a youtube-dl process, with the full arguments being:
```
youtube-dl -j video-id
```
This command returns the full metadata of the video. There is a command "youtube-dl -e video-id" which returns only the title of the video, but due to the encoding limitations of the console in C#, cyrillic titles just couldn't be read. But when reading json data, youtube-dl encodes cyrillic characters in UTF-8 and when deserializing them with [Json.NET](https://www.newtonsoft.com/json), they come out correctly. Therefore, the code for this thread is the following:
```csharp
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
```
With this, we have the title of the video being downloaded which shows up in our GUI interface, which we will see at the [end of this document](#5-usage).
#### FormatAndSizeSetter
This thread calls the following command in youtube-dl:
```
youtube-dl -F video-id
```
Here's an example of how the output of this command for a certain video looks like:
```
youtube-dl -F dQw4w9WgXcQ
[youtube] dQw4w9WgXcQ: Downloading webpage
[info] Available formats for dQw4w9WgXcQ:
format code  extension  resolution note
249          webm       audio only tiny   49k , opus @ 50k (48000Hz), 1.18MiB
250          webm       audio only tiny   65k , opus @ 70k (48000Hz), 1.55MiB
140          m4a        audio only tiny  130k , m4a_dash container, mp4a.40.2@128k (44100Hz), 3.27MiB
251          webm       audio only tiny  136k , opus @160k (48000Hz), 3.28MiB
394          mp4        256x144    144p   73k , av01.0.00M.08, 25fps, video only, 1.72MiB
278          webm       256x144    144p   97k , webm container, vp9, 25fps, video only, 2.25MiB
160          mp4        256x144    144p  107k , avc1.4d400c, 25fps, video only, 2.05MiB
395          mp4        426x240    240p  159k , av01.0.00M.08, 25fps, video only, 3.42MiB
242          webm       426x240    240p  217k , vp9, 25fps, video only, 4.04MiB
133          mp4        426x240    240p  290k , avc1.4d4015, 25fps, video only, 4.48MiB
396          mp4        640x360    360p  340k , av01.0.01M.08, 25fps, video only, 6.68MiB
243          webm       640x360    360p  396k , vp9, 25fps, video only, 6.96MiB
134          mp4        640x360    360p  484k , avc1.4d401e, 25fps, video only, 8.27MiB
244          webm       854x480    480p  586k , vp9, 25fps, video only, 10.03MiB
397          mp4        854x480    480p  603k , av01.0.04M.08, 25fps, video only, 11.37MiB
135          mp4        854x480    480p  741k , avc1.4d401e, 25fps, video only, 11.56MiB
247          webm       1280x720   720p 1035k , vp9, 25fps, video only, 17.67MiB
136          mp4        1280x720   720p 1077k , avc1.4d401f, 25fps, video only, 16.72MiB
398          mp4        1280x720   720p 1133k , av01.0.05M.08, 25fps, video only, 22.07MiB
399          mp4        1920x1080  1080p 2106k , av01.0.08M.08, 25fps, video only, 40.74MiB
248          webm       1920x1080  1080p 2666k , vp9, 25fps, video only, 58.46MiB
137          mp4        1920x1080  1080p 4640k , avc1.640028, 25fps, video only, 78.96MiB
18           mp4        640x360    360p  601k , avc1.42001E, 25fps, mp4a.40.2@ 96k (44100Hz), 15.19MiB (best)
```
This gives a lot of useful information for our downloader, but we are mainly interested in three columns. The **format code**, the **note** and the last part of the last column, which is the **size** of these formats. We will not see the code of the **FormatAndSizeSetter** function because it's huge, but basically what it does is for a given format, it tries to find it in this list (the example above). If it finds it, it gets the format code on the left, then finds the highest audio quality regardless of the video format, and gets the audio format code which is also in the leftmost column. And finally, calculates the size for these two formats (video and audio) from the rightmost column and returns them to the calling function. If the format cannot be found, it returns the highest one available (by resolution and bitrate).
For our example though, let's say we are downloading 1080p video. Our FormatAndSizeSetter function would select the **137** format code (since it has the highest bitrate of all 1080p formats) and the **251** audio format code (since it has the highest bitrate of all video formats), it would sum up their sizes to **82.24MiB** and would return a string array with two elements: { **137+251** , **82.24MB** }.
#### downloadThread
Finally, the download thread creates a youtube-dl process with the following arguments:
```
youtube-dl -f format -o location video-id
```
In our case, the format is 137+251, which to youtube-dl it means: **download video with format code 137 and audio with format code 251, and combine them using ffmpeg**. The code for the downloadThread function is:
```csharp
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
            download.Start();
            while (!download.StandardOutput.EndOfStream)
            {
                if(Status!=10)
                    UpdateDataFromDownload(download.StandardOutput.ReadLine());
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
        }
```
While the video is downloading, youtube-dl provides the following information:
```
[download]  27.4% of 78.96MiB at 14.16MiB/s ETA 00:04
```
This is just a moment in downloading the video part of the video in our example, and using our function **UpdateDataFromDownload(string data)**, all of our object's data is constantly being updated. This data is later fetched in the upper hierarchy of our program so it can be shown to the user via our GUI.
### Additional details
At this point, we can see that our program creates approximately **three processes of youtube-dl for a video download**. Even though they are not super-intensive, they can quickly become **performance-intensive** if we are downloading a lot of videos at the same time. Although the major performance hit happens when starting the download and when the data is being fetched with multiple processes in the background, we still need to limit how many processes can be created. That is why I used a **Semaphore** for controlling the threads and setting a **maximum number of simultaneous video downloads**. The default is set to 3, but the user can change it any time in the settings of the program (which we will also see at the [end of this document](#5-usage)). While we are here, here's the static class for obtaining all video IDs from a single playlist in my program:
```csharp
///<summary>Returns a dictionary of strings with video id as key and video name as value.</summary>
        static public Dictionary<string, string> GetIdsAndNames(string PlaylistId)
            {
            StringBuilder TextData = new StringBuilder();
            TextData.Append("[");
            Dictionary<string, string> playlistInfo = new Dictionary<string, string>();
            Process playlistProcess = new Process();
            playlistProcess.StartInfo.FileName = "youtube-dl";
            playlistProcess.StartInfo.Arguments = "-j --flat-playlist " + PlaylistId;
            playlistProcess.StartInfo.UseShellExecute = false;
            playlistProcess.StartInfo.RedirectStandardOutput = true;
            playlistProcess.StartInfo.CreateNoWindow = true;
            playlistProcess.Start();
            while(!playlistProcess.StandardOutput.EndOfStream)
            {
                TextData.Append(playlistProcess.StandardOutput.ReadLine());
                TextData.Append(",");
            }
            TextData.Append("]");
            dynamic JsonData = JsonConvert.DeserializeObject(TextData.ToString());
            foreach (var item in JsonData)
            {
                playlistInfo.Add((string)item.id, (string)item.title);
            }
            return playlistInfo;
        }
```
This is why I have two constructors for video and two constructors for audio downloads. Because when fetching playlist info, the titles of the videos are also provided by youtube-dl, so I **skip the NameSetter** thread.
## **4. The GUI**
Even though there are many Forms in the GUI, the three main forms are the **Main MDI Form**, the Download Form, or as I like to call it, the **Download Tab** and the **Activity Tab**. From the screenshots in the [usage section](#5-usage) below, it can be graphically seen everything that is described textually here. Let's briefly explain how these two tabs are implemented and what the user does in each of them.
### Download Tab
In the download tab there is a **TextBox** control where the user enters the desired videos (video links) to be downloaded. The textbox checks wheter the link is a valid YouTube link, and if it is, it automatically pastes the link when the mouse enters the textbox. Then, there's a **Format Selection** dropdown list, where the user specifies in which format the videos shall be downloaded. There is also a **Location** link, where the user specifies where the videos should be downloaded. And finally, if the user chooses to download an mp3 audio file, the **Trimming option** becomes available where the user specifies start and end time for the audio being downloaded.
In addition, here's the code of how the **IDs of all the videos and playlists** provided as links in the textbox are extracted:
```csharp
static class IdExtractor
    {
        static Regex shortLinkRegex = new Regex(@"youtu\.be/\S{11}");
        static Regex longLinkRegex = new Regex(@"youtube\.com/watch\?v=\S{11}");
        static Regex playlistRegex = new Regex(@"youtube\.com/playlist\?list=\S{34}");
        static Regex playlistAndVideoRegex = new Regex(@"youtube\.com/watch\?v=\S{11}&list=\S{34}");
        public static Dictionary<string, string> GetIdsAndTypes(string allLinks)
        {
            Dictionary<string, string> IdsAndType = new Dictionary<string, string>();
            string ID;
            foreach (Match videoMatch in playlistAndVideoRegex.Matches(allLinks))
            {
                ID = videoMatch.ToString().Replace("youtube.com/watch?v=", "").Replace("&list=", ",");
                if (!IdsAndType.ContainsKey(ID))
                    IdsAndType.Add(ID.Split(',')[0], ID.Split(',')[1]);
            }
            foreach (Match videoMatch in longLinkRegex.Matches(allLinks))
            {
                ID = videoMatch.ToString().Replace("youtube.com/watch?v=", "");
                if (!IdsAndType.ContainsKey(ID))
                    IdsAndType.Add(ID, "Video");
            }
            foreach (Match videoMatch in playlistRegex.Matches(allLinks))
            {
                ID = videoMatch.ToString().Replace("youtube.com/playlist?list=", "");
                if (!IdsAndType.ContainsKey(ID))
                    IdsAndType.Add(ID, "Playlist");
            }
            foreach (Match videoMatch in shortLinkRegex.Matches(allLinks))
            {
                ID = videoMatch.ToString().Replace("youtu.be/", "");
                if (!IdsAndType.ContainsKey(ID))
                    IdsAndType.Add(ID, "Video");
            }
            return IdsAndType;
        }
    }
```
### Activity Tab
In the activity tab, the user can view **all the videos that are currently being downloaded**, including their **Name, Size, Progress, Format, Download Speed, Status and ETA**. The user can select one or multiple videos from the list, and can execute several actions upon them, such as:
- **Pause/Resume download**
- **Cancel download**
- **Open the containing folder**
- **Remove the selected videos from the list**
- **Remove the selected videos from the computer**

As far as the implementation for this list goes, it is implemented with a **ListView** control, which uses **Strings** to show each of the videos' information fields. And for their progress, I am using **custom drawn Progress Bars** drawn in **Bitmaps**, which are stored in an **ImageList** linked to the **ListView** control and displayed in the Progress column. The custom drawn progress bars are stored in a class **MyProgressBar** which inherits from the main **ProgressBar** class. And in order for the progress bar to look the way I need it to look, here's the overrided **onPaint()** function in the **MyProgressBar** class:
```csharp
protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rec = e.ClipRectangle;
            rec.Width = (int)(rec.Width * ((double)Value / Maximum)) - 4;
            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);
            rec.Height = rec.Height - 4;
            switch (color)
            {
                case "b": e.Graphics.FillRectangle(Brushes.SteelBlue, 2, 2, rec.Width, rec.Height); break;
                case "o": e.Graphics.FillRectangle(Brushes.Orange, 2, 2, rec.Width, rec.Height); break;
                case "g": e.Graphics.FillRectangle(Brushes.Green, 2, 2, rec.Width, rec.Height); break;
                case "r": e.Graphics.FillRectangle(Brushes.Red, 2, 2, rec.Width, rec.Height); break;
                default: e.Graphics.FillRectangle(Brushes.SteelBlue, 2, 2, rec.Width, rec.Height); break;
            }
            if (statusText == null)
            {
                e.Graphics.DrawString(Value.ToString() + "%",
                    new Font("Arial", 10, FontStyle.Regular),
                    Brushes.Black, DisplayRectangle, centered);
            }
            else
            {
                e.Graphics.DrawString(statusText,
                    new Font("Arial", 10, FontStyle.Regular),
                    Brushes.Black, DisplayRectangle, centered);
            }
        }
```
### Settings
And finally, in the Settings Form, the user can change the **language** of the program (English and Macedonian supported as of now, more to be added soon), as well as the **maximum number of simultaneous video downloads**.
## **5. Usage**
Finally, we get to see how this application looks like and how we can use it! Like we mentioned in the [GUI section](#4-the-gui) above, the two main tabs of the program are the **Download Tab** and the **Activity Tab**.
### Download Tab
![](http://gp.mk/mainEng.jpg)

On this tab, the user can do the following:
1. Open the Settings window
2. Go to the Activity Tab
3. Paste a YouTube video/playlist URL
4. Or alternatively, instead of hovering over the TextBox, a button for pasting
5. Choose the desired video/audio format
6. Force the video to download in 60fps
7. Choose where to download
8. If format is MP3, write start time for trimming
9. If format is MP3, write end time for trimming
10. Download the videos

### Activity Tab
![](http://gp.mk/activityEng.jpg)

On this tab, the user can do the following:
1. Open the Settings window
2. Go to the Download Tab
3. Pause or resume downloading the selected videos
4. Stop downloading the selected videos
5. Delete the selected videos from the activity tab
6. Delete the selected videos from the computer
7. Open the containing folder of the selected videos
8. Select a video, or right-click for the same options as above

### Settings
![](http://gp.mk/settingsEng.jpg)

---
![](http://gp.mk/ytdMkd.jpg)
#### [English](#македонски)
Јутјуб Давнлодер е графичка апликација која овозможува симнување на Јутјуб видеа во најразлични формати, од **MP3 аудио** до највисокиот видео квалитет којшто Јутјуб го нуди **(8K 60свс)**. Го прави тоа користејќи го [youtube-dl](https://youtube-dl.org/) АПИто, како и [ffmpeg](https://ffmpeg.org/) за конвертирање во MP3.
## **1. Главни Карактеристики**
  - Практично **секој формат** којшто Јутјуб го нуди е поддржан и избирлив
  - **Симнување на повеќе видеа истовремено**
  - **Плејлист линкови** се исто така поддржани, кои автоматски ги додаваат сите видеа за симнување
  - **Паузирање и продолжување на симнувања**
  - **Кратење** на аудио (MP3) downloads

## **2. Главна Идеја**
Оваа апликација беше многу инспирирана од [YTD Video Downloader](https://www.ytddownloader.com/), што е „Јутјуб симнувач“ што се плаќа, но најчесто не работи, особено кога станува збор за музички видеа на Јутјуб, поради начинот на којшто тие се хостирани на Јутјуб. [Youtube-dl](https://youtube-dl.org/) пак, го надминува овој проблем и е надградуван многу често со цел секогаш да функционира и со најновите „начини на сервисирање на видеа“ на Јутјуб. Единствената негова мана е тоа што е „command-line“ апликација, па не е баш примамлива за поголемиот број од крајните корисници. И токму затоа решив да направам своја графичка верзија од истата.

## **3. Решение на проблемот**
Апликацијата е имплементирана користејќи ги стандардните податочни структури на C# и Windows Forms (како што се Листи, Речници, Низи итн.), како и [Json.NET](https://www.newtonsoft.com/json) за превземање на податоците од видеата и плејлистите преку [youtube-dl](https://youtube-dl.org/). Во срцето на мојата апликација стои класата **DownloadThread**.
```csharp
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
        private int Status;
        private string ETA;
        private bool UpdateSizePeriodically;
        private bool queuedForStop;
        private int FormatErrorCounter;
        private Trim trim;
        public DownloadThread(string ID, string Location, string Format) //video
        public DownloadThread(string ID, string Name, string Location, string Format) //video from playlist
        public void CancelDownload()
        public void PauseDownload()
        void PrepareDownload(string ID, string Format)
        void StartDownload(string ID, string Location)
        void UpdateDataFromDownload(String Line)
        public DownloadThread(string ID, string Location, string Format, Trim Trim, string NullForAudioDifferantiation) //audio
        public DownloadThread(string ID, string Name, string Location, string Format, Trim Trim, string NullForAudioDifferantiation) //audio from playlist
        void StartAudioDownload(string ID, string Location)
        void UpdateDataFromAudioDownload(String Line)
        void setName(string ID)
        void setFormatAndSize(string ID, string Format)
        void setFormatAndSize(string ID)
        static string getName(string ID)
        static string[] getFormatAndSizeForVideoDownload(string ID, string Format) //returns an array of two strings, one is format ex: 312+bestaudio, and the second one is size

        public string getName()
        public string getSize()
        public int getProgress()
        public string getDownloadSpeed()
        public int getStatus()
        public string getETA()
        public void queueForStop()
    }
```
Тука се случува поголемиот дел од процесирањето во Јутјуб Давнлодер. Во основа, влезните податоци во DownloadThread класата се состојат од следниве аргументи:
- Видео ИД
- Локација (каде да се симне)
- Формат
- Информација за кратење (ако одбраниот формат е MP3)

**Видео ИДто** е токму тоа што пишува: тоа е ИДто од Јутјуб видеото, од кое [youtube-dl](https://youtube-dl.org/) може да превземе податоци, формати, како и да го симне истото тоа видео. Тоа е впрочем болдираниот дел (https[]()://www[]().youtube.com/watch?v=**dQw4w9WgXcQ**) од секој Јутјуб видео линк. Ќе видиме подоцна во документов како ова ИД влегува во аргументите на [youtube-dl](https://youtube-dl.org/) за превземање на податоци и симнување на видеото.

**Локацијата** е локален директориумски линк кој корисникот го бира и кој прикажува каде видеото/видеата ќе бидат симнати.

**Форматот** е видео/аудио форматот кој повторно, корисникот го бира пред да го симне видеото, и видеото ќе биде симнато во тој формат. Ако видеото е во послаб квалитет од корисничко-избраниот, тогаш највисокиот достапен квалитет ќе биде симнат.

Конечно, информацијата за **Кратење** се времиња кои корисникот ги задава за кратење на симнатото MP3 аудио.

### А сега, ајде да навлеземе во повеќе детали!
Накратко ќе продискутираме како се одвива симнувањето. Ќе се фокусираме на видео делот и на видео конструкторот од DownloadThread класата, бидејќи симнувањето на аудио е слично и поедноставно. Ајде прво да го разгледаме кодот:
```csharp
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
```
Доста едноставно, ги превзема аргументите и ги додава соодветно на податоците во класата. На самиот крај, ја стартува **PrepareAndDownload** нишката. Тука се случува целата магија! :) Оваа нишка постои за конструкторот да не мора да чека сите процеси да завршат, па дури после да го иницијализира објектот. Така што, ајде да ја видиме нишката PrepareAndDownload:
```csharp
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
```
[Youtube-dl](https://youtube-dl.org/) нуди многу команди за превземање на информации од едно видео, како на пример наслов, „thumbnail“, број на „лајкови“ и „дислајкови“, ИДиња на формати, големина итн. За жал, не сите информации можат да бидат дознаени преку една команда, што значи дека youtube-dl треба да биде повикан (креиран како процес) неколку пати. Токму тука нишките **NameSetter**, **FormatAndSizeSetter** и **downloadThread** доаѓаат во игра. Иако се само-објаснувачки, ајде да видиме накратко како тие работат:
#### NameSetter
Оваа нишка креира youtube-dl процес, така што целосните аргументи се:
```
youtube-dl -j video-id
```
Оваа команда ги враќа сите текстуални информации за видеото. Постои и команда „youtube-dl -e video-id“ која го враќа само насловот од видеото, но поради ограничувањата на енкодирањето на конзолата во C#, кирилични наслови едноставно не можат да бидат прочитани. Но кога се читаат json податоци, youtube-dl ги енкодира кириличните карактери во UTF-8 код, па кога се десеријализираат преку [Json.NET](https://www.newtonsoft.com/json), можат да бидат прочитани. Затоа, кодот за оваа нишка е следниов:
```csharp
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
```
Со ова, го имаме насловот на видеото кое се симнува, којшто ќе биде прикажан во нашиот графички интерфејс, како што може да се види на [крајот од овој документ](#5-користење).
#### FormatAndSizeSetter
Оваа нишка ја повикува следнава команда во youtube-dl:
```
youtube-dl -F video-id
```
Еве пример како изгледа излезот од оваа команда за некое видео:
```
youtube-dl -F dQw4w9WgXcQ
[youtube] dQw4w9WgXcQ: Downloading webpage
[info] Available formats for dQw4w9WgXcQ:
format code  extension  resolution note
249          webm       audio only tiny   49k , opus @ 50k (48000Hz), 1.18MiB
250          webm       audio only tiny   65k , opus @ 70k (48000Hz), 1.55MiB
140          m4a        audio only tiny  130k , m4a_dash container, mp4a.40.2@128k (44100Hz), 3.27MiB
251          webm       audio only tiny  136k , opus @160k (48000Hz), 3.28MiB
394          mp4        256x144    144p   73k , av01.0.00M.08, 25fps, video only, 1.72MiB
278          webm       256x144    144p   97k , webm container, vp9, 25fps, video only, 2.25MiB
160          mp4        256x144    144p  107k , avc1.4d400c, 25fps, video only, 2.05MiB
395          mp4        426x240    240p  159k , av01.0.00M.08, 25fps, video only, 3.42MiB
242          webm       426x240    240p  217k , vp9, 25fps, video only, 4.04MiB
133          mp4        426x240    240p  290k , avc1.4d4015, 25fps, video only, 4.48MiB
396          mp4        640x360    360p  340k , av01.0.01M.08, 25fps, video only, 6.68MiB
243          webm       640x360    360p  396k , vp9, 25fps, video only, 6.96MiB
134          mp4        640x360    360p  484k , avc1.4d401e, 25fps, video only, 8.27MiB
244          webm       854x480    480p  586k , vp9, 25fps, video only, 10.03MiB
397          mp4        854x480    480p  603k , av01.0.04M.08, 25fps, video only, 11.37MiB
135          mp4        854x480    480p  741k , avc1.4d401e, 25fps, video only, 11.56MiB
247          webm       1280x720   720p 1035k , vp9, 25fps, video only, 17.67MiB
136          mp4        1280x720   720p 1077k , avc1.4d401f, 25fps, video only, 16.72MiB
398          mp4        1280x720   720p 1133k , av01.0.05M.08, 25fps, video only, 22.07MiB
399          mp4        1920x1080  1080p 2106k , av01.0.08M.08, 25fps, video only, 40.74MiB
248          webm       1920x1080  1080p 2666k , vp9, 25fps, video only, 58.46MiB
137          mp4        1920x1080  1080p 4640k , avc1.640028, 25fps, video only, 78.96MiB
18           mp4        640x360    360p  601k , avc1.42001E, 25fps, mp4a.40.2@ 96k (44100Hz), 15.19MiB (best)
```
Од тука имаме многу потребни информации за нашиот симнувач, но главно се интересираме за три колони. А тоа се **format code**, **note** и последниот дел од последната колона, што се всушност **големините** на овие формати. Нема да го разгледаме кодот на **FormatAndSizeSetter** функцијата бидејќи е огромен, но во основа тоа што прави е за даден формат, пробува да го најде на листата (примерот погоре). Ако го најде, го превзема формат кодот од левата страна, потоа го наоѓа најдобриот аудио квалитет без разлика на тоа кој видео формат е одбран, и го превзема формат кодот за аудиото кој е исто така во најлевата колона. И конечно, ја пресметува големината за овие два формати (видео и аудио) од најдесната колона и ги враќа овие информации кај повикувачката функција. Ако форматот не може да биде пронајден, го враќа најдобриот достапен (гледајќи резолуција и „bitrate“).
За нашиот пример пак, да речеме дека симнуваме 1080p видео. Нашата FormatAndSizeSetter функција ќе го селектира формат кодот **137** (бидејќи го има највисокиот „bitrate“ од сите 1080p формати) и аудио формат кодот **251** (бидејќи го има највисокиот „bitrate“ од сите аудио формати), ќе ги собере нивните големини во **82.24MiB** и ќе врати низа од стрингови со два елемента: { **137+251** , **82.24MB** }.
#### downloadThread
Конечно, нишката за симнување креира youtube-dl процес со следниве аргументи:
```
youtube-dl -f format -o location video-id
```
Во нашиов случај, форматот е 137+251, кој му кажува на youtube-dl: **симни видео со формат код 137 и аудио со формат код 251, и спој ги преку ffmpeg**. Кодот за downloadThread функцијата е:
```csharp
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
            download.Start();
            while (!download.StandardOutput.EndOfStream)
            {
                if(Status!=10)
                    UpdateDataFromDownload(download.StandardOutput.ReadLine());
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
        }
```
Додека видеото се симнува, youtube-dl ја прикажува следнава информација:
```
[download]  27.4% of 78.96MiB at 14.16MiB/s ETA 00:04
```
Ова е само еден момент од симнувањето на видео делот од нашето видео, и користејќи ја нашата функција **UpdateDataFromDownload(string data)**, сите податоци на нашиот објект константно се ажурираат. Овие податоци подоцна се превземани во горната хиерархија од нашата програма за да можат да бидат графички прикажани на корисникот.
### Дополнителни детали
Од тука, можеме да видиме дека нашата програма креира отприлика **три процеси од youtube-dl за симнување на едно видео**. Иако тие не се супер-интензивни, многу брзо можат да станат **интенизивни по перформансот** ако симнуваме многу видеа во исто време. Иако главниот удар по перформансите се случува за време на стартување на симнувањето кога информациите за видеото се добиваат од многу различни процеси во позадина, сепак треба да ограничиме колку процеси можат да се создадат. Затоа искористив **Семафор** за контролирање на нишките и воспоставување **максимален број на истовремени видео симнувања**. На почеток оваа бројка е поставена на 3, но корисникот може да ја смени било кога од поставувањата на програмата (кои ќе ги видиме на [крајот од овој документ](#5-користење)). Додека сме кај симнувања на повеќе видеа, еве ја статичната класа за превземање на сите видео ИДиња од една плејлиста во мојата програма:
```csharp
///<summary>Returns a dictionary of strings with video id as key and video name as value.</summary>
        static public Dictionary<string, string> GetIdsAndNames(string PlaylistId)
            {
            StringBuilder TextData = new StringBuilder();
            TextData.Append("[");
            Dictionary<string, string> playlistInfo = new Dictionary<string, string>();
            Process playlistProcess = new Process();
            playlistProcess.StartInfo.FileName = "youtube-dl";
            playlistProcess.StartInfo.Arguments = "-j --flat-playlist " + PlaylistId;
            playlistProcess.StartInfo.UseShellExecute = false;
            playlistProcess.StartInfo.RedirectStandardOutput = true;
            playlistProcess.StartInfo.CreateNoWindow = true;
            playlistProcess.Start();
            while(!playlistProcess.StandardOutput.EndOfStream)
            {
                TextData.Append(playlistProcess.StandardOutput.ReadLine());
                TextData.Append(",");
            }
            TextData.Append("]");
            dynamic JsonData = JsonConvert.DeserializeObject(TextData.ToString());
            foreach (var item in JsonData)
            {
                playlistInfo.Add((string)item.id, (string)item.title);
            }
            return playlistInfo;
        }
```
Еве зошто и имам два конструктори за видео и два конструктори за аудио симнувања. Бидејќи кога се превземаат информации од плејлиста, насловите на видеата се исто така дадени од страна на youtube-dl, па затоа **ја прескокнувам NameSetter** нишката.
## **4. Графичкиот Интерфејс**
Иако има многу Форми во графичкиот интерфејс, главните три форми се **Главната MDI Форма**, Симнувачката Форма, или како што јас сакам да ја викам, **Симнување Табот** и **Активност Табот**. Од скриншотовите во [користење секцијата](#5-користење) подоле, може графички да се види се што е текстуално опишано тука. Ајде накратко да објасниме како овие два таба се имплементирани и што прави корисникот во секој од нив.
### Симнување Таб
Во симнувачкиот таб има една **TextBox** контрола каде што корисникот ги внесува видеата (видео линкови) што сака да ги симне. TextBox-от проверува дали линкот е валиден Јутјуб линк, и ако е, автоматски го вметнува линкот кога глувчето влегува во TextBox-от. Потоа, има **селекција на формат** преку dropdown листа, каде што корисникот бира во кој формат сака видеата да ги симне. Понатаму има и линк за **Локација**, каде што корисникот кажува каде да бидат симнати видеата. И конечно, ако корисникот одбере да симне mp3 аудио фајл, тогаш **опцијата за кратење** се овозможува каде што корисникот внесува почетно и крајно време за аудиото кое сака да го симне.
Дополнително, еве го кодот за тоа како **сите ИДиња од сите видеа и плејлисти** дадени како линкови во TextBox-от се извлечени:
```csharp
static class IdExtractor
    {
        static Regex shortLinkRegex = new Regex(@"youtu\.be/\S{11}");
        static Regex longLinkRegex = new Regex(@"youtube\.com/watch\?v=\S{11}");
        static Regex playlistRegex = new Regex(@"youtube\.com/playlist\?list=\S{34}");
        static Regex playlistAndVideoRegex = new Regex(@"youtube\.com/watch\?v=\S{11}&list=\S{34}");
        public static Dictionary<string, string> GetIdsAndTypes(string allLinks)
        {
            Dictionary<string, string> IdsAndType = new Dictionary<string, string>();
            string ID;
            foreach (Match videoMatch in playlistAndVideoRegex.Matches(allLinks))
            {
                ID = videoMatch.ToString().Replace("youtube.com/watch?v=", "").Replace("&list=", ",");
                if (!IdsAndType.ContainsKey(ID))
                    IdsAndType.Add(ID.Split(',')[0], ID.Split(',')[1]);
            }
            foreach (Match videoMatch in longLinkRegex.Matches(allLinks))
            {
                ID = videoMatch.ToString().Replace("youtube.com/watch?v=", "");
                if (!IdsAndType.ContainsKey(ID))
                    IdsAndType.Add(ID, "Video");
            }
            foreach (Match videoMatch in playlistRegex.Matches(allLinks))
            {
                ID = videoMatch.ToString().Replace("youtube.com/playlist?list=", "");
                if (!IdsAndType.ContainsKey(ID))
                    IdsAndType.Add(ID, "Playlist");
            }
            foreach (Match videoMatch in shortLinkRegex.Matches(allLinks))
            {
                ID = videoMatch.ToString().Replace("youtu.be/", "");
                if (!IdsAndType.ContainsKey(ID))
                    IdsAndType.Add(ID, "Video");
            }
            return IdsAndType;
        }
    }
```
### Активност Таб
Во активност табот, корисникот може да ги види **сите видеа кои се симнуваат во даден момент**, вклучувајќи го нивното **Име, Големина, Прогрес, Формат, Брзина на Симнување, Статус и Преостанато Време**. Корисникот може да одбере едно или повеќе видеа од листата, врз кои може да изврши неколку акции, а тоа се:
- **Паузирај/Продолжи симнување**
- **Сопри симнување**
- **Отвори го фолдерот каде што се наоѓаат видеата**
- **Избриши ги селектираните видеа од листата**
- **Избриши ги селектираните видеа од компјутер**

Во врска со имплементацијата на оваа листа, имплементирана е преку **ListView** контрола, која користи **Стрингови** за да ги прикаже поединечно сите информации за видеата. Додека пак за нивниот прогрес, користам **корисничко исцртани Прогрес Бар контроли**, кои се цртаат во **Bitmap-ови**, кои пак се зачувани во **ImageList** која е поврзана со **ListView** контролата и прикажана во Прогрес колоната. Корисничко исцртаните прогрес барови се зачувани во класа **MyProgressBar**, која наследува од главната **ProgressBar** класа. И за да прогрес барот изгледа така како што ми треба да изгледа, еве го преоптоварениот **onPaint()** метод во **MyProgressBar** класата:
```csharp
protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rec = e.ClipRectangle;
            rec.Width = (int)(rec.Width * ((double)Value / Maximum)) - 4;
            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);
            rec.Height = rec.Height - 4;
            switch (color)
            {
                case "b": e.Graphics.FillRectangle(Brushes.SteelBlue, 2, 2, rec.Width, rec.Height); break;
                case "o": e.Graphics.FillRectangle(Brushes.Orange, 2, 2, rec.Width, rec.Height); break;
                case "g": e.Graphics.FillRectangle(Brushes.Green, 2, 2, rec.Width, rec.Height); break;
                case "r": e.Graphics.FillRectangle(Brushes.Red, 2, 2, rec.Width, rec.Height); break;
                default: e.Graphics.FillRectangle(Brushes.SteelBlue, 2, 2, rec.Width, rec.Height); break;
            }
            if (statusText == null)
            {
                e.Graphics.DrawString(Value.ToString() + "%",
                    new Font("Arial", 10, FontStyle.Regular),
                    Brushes.Black, DisplayRectangle, centered);
            }
            else
            {
                e.Graphics.DrawString(statusText,
                    new Font("Arial", 10, FontStyle.Regular),
                    Brushes.Black, DisplayRectangle, centered);
            }
        }
```
### Поставувања
И конечно, во Формата за Поставувања, корисникот може да го смени **јазикот** на програмата (за сега поддржани се само Англиски и Македонски, наскоро и други ќе бидат додадени), како и **максималниот број на истовремени видео симнувања**.
## **5. Користење**
Конечно, можеме да видиме како оваа апликација изгледа и како се користи! Како што спомнавме во [графичката секција](#4-графичкиот-интерфејс) погоре, главните два таба во програмава се **Симнување Табот** и **Активност Табот**.
### Симнување Таб
![](http://gp.mk/mainMkd.jpg)

Во овој таб, корисникот може да го прави следново:
1. Отвори го прозорецот за поставувања
2. Оди во Активност Табот
3. Залепи линк од Јутјуб видео/плејлиста
4. Или алтернативно, наместо со глувчето врз TextBox-от, копче за вметнување линк
5. Одбирање на посакуваниот видео/аудио формат
6. Форсирај видеото да биде симнато во 60свс
7. Одбери каде да се симне
8. Ако форматот е MP3, напиши почетно време за кратење
9. Ако форматот е MP3, напиши завршно време за кратење
10. Симни ги видеата

### Активност Таб
![](http://gp.mk/activityMkd.jpg)

Во овој таб, корисникот може да го прави следново:
1. Отвори го прозорецот за поставувања
2. Оди во Симнување Табот
3. Паузирај или продолжи со симнување на селектираните видеа
4. Сопри симнување за селектираните видеа
5. Избриши ги селектираните видео од активност табот
6. Избриши ги селектираните видео од компјутерот
7. Отвори го фолдерот каде што се наоѓаат селектираните видеа
8. Одбери видео, или десен-клик за истите опции како и погоре

### Поставувања
![](http://gp.mk/settingsMkd.jpg)
