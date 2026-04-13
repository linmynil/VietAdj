using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.EntityFramework;

namespace triluatsoft.tls.OldTable
{
    public class OfficeAppService : tlsAppServiceBase, IOfficeAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        public OfficeAppService(ISqlExecuter sqlExecuter)
        {
            _sqlExecuter = sqlExecuter;
        }
        public List<Office> GetAll()
        {
            List<Office> office = _sqlExecuter.GetDatabase().SqlQuery<Office>("Select * from tblOffice").OrderBy(x => x.Name).ToList();
            return office;
        }        
    }
}
