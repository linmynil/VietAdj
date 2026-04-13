using EntityFramework.DynamicFilters;
using triluatsoft.tls.EntityFramework;

namespace triluatsoft.tls.Tests.TestDatas
{
    public class TestDataBuilder
    {
        private readonly tlsDbContext _context;
        private readonly int _tenantId;

        public TestDataBuilder(tlsDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new TestOrganizationUnitsBuilder(_context, _tenantId).Create();

            _context.SaveChanges();
        }
    }
}
