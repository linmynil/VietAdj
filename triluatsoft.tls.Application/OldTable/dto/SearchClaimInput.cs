using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.Dto;

namespace triluatsoft.tls.OldTable.dto
{
    public class SearchClaimInput : PaginationInputBase
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public int? InsurerId { get; set; } = -1;
        public int? AccountManagerId { get; set; } = -1;
        public int? OfficeId { get; set; } = -1;
        public string ClaimTypeCode { get; set; }
        public int? RefStatusId { get; set; } = -1;
        public string RefStatusName { get; set; }
        public string ClaimRefId { get; set; }
        public int? CauseId { get; set; } = -1;
        public int? TypeOfLossId { get; set; } = -1;
        public string PolicyNo { get; set; }
        public string TheInsured { get; set; }
        public int? BrokerId { get; set; } = -1;
        public string SurName { get; set; }
    }
}
