using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.OldTable.dto;

namespace triluatsoft.tls.OldTable
{
    public interface ITypeOfLossAppService: IApplicationService
    {
        List<TypeOfLoss> GetAll();
        int Create(string name);
        int Delete(int ID);
        
        int Update(int ID, string name);
        int CreateOrUpdate(CreateOrUpdateTypeOfLossInput input);
    }
}
