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
    [Table("tblFileUpload")]
    public class FileUpload : Entity<int>
    {
        public FileUpload()
        {
            
        }        
        public string FName { get; set; }
        public string FPath { get; set; }                
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Catagory { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
