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
    [Table("CoOwnerClaim")]
    public class CoOwnerClaim: Entity
    {
        public int CustomerID { get; set; }
        public string ClaimID { get; set; }
    }
}
