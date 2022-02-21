using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Graph07
{
    class Utilities
    {

        public static void DelayProc(Action proc, int milliseconds)
        {
            var t = new DispatcherTimer(DispatcherPriority.Render);
            t.Interval = new TimeSpan(0, 0, 0, 0, milliseconds);
            t.Tick += (ts, te) =>
            {
                ((DispatcherTimer)ts).Stop();
                proc();
            };
            t.Start();
        }
    }
}
