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
            Marker("Started");
            
            using (var sqlServer = new SqlServerViaDocker("sqlserver-prepped")) 
            {
                await sqlServer.Start();
                
                Marker("SqlServer ready");

                using (var db = new SqlConnection($"Server=localhost,1433;Database=CamelsRUs;User Id=sa;Password=Wibble123!")) 
                {
                    await db.OpenAsync();
                    
                    Marker("Connected");

                    await db.ExecuteAsync(@"
                        INSERT INTO Camels
                        VALUES 
                            ('Clarence', 'Dromedary', 12),
                            ('Cassandra', 'Bactrian', 10),
                            ('Cuthbert', 'Dromedary', 8)
                      ");
                    
                    Marker("Created");

                    var camels = await db.QueryAsync("SELECT * FROM Camels");

                    Assert.That(camels.Count(), Is.EqualTo(3));
                }
                
                Marker("Done!");
            }
        }
    }
}