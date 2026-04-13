using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class CreateOrUpdateTaskInput
    {
        public int? Id { get; set; }
        public string ClaimID { get; set; }
        public int? EmployeeID { get; set; }

        public int TaskNameID { get; set; }

        public string Description { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
