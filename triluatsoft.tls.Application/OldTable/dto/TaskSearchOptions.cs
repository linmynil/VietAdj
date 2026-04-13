using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class TaskSearchOptions
    {
        public string TaskName { get; set; }
        public string ClaimID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Type { get; set; }
        public int? Status { get; set; }

        public int? CurrentUserID { get; set; }
        public bool SearchAll { get; set; }
    }
}
