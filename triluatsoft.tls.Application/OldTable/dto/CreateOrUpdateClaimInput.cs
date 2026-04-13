using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class CreateOrUpdateClaimInput
    {
        public string ClaimId { get; set; }
        public int OfficeID { get; set; }
        public DateTime? DateOfAssignment { get; set; }

        public int? InsurerID { get; set; }

        public int? BrokerID { get; set; }

        public DateTime? DateOfLoss { get; set; }

        public string TheInsured { get; set; }

        public int? CauseID { get; set; }

        public int? TypeOfLossId { get; set; }

        public string OtherCause { get; set; }

        public string InsuredProject { get; set; }

        public int? CtypeID { get; set; }

        public string Estimate { get; set; }

        public string Currency { get; set; }

        public string PolicyNo { get; set; }

        public string ClientsRef { get; set; }

        public string RiskLocation { get; set; }

        public List<int> ListCoOwner { get; set; }

        public List<int> ListAE { get; set; }

        public int AccountManagerID { get; set; }
        public Nullable<int> RefStatusID { get; set; }

    }
}
