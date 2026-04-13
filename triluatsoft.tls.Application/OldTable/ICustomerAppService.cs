using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.OldTable.dto;

namespace triluatsoft.tls.OldTable
{
    public interface ICustomerAppService : IApplicationService
    {
        List<CustomerDBObj> GetAll();
        List<CustomerDBObj> GetByType(string type);
        List<CustomerDBObj> Search(SearchCustomerInput input);
        int CreateOrUpdate(CreateOrUpdateCustomerInput input);
        CustomerDBObj GetById(int Id);
        List<CustomerView> GetLstCustomerByClaimID(string claimID);
        CustomerView GetInfo(int CustomerID);
    }
}
