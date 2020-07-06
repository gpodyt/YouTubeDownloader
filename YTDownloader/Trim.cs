using System;

namespace YTDownloader
{
    public class Trim
    {
        DateTime startTime;
        DateTime endTime;
        int[] startAndEndIntegers;
        bool valid;
        public Trim(string startTime, string endTime)
        {
            valid = false;
            startAndEndIntegers = new int[6];
            int counter = 0;
            try
            {
                foreach (string s in startTime.Split(':'))
                {
                    startAndEndIntegers[counter++] = Int16.Parse(s);
                }
                foreach (string s in endTime.Split(':'))
                {
                    startAndEndIntegers[counter++] = Int16.Parse(s);
                }
                this.startTime = new DateTime(1, 1, 1, startAndEndIntegers[0], startAndEndIntegers[1], startAndEndIntegers[2]);
                this.endTime = new DateTime(1, 1, 1, startAndEndIntegers[3], startAndEndIntegers[4], startAndEndIntegers[5]);
            }
            catch (Exception ex)
            {
                startTime = null;
                endTime = null;
                return;
            }
            if (this.startTime.Ticks < this.endTime.Ticks)
                valid = true;
        }
        public bool isValid()
        {
            return valid;
        }
        public string GetStartTime()
        {
            return startAndEndIntegers[0] + ":" + startAndEndIntegers[1] + ":" + startAndEndIntegers[2];
        }
        public string GetEndTime()
        {
            return startAndEndIntegers[3] + ":" + startAndEndIntegers[4] + ":" + startAndEndIntegers[5];
        }
    }
}
