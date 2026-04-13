using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    public class TaskView
    {
        public TaskView()
        {
            IsNew = true;
            MarkAsDeleted = false;
            //HasTimesheet = false;
        }

        public int ID { get; set; }
        public int? TaskNameID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Type { get; set; } //true: Task assignment; false: submit timesheet request
        public string ClaimID { get; set; }//hvtam-04032016 -enable lại quan hệ task tạo cho claim //Thanh-19092014 public string ClaimID { get; set; }
        public int TimesheetID { get; set; }
        public int? EmployeeID { get; set; }    //asignee ID
        public string Assignee { get; set; }
        //public decimal? WorkingHour { get; set; }
        //public decimal? ChargedHour { get; set; }
        public bool? IsCompleted { get; set; }
        //public decimal? TotalFee { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public string Description { get; set; }

        //public bool HasTimesheet { get; set; } //Deprecated
        public string TaskName { get; set; }
        public string JobCode { get; set; }
        public bool IsNew { get; set; }
        public bool MarkAsDeleted { get; set; }
        public string CreatedUser { get; set; }
        public decimal UserFee { get; set; }
        public decimal TaskFee { get; set; }
        public bool IsEditable { get; set; }//hvtam-04032016 //Thanh-19092014 public bool IsEditable { get; set; }
        public bool IsMissed { get; set; }

        public override int GetHashCode()
        {
            return ID.GetHashCode()
                ^ (TaskNameID == null ? 0 : TaskNameID.GetHashCode())
                ^ (StartDate == null ? 0 : StartDate.Value.GetHashCode())
                ^ (EndDate == null ? 0 : EndDate.Value.GetHashCode())
                ^ (Type == null ? 0 : Type.Value.GetHashCode())
                ^ (string.IsNullOrWhiteSpace(ClaimID) ? 0 : ClaimID.GetHashCode()) //hvtam-04032016 //Thanh-19092014 ^ (string.IsNullOrWhiteSpace(ClaimID) ? 0 : ClaimID.GetHashCode())
                ^ (EmployeeID == null ? 0 : EmployeeID.Value.GetHashCode())
                ^ (string.IsNullOrWhiteSpace(Assignee) ? 0 : Assignee.GetHashCode())
                //^ (WorkingHour == null ? 0 : WorkingHour.Value.GetHashCode())
                //^ (ChargedHour == null ? 0 : ChargedHour.Value.GetHashCode())
                ^ (IsCompleted == null ? 0 : IsCompleted.Value.GetHashCode())
                //^ (TotalFee == null ? 0 : TotalFee.Value.GetHashCode())
                ^ (CreatedDate == null ? 0 : CreatedDate.Value.GetHashCode())
                //^ HasTimesheet.GetHashCode()
                ^ IsNew.GetHashCode()
                ^ MarkAsDeleted.GetHashCode()
                ^ (string.IsNullOrWhiteSpace(Description) ? 0 : Description.GetHashCode());
        }
    }
}
