using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldTable
{
    public interface IFirstRefAppService : IApplicationService
    {
        List<FirstRef> GetAll(int officeID);
        int Create(int officeID, string syear, int initialCode);                
        int SetActive(FirstRef firstRefItem);
    }
}
