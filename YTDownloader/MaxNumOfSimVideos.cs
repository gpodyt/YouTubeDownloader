using System.Threading;

namespace YTDownloader
{
    //Max number of simultaenous video downloads

    static class MaxNumOfSimVideos
    {
        static public Semaphore maxN;
        static public void changeNumber(int N)
        {
            maxN = new Semaphore(N, N);
        }
    }
}
