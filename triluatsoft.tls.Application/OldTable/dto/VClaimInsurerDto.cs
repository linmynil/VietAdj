using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class VClaimInsurerDto
    {
        public string ID { get; set; }
        public int? InsurerID { get; set; }
        public int? BrokerID { get; set; }
        public string BrokerName { get; set; }
        public string InsurerName { get; set; }
        public int? AccountManagerID { get; set; }
        public string AccManName { get; set; }
        public int? OfficeID { get; set; }
        public string OfficeName { get; set; }
        public string ClaimTypeCode { get; set; }
        public int? CtypeID { get; set; }
        public DateTime? DateOfAssignment { get; set; }
        public DateTime? DateOfLoss { get; set; }
        public int? RefStatusID { get; set; }
        public string RefStatusName { get; set; }
        public string Reserve { get; set; }
        public int? CauseID { get; set; }
        public string CauseName { get; set; }
        public int? TypeOfLossID { get; set; }
        public string LossName { get; set; }
        public string TheInsured { get; set; }

        public string PolicyNo { get; set; }

        public decimal? Estimate { get; set; }
        public string Currency { get; set; }
        public string OtherCause { get; set; }
        public string InsuredProject { get; set; }
        public string RiskLocation { get; set; }

        public string ClientsRef { get; set; }

        public List<CoOwner> CoOwners { get; set; }
        public List<CoOwner> AvaiCoOwners { get; set; }
        public List<AccExec> AEs { get; set; }
        public List<AccExec> AvaiAEs { get; set; }
    }

    public class CoOwner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            if (this.Id == ((CoOwner)obj).Id)
            {
                return true;
            }
            return base.Equals(obj);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        
    }
    public class AccExec
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        // override object.Equals
        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (this.EmployeeId == ((AccExec)obj).EmployeeId)
            {
                return true;
            }
            return base.Equals(obj);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
