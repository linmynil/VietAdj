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
    public interface IPaymentAppService : IApplicationService
    {
        PagedResultDto<CPaymentView> Search(ReceivementSearchOption opt);
        string Create(CreatePaymentInput input);
    }
}
