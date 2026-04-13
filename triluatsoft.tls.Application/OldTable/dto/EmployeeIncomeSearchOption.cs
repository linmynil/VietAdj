using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class EmployeeIncomeSearchOption
    {
        public int? EmployeeID { get; set; }
        public int? CIncomeTypeID { get; set; }
        public bool? Contribution { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
