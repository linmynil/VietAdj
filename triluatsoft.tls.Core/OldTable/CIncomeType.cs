using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    [Table("CIncomeType")]
    public class CIncomeType:Entity
    {
        public CIncomeType()
        {
            this.EmployeeIncomes = new HashSet<EmployeeIncome>();
        }
        public string IncomeName { get; set; }

        public virtual ICollection<EmployeeIncome> EmployeeIncomes { get; set; }
    }
}
