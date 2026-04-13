using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.View
{
    public class NotificationView
    {
        public NotificationView()
        {
            Style = "normal";
        }

        public string ClaimID { get; set; }
        public string Message { get; set; }
        public DateTime LastUpdate { get; set; }
        public string Style { get; set; }
        public DateTime? RecentCorrespondence { get; set; }
        public string Remark { get; set; }
    }
}
