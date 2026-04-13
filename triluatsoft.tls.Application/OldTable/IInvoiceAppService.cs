using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.OldTable.dto;
using triluatsoft.tls.OldTable.OldViewClass;

namespace triluatsoft.tls.OldTable
{
    public interface IInvoiceAppService: IApplicationService
    {
        InvoicePagedResultDto<InvoiceView> Search(InvoiceSearchOption options);
        string Create(CreateInvoiceInput input);
        List<InvoiceView> GetAdvanceInvoices(string ClaimID);
        string CreateFinalInvoice(CreateFinalInvoiceInput input);
        InvoiceView GetInfo(int InvoiceID);
        List<InvoiceView> SearchReport(InvoiceSearchOption options);
    }
}
