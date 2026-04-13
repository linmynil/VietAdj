using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;

namespace triluatsoft.tls.EntityFramework
{
    public class AbpZeroDbMigrator : AbpZeroDbMigrator<tlsDbContext, Migrations.Configuration>
    {
        public AbpZeroDbMigrator(
            IUnitOfWorkManager unitOfWorkManager,
            IDbPerTenantConnectionStringResolver connectionStringResolver,
            IIocResolver iocResolver) :
            base(
                unitOfWorkManager,
                connectionStringResolver,
                iocResolver)
        {

        }
    }
}
