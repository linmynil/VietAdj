using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable.dto
{
    public class CustomPagedResultDto<T> : PagedResultDto<T>
    {

        //
        // Summary:
        //     Sum some value of Items.
        public decimal SumValue { get; set; }
    }
    public class InvoicePagedResultDto<T> : PagedResultDto<T>
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalNetCost { get; set; }
        public decimal TotalExpense { get; set; }
    }
    public class ARPagedResultDto<T> : PagedResultDto<T>
    {
        public decimal TotalTax { get; set; }
        public decimal SumValue { get; set; }
        public decimal SubTotal { get; set; }
    }
}
