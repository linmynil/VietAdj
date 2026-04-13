using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    [Table("Bordereaux")]
    public class Bordereaux : Entity
    {
        [StringLength(25)]
        public string ClaimID
        {
            get; set;
        }

        public DateTime? DateOfSurvey { get; set; }
        public DateTime? RecentCorrespondence { get; set; }


        public int? ReportID { get; set; }

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

        public virtual BordereauxStatus BordereauxStatus { get; set; }
        //public virtual ICollection<Claim> Claims { get; set; }
        public virtual FollowUp FollowUp { get; set; }
        public virtual Report Report { get; set; }
        //public virtual Claim Claim { get; set; }
    }
}
