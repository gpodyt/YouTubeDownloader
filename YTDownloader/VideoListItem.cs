using System.Drawing;
using System.Windows.Forms;

namespace YTDownloader
{
    class VideoListItem : ListViewItem
    {
        public Bitmap PBBitmap;
        public MyProgressBar PB;
        public VideoListItem(Video video, ref Bitmap PBBitmap, ref MyProgressBar PB)
        {
            this.PBBitmap = PBBitmap;
            this.PB = PB;
            Tag = video;
            SubItems.Add("jas");
            SubItems.Add("sum");
            SubItems.Add(video.getFormatForUpdate());
            SubItems.Add("gp");
            SubItems.Add("i");
            SubItems.Add("sum");
            SubItems.Add("najjak");
        }
        public VideoListItem(string playlistId)
        {
            SubItems.Add(playlistId);
            SubItems.Add("");
            SubItems.Add(AllUserConfig.languageRM.GetString("playlist"));
            SubItems.Add("");
            SubItems.Add(AllUserConfig.languageRM.GetString("playlistStatus"));
            SubItems.Add("");
            SubItems.Add("");
        }
    }
}
