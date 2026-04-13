using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.OldViewClass
{
    public class ProfessionalFeeView
    {
        public int ProfessionalFeeID { get; set; }
        public int TimeSheetID { get; set; }

        public DateTime? InputDate { get; set; }
        public int? JobCodeID { get; set; }
        public string Notes { get; set; }
        //public decimal? WorkingHour { get; set; }  //hvtam-14122014 se bo???
        //public decimal? ApprovedHour { get; set; } //hvtam-14122014 se bo???

        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
        public int? UpdateBy { get; set; }


        public string JobName { get; set; }
        public decimal? StandardTime { get; set; }
        //public string JobCodeTime { get { return string.Format("{0}^{1}", JobCodeID, StandardTime.Value.ToString("N0")); } }
        public string JobCodeTime { get; set; }
        public string CreateByName { get; set; }
        public string ChargedByName { get; set; } //hvtam-20052015
        public decimal? FeePerHour { get; set; }
        public decimal? ProFeeValue { get; set; }

        public string TimeSheetName { get; set; }

        public TimeSpan? WorkTime { get; set; }
        public string WorkTimeStr {
            get { return CConvert.ToTimeSpan(WorkTime.Value); }
            set { CConvert.ToTimeSpan(WorkTime.Value); }
        }
        public TimeSpan? ApproveTime { get; set; }
        public string ApproveTimeStr
        {
            get { return ApproveTime.HasValue?CConvert.ToTimeSpan(ApproveTime.Value):""; }
            set { if (ApproveTime.HasValue) CConvert.ToTimeSpan(ApproveTime.Value); }
        }

        public int? ChargedBy { get; set; }  //hvtam-28042015 : user duoc tinh phi trong timesheet
        public decimal? ChargedFeePerHour { get; set; } //hvtam-28042015 : user duoc tinh phi trong timesheet
        public decimal? ChargedProFeeValue { get; set; } //hvtam-22042015 ApprovedProFeeValue

        public bool? isOwner { get; set; }
    }
}
