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
    [Table("tblOfficeClaimCode")]
    public class FirstRef : Entity
    {
        public FirstRef()
        {
            //this.Claims = new HashSet<Claim>();
        }       

        public int OfficeID { get; set; }
        public string SYear { get; set; }
        public int InitialCode { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> LastID { get; set; }        
    }
}
