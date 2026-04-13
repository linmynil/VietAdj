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

    [Table("ProfessionalFee")]
    public class ProfessionalFee : Entity
    {
        [Key]
        [Column("ProfessionalFeeID")]
        public override int Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                base.Id = value;
            }
        }

        public Nullable<int> TimeSheetID { get; set; }
        public Nullable<System.DateTime> InputDate { get; set; }
        public Nullable<int> JobCodeID { get; set; }
        public string Notes { get; set; }
        public Nullable<decimal> WorkingHour { get; set; }
        public Nullable<decimal> ApprovedHour { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> ChargedBy { get; set; }
        public Nullable<decimal> FeePerHour { get; set; }
        public Nullable<System.TimeSpan> WorkTime { get; set; }
        public Nullable<System.TimeSpan> ApproveTime { get; set; }
        public Nullable<decimal> ProFeeValue { get; set; }
        public Nullable<decimal> ChargedFeePerHour { get; set; }
        public Nullable<decimal> ChargedProFeeValue { get; set; }
    }
}
