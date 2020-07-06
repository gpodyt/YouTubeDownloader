using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;

namespace YTDownloader
{
    static class IdAndNameExtractorFromPlaylist
    {
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
    }
}
