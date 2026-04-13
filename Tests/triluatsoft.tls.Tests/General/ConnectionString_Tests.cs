using System.Data.SqlClient;
using Shouldly;
using Xunit;

namespace triluatsoft.tls.Tests.General
{
    public class ConnectionString_Tests
    {
        [Fact]
        public void SqlConnectionStringBuilder_Test()
        {
            var csb = new SqlConnectionStringBuilder("Server=localhost; Database=tls; Trusted_Connection=True;");
            csb["Database"].ShouldBe("tls");
        }
    }
}
