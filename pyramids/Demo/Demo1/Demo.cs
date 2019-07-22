using System.Data.Common;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Demo
{
    public class Demo
    {
        [Test]
        public async Task Blah() 
        {
            using (var sqlServer = new SqlServerViaDocker()) 
            {
                await sqlServer.Start();


//            ConnectionString =
//                $"Server=localhost,1433;Database=default;User Id=sa;Password=Wibble123!";

            }
        }
    }
}