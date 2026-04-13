using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class CreateOrUpdateContributionAdjustmentInput
    {
        public int? ID { get; set; }
        public int? EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public DateTime AdjustDate { get; set; }
        public decimal AdjustAMT { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string Description { get; set; }
    }
}
