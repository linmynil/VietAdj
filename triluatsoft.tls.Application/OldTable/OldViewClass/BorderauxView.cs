using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.OldViewClass
{
    public class BorderauxView
    {
        public int ID { get; set; }
        public string ClaimID { get; set; }
        public DateTime? DateOfSurvey { get; set; }
        public DateTime? RecentCorrespondence { get; set; }

        public int? ReportID { get; set; }
        public string ReportName { get; set; }

        public DateTime? DateOfReport { get; set; }

        public string ClaimStatusID { get; set; } //Claim Borderaux status
        public string StatusName { get; set; }
        public string OtherStatus { get; set; }

        public int? FollowUpID { get; set; }
        public string FollowUp { get; set; }
        public string OtherFollowUp { get; set; }

        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }

        public bool IsClosed { get; set; }      //se bo???
        //public bool? isActive { get; set; }     //hvtam-20022016

        public decimal? Reserve { get; set; }//hvtam-20022016 public string Reserve { get; set; }  //hvtam
        public decimal? ProgressPayment { get; set; } //hvtam-20022016
        public override int GetHashCode()
        {
            return (string.IsNullOrWhiteSpace(ClaimID) ? 0 : ClaimID.GetHashCode())
                ^ DateOfSurvey.GetValueOrDefault(DateTime.MinValue).GetHashCode()
                ^ ReportID.GetValueOrDefault(0).GetHashCode()
                ^ DateOfReport.GetValueOrDefault(DateTime.MinValue).GetHashCode()
                //^ RecentCorrespondence.GetValueOrDefault(DateTime.MinValue).GetHashCode()
                ^ (string.IsNullOrWhiteSpace(ClaimStatusID) ? 0 : ClaimStatusID.GetHashCode())
                ^ (string.IsNullOrWhiteSpace(OtherStatus) ? 0 : OtherStatus.GetHashCode())
                ^ FollowUpID.GetValueOrDefault(0).GetHashCode()
                ^ (string.IsNullOrWhiteSpace(OtherFollowUp) ? 0 : OtherFollowUp.GetHashCode())

                //hvtam-29022016 them phan reserve & payment
                ^ (Reserve.HasValue ? Reserve.Value.GetHashCode() : -999999999)     //hvtam-15032016: nulll thi tra ve gia tri 999999999 de phan biet voi 0 //(Reserve.HasValue ? Reserve.Value.GetHashCode() : 0)
                ^ (ProgressPayment.HasValue ? ProgressPayment.Value.GetHashCode() : -999999999); //hvtam-15032016: nulll thi tra ve gia tri 999999999 đe phan biet voi 0 //(Reserve.HasValue ? Reserve.Value.GetHashCode() : 0)
        }
    }
}
