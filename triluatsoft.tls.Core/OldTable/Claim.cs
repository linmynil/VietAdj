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
    [Table("Claim")]
    public class Claim : Entity<string>
    {        
        public Nullable<int> InsurerID { get; set; }
        public Nullable<int> BrokerID { get; set; }
        public Nullable<int> FID { get; set; }
        public string TheInsured { get; set; }
        public string InsuredProject { get; set; }
        public string RiskLocation { get; set; }
        public string ClientsRef { get; set; }
        public string PolicyNo { get; set; }
        public Nullable<System.DateTime> DateOfLoss { get; set; }
        public Nullable<int> TypeOfLossID { get; set; }
        public Nullable<int> CauseID { get; set; }
        public string OtherCause { get; set; }
        public Nullable<decimal> Estimate { get; set; }
        public string Currency { get; set; }
        public string Reserve { get; set; }
        public Nullable<int> AccountManagerID { get; set; }
        public Nullable<int> BordereauxID { get; set; }
        public Nullable<bool> Issued { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public Nullable<bool> Closed { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<int> CtypeID { get; set; }
        public Nullable<int> OfficeID { get; set; }
        public Nullable<decimal> TotalAdvAmount { get; set; }
        public Nullable<int> RefStatusID { get; set; }
        public Nullable<System.DateTime> DateOfAssignment { get; set; }
        public virtual Cause Cause { get; set; }
        //public virtual Employee Employee { get; set; }
        //public virtual Customer Customer { get; set; }
    }
}
