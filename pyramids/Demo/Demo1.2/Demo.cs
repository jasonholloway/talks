using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
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