namespace triluatsoft.tls.EntityFramework.OldTables
{
    using Abp.Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Claim")]
    public partial class Claim : Entity
    {
        [Key]
        [StringLength(25)]
        public string ID { get; set; }

        public int? InsurerID { get; set; }

        public int? BrokerID { get; set; }

        public int? FID { get; set; }

        [StringLength(100)]
        public string TheInsured { get; set; }

        [StringLength(100)]
        public string InsuredProject { get; set; }

        [StringLength(100)]
        public string RiskLocation { get; set; }

        [StringLength(100)]
        public string ClientsRef { get; set; }

        [StringLength(100)]
        public string PolicyNo { get; set; }

        public DateTime? DateOfLoss { get; set; }

        public int? TypeOfLossID { get; set; }

        public int? CauseID { get; set; }

        [StringLength(100)]
        public string OtherCause { get; set; }

        public decimal? Estimate { get; set; }

        [StringLength(3)]
        public string Currency { get; set; }

        [StringLength(100)]
        public string Reserve { get; set; }

        public int? AccountManagerID { get; set; }

        public int? BordereauxID { get; set; }

        public bool? Issued { get; set; }

        public DateTime? IssueDate { get; set; }

        public decimal? ExchangeRate { get; set; }

        public bool? Closed { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public int? CtypeID { get; set; }

        public int? OfficeID { get; set; }

        public decimal? TotalAdvAmount { get; set; }

        public int? RefStatusID { get; set; }

        public DateTime? DateOfAssignment { get; set; }
    }
}
