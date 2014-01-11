using System.Diagnostics;
using System.Threading;

namespace Engine.Tools
{
    public class Tools
    {
        public static void TimeOut(ref bool status, int ms)
        {
            if (!status)
                return;
            Stopwatch st = new Stopwatch();
            st.Start();
            while (status)
            {
                if (st.ElapsedMilliseconds > ms)
                {
                    st.Stop();
                    return;
                }
                Thread.Sleep(1);
            }
        }
    }
}
