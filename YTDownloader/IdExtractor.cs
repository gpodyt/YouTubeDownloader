using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace YTDownloader
{
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
}
