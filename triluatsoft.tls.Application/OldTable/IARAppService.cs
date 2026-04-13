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
    public interface IARAppService : IApplicationService
    {
        ARPagedResultDto<ARView> Search(ARSearchOption options);       
        List<ARView>SearchReport(InvoiceSearchOption options);

    }
}
