using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.EntityFramework
{
    public interface ISqlExecuter
    {
        int Execute(string sql, params object[] parameters);
        Database GetDatabase();
        tlsDbContext GetTLSDBContext();
    }
}
