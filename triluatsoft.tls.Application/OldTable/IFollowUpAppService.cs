using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.OldTable.dto;

namespace triluatsoft.tls.OldTable
{
    public interface IFollowUpAppService : IApplicationService
    {
        List<FollowUpDBObj> GetAll();
        int Delete(int ID);
        int CreateOrUpdate(CreateOrUpdateFollowUpInput input);
    }
}
