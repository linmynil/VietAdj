using Abp.Dependency;
using Abp.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using triluatsoft.tls.EntityFramework;

namespace triluatsoft.tls.EntityFramework
{
    public class SqlExecuter : ISqlExecuter, ITransientDependency
    {
        private readonly IDbContextProvider<tlsDbContext> _dbContextProvider;

        public SqlExecuter(IDbContextProvider<tlsDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
            //_dbContextProvider.GetDbContext().Database.CommandTimeout = 600;
        }

        public int Execute(string sql, params object[] parameters)
        {
            _dbContextProvider.GetDbContext().Database.CommandTimeout = 3000;
            return _dbContextProvider.GetDbContext().Database.ExecuteSqlCommand(sql, parameters);
        }
        public Database GetDatabase()
        {
            _dbContextProvider.GetDbContext().Database.CommandTimeout = 3000;
            return _dbContextProvider.GetDbContext().Database;
        }

        public tlsDbContext GetTLSDBContext()
        {
            _dbContextProvider.GetDbContext().Database.CommandTimeout = 3000;
            return _dbContextProvider.GetDbContext();
        }
    }
}
