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
    [Table("tblFolder")]
    public class F : Entity<int>
    {
        public F()
        {
            //this.Claims = new HashSet<Claim>();
        }
        [Key]
        [Column("ID")]
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

        public string FName { get; set; }
        public string FPath { get; set; }
        public string FSecurity { get; set; }
        public Nullable<bool> isUpload { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }

        public virtual ICollection<Claim> Claims { get; set; }
    }
}
