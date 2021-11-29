using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChadWebCalendar.Data
{
    public class NotificationWorkerEventArgs : EventArgs
    {
        public DateTime? dt;
        public NotificationWorkerEventArgs(DateTime? _dt)
        {
            dt = _dt;
        }
    }
}
