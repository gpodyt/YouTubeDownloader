using System.Drawing;
using System.Windows.Forms;

namespace YTDownloader
{
    class MyProgressBar : ProgressBar
    {
        private string color;
        private string statusText;
        private StringFormat centered;
        public MyProgressBar()
        {
            SetStyle(ControlStyles.UserPaint, true);
            color = "b";
            statusText = null;
            centered = new StringFormat();
            centered.Alignment = StringAlignment.Center;
            centered.LineAlignment = StringAlignment.Center;
        }
        /// <summary>
        /// Progress bar value and status code.
        /// </summary>
        public void changeValue(int value, int status)
        {
            Value = value;
            switch (status)
            {
                case 0: statusText = AllUserConfig.languageRM.GetString("pbStatus_done"); color = "g"; break;
                case 1: statusText = AllUserConfig.languageRM.GetString("pbStatus_loading"); color = "b"; break;
                case 2: statusText = null; color = "b"; break;
                case 3: statusText = null; color = "b"; break;
                case 4: statusText = null; color = "o"; break;
                case 5: statusText = AllUserConfig.languageRM.GetString("pbStatus_muxing"); color = "o"; break;
                case 6: statusText = AllUserConfig.languageRM.GetString("pbStatus_error"); color = "r"; Value = 100; break;
                case 7: statusText = AllUserConfig.languageRM.GetString("pbStatus_converting"); color = "o"; break;
                case 8: statusText = AllUserConfig.languageRM.GetString("pbStatus_cancelled"); color = "r"; break;
                case 9: statusText = AllUserConfig.languageRM.GetString("pbStatus_paused"); color = "o"; break;
                case 10: statusText = AllUserConfig.languageRM.GetString("pbStatus_done"); color = "g"; break;
                default: statusText = AllUserConfig.languageRM.GetString("pbStatus_error"); color = "r"; Value = 100; break;
            }
        }
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
    }
}
