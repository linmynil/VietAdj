using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.EntityFramework;

namespace triluatsoft.tls.OldTable
{
    public class FirstRefAppService : tlsAppServiceBase, IFirstRefAppService
    {
        private readonly ISqlExecuter _sqlExecuter;
        public FirstRefAppService(ISqlExecuter sqlExecuter)
        {
            _sqlExecuter = sqlExecuter;
        }
        public List<FirstRef> GetAll(int officeID)
        {
            List<FirstRef> firstref = _sqlExecuter.GetDatabase().SqlQuery<FirstRef>("Select * from tblOfficeClaimCode WHERE OfficeID = " + officeID.ToString()).OrderBy(x => x.SYear).ToList();
            return firstref;
        }

        public int Create(int officeID, string syear, int initialCode)
        {
            return _sqlExecuter.GetDatabase().ExecuteSqlCommand("INSERT INTO tblOfficeClaimCode (OfficeID, SYear, InitialCode, IsActive) VALUES(@p0, @p1, @p2, 0)", officeID, syear, initialCode);
        }
        
        public int SetActive(FirstRef firstRefItem)
        {
            var deac = _sqlExecuter.GetDatabase().ExecuteSqlCommand("UPDATE tblOfficeClaimCode SET IsActive=0 WHERE OfficeID = @p0", firstRefItem.OfficeID);
            var act = _sqlExecuter.GetDatabase().ExecuteSqlCommand("UPDATE tblOfficeClaimCode SET IsActive=1 WHERE OfficeID = @p0 AND SYear = @p1 AND InitialCode = @p2", firstRefItem.OfficeID, firstRefItem.SYear, firstRefItem.InitialCode);
            return deac + act;
        }
    }
}
