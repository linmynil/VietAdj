using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    public class ClaimView
    {
        public ClaimView()
        {
            TaskList = new List<TaskView>();
            EmployeeList = new List<EmployeeView>();
        }

        public string ID { get; set; }
        public int? InsurerID { get; set; }
        public int? BrokerID { get; set; }
        public string TheInsured { get; set; }
        public string InsuredProject { get; set; }
        public string RiskLocation { get; set; }
        public string ClientsRef { get; set; }
        public string PolicyNo { get; set; }
        public DateTime? DateOfLoss { get; set; }
        public DateTime? DateOfAssignment { get; set; }     //hvtam-02022016
        public int? TypeOfLossID { get; set; }
        public int? CauseID { get; set; }
        public string OtherCause { get; set; }
        public decimal? Estimate { get; set; }
        public string Currency { get; set; }
        public string Reserve { get; set; }//hvtam-20022016 public string Reserve { get; set; }  //hvtam
        //public decimal? ProgressPayment { get; set; } //hvtam-20022016
        public int? AccountManagerID { get; set; }
        public int? BordereauxID { get; set; }  //LastUpdateID =>BordereauxID
        public bool? Issued { get; set; }
        public DateTime? IssueDate { get; set; }
        public decimal? ExchangeRate { get; set; }
        public bool? Closed { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public List<TaskView> TaskList { get; set; }
        public List<EmployeeView> EmployeeList { get; set; } //List Assigned list

        public List<CustomerView> Co_OwnerList { get; set; } //List Co_Owner Insurance

        public string DebitNote { get; set; }
        public DateTime? Date { get; set; }
        public decimal? DiscountValue { get; set; }
        public string DiscountType { get; set; }
        public int? CTypeID { get; set; }  //hvtam-23092014
        public string CTypeName { get; set; }
        public int? FID { get; set; }  //hvtam-23092014; ID cua thu muc
        public int? OfficeID { get; set; }  //hvtam-23092014; ID cua thu muc
        public int? RefStatusID { get; set; }  //hvtam-22102014: Ref status: 1-Open, 2-Closed, 3-Re-open
        public List<EmployeeView> AssignedEmpList { get; set; } //hvtam-11112014 assigneedlist

        //hvtam-28022016: khai bao nhung bien o ben ClaimItemView cu sang, nhung bien nao khai bao roi thi comment di
        //public string ClaimID { get; set; }
        public string Insurer { get; set; }
        public string insurerbrandname { get; set; }
        public string Broker { get; set; }

        public string AccountManager { get; set; }

        public string StatusID { get; set; } //hvtam-27092014: Claim Borderaux status
        public string CurrentStatus { get; set; }
        public string FollowUp { get; set; }
        public string Report { get; set; }

        public string OfficeName { get; set; } //hvtam-27092014 

        public string FName { get; set; }//hvtam-27092014

        public string RefStatus { get; set; } //hvtam-22102014: RefStatusID: 1-Open, 2-Closed, 3-ReOpen 

        //hvtam-28022016: Phan  nay da co khai bao o tren roi
        //public int? AccountManagerID { get; set; }
        //public int? FID { get; set; } //hvtam-27092014
        //public bool? Closed { get; set; } //Refstatus
        //public int? CTypeID { get; set; } //hvtam-27092014
        //public int? OfficeID { get; set; } //hvtam-27092014 
        //public bool? Issued { get; set; }
        //public DateTime? DateOfAssignment { get; set; } //hvtam-12102014
        //public DateTime? CreatedDate { get; set; } //hvtam-12102014
        //public int? RefStatusID { get; set; } //hvtam-22102014: RefStatusID: 1-Open, 2-Closed, 3-ReOpen 
        //endhvtam-28022016: Phan  nay da co khai bao o tren roi

        public new int GetHashCode()
        {
            int hashcode = string.IsNullOrWhiteSpace(ID) ? 0 : ID.GetHashCode();
            hashcode ^= (InsurerID.HasValue ? InsurerID.Value.GetHashCode() : 0);
            hashcode ^= (BrokerID.HasValue ? BrokerID.Value.GetHashCode() : 0);
            hashcode ^= (!string.IsNullOrWhiteSpace(TheInsured) ? TheInsured.Trim().GetHashCode() : 0);
            hashcode ^= (!string.IsNullOrWhiteSpace(InsuredProject) ? InsuredProject.Trim().GetHashCode() : 0);
            hashcode ^= (!string.IsNullOrWhiteSpace(RiskLocation) ? RiskLocation.Trim().GetHashCode() : 0);
            hashcode ^= (!string.IsNullOrWhiteSpace(ClientsRef) ? ClientsRef.Trim().GetHashCode() : 0);
            hashcode ^= (!string.IsNullOrWhiteSpace(PolicyNo) ? PolicyNo.Trim().GetHashCode() : 0);
            hashcode ^= (DateOfAssignment.HasValue ? DateOfAssignment.Value.GetHashCode() : 0); //hvtam-02022016 DateOfAssignment
            hashcode ^= (DateOfLoss.HasValue ? DateOfLoss.Value.GetHashCode() : 0);
            hashcode ^= (TypeOfLossID.HasValue ? TypeOfLossID.Value.GetHashCode() : 0);
            hashcode ^= (CauseID.HasValue ? CauseID.Value.GetHashCode() : 0);
            hashcode ^= (!string.IsNullOrWhiteSpace(OtherCause) ? OtherCause.Trim().GetHashCode() : 0);
            hashcode ^= (Estimate.HasValue ? Estimate.Value.GetHashCode() : 0);
            hashcode ^= (!string.IsNullOrWhiteSpace(Currency) ? Currency.Trim().GetHashCode() : 0);
            //hashcode ^= (Reserve.HasValue ? Reserve.Value.GetHashCode() : 0);      //hvtam-20022016
            //hashcode ^= (ProgressPayment.HasValue ? ProgressPayment.Value.GetHashCode() : 0);      //hvtam-20022016
            hashcode ^= (AccountManagerID.HasValue ? AccountManagerID.Value.GetHashCode() : 0);
            hashcode ^= (BordereauxID.HasValue ? BordereauxID.Value.GetHashCode() : 0);
            hashcode ^= (Issued.HasValue ? Issued.Value.GetHashCode() : 0);
            hashcode ^= (IssueDate.HasValue ? IssueDate.Value.GetHashCode() : 0);
            hashcode ^= (ExchangeRate.HasValue ? ExchangeRate.Value.GetHashCode() : 0);
            hashcode ^= (DiscountValue.HasValue ? DiscountValue.Value.GetHashCode() : 0);
            hashcode ^= (!string.IsNullOrWhiteSpace(DiscountType) ? DiscountType.Trim().GetHashCode() : 0);
            hashcode ^= (CTypeID.HasValue ? CTypeID.Value.GetHashCode() : 0); //hvtam-25092014
            hashcode ^= (FID.HasValue ? FID.Value.GetHashCode() : 0); //hvtam-25092014
            hashcode ^= (OfficeID.HasValue ? OfficeID.Value.GetHashCode() : 0); //hvtam-25092014
            hashcode ^= (RefStatusID.HasValue ? RefStatusID.Value.GetHashCode() : 0); //hvtam-22102014
                                                                                      //hashcode ^= (AssignedEmpList.HasValue ? AssignedEmpList.Value.GetHashCode() : 0); hvtam-11112014 

            return hashcode;
        }
    }
}
