using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.OldTable.dto;
using triluatsoft.tls.OldTable.OldViewClass;

namespace triluatsoft.tls.OldTable
{
    public interface IBorderauxStatusAppService: IApplicationService
    {
        List<BordereauxStatusDBObj> GetAll();
        int Delete(string ID);
        int CreateOrUpdate(CreateOrUpdateBorderauxStatusInput input);
        List<BordereauxStatusView> GetActiveList();
    }
}
