using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.OldTable.dto;
using triluatsoft.tls.OldTable.View;

namespace triluatsoft.tls.OldTable
{
    public interface ICashAppService : IApplicationService
    {
        CustomPagedResultDto<CashView> Search(CashSearchOptions opts);
        List<CashBorderauxView> GetHistories(int cashID);
        string SaveCash(CreateOrUpdateCashHistoryInput input);
        CashView GetById(int cashId);
        string DeleteCash(int cashId);
    }
}
