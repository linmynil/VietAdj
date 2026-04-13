using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    public interface ICauseAppService: IApplicationService
    {
        List<CauseDbObj> GetAll();
        int Create(string name);
        int Delete(int ID);
        
        int Update(int ID, string name);
    }
}
