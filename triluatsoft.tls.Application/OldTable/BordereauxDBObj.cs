using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    public class BordereauxDBObj
    {
        public string ID { get; set; }

        [StringLength(25)]
        public string ClaimID { get; set; }

        public DateTime? DateOfSurvey { get; set; }

        public int? ReportID { get; set; }

        public DateTime? RecentCorrespondence { get; set; }

        public DateTime? DateOfReport { get; set; }

        [StringLength(2)]
        public string BordereauxStatusID { get; set; }

        [StringLength(100)]
        public string OtherStatus { get; set; }

        public int? FollowUpID { get; set; }

        [StringLength(100)]
        public string OtherFollowUp { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public decimal? Reserve { get; set; }

        public decimal? ProgressPayment { get; set; }
    }
}
