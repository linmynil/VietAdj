using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.OldTable.dto;

namespace triluatsoft.tls.OldTable
{
    public interface IContributionAdjustmentAppService: IApplicationService
    {
        PagedResultDto<ContributionAdjustmentSearchOptions> GetAll(ContributionSearchOption option);
        string Save(CreateOrUpdateContributionAdjustmentInput input);
        ContributionAdjustmentSearchOptions GetById(int taskId);
        string DeleteById(int taskId);
    }
}
