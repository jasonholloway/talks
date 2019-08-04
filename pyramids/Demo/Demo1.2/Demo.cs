using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Dapper;
using NUnit.Framework;
using static Common.Markers;

namespace Demo
{
    public class Demo
    {
        [Test]
        public async Task Blah() 
        {
            using (var sqlServer = new SqlServerViaDocker("sqlserver-prepped")) 
            {
                await sqlServer.Start();

                using (var db = new SqlConnection($"Server=localhost,1433;User Id=sa;Password=Wibble123!")) 
                {
                    await db.OpenAsync();

                    await db.ExecuteAsync(@"
                        INSERT INTO Camels
                        VALUES 
                            ('Clarence', 12),
                            ('Cassandra', 10),
                            ('Cuthbert', 8)
                      ");

                    var camels = await db.QueryAsync("SELECT * FROM Camels");

                    Assert.That(camels.Count(), Is.EqualTo(3));
                }
            }
        }
    }
}