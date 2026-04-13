using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class UpdateBorderauxStatusInput
    {
        public string ClaimId { get; set; }
        public int? ReportID { get; set; }
        public string ClaimStatusID { get; set; }
        public DateTime? RecentCorrespondence { get; set; }

        public string OtherStatus { get; set; }
        public int? FollowUpID { get; set; }
        public string OtherFollowUp { get; set; }
        public DateTime? DateOfReport { get; set; }
        public DateTime? DateOfSurvey { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public decimal? Reserve { get; set; }
        public decimal? ProgressPayment { get; set; }
        
    }
}
