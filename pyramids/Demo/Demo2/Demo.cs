using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Dapper;
using NUnit.Framework;
using static Common.Markers;

namespace Demo2
{
    public class Demo
    {
        [Test]
        public async Task Normal() 
        {
            Marker("Starting");
            
            using (var sqlServer = new SqlServerViaDocker("sqlservr-tracking")) 
            {
                await sqlServer.Start();
                Marker("Started");

                using (var db = new SqlConnection($"Server=localhost,1433;Database=electio_tracking;User Id=sa;Password=Wibble123!")) 
                {
                    await db.OpenAsync();
                    Marker("Opened");

                    var consignmentId = await db.CreateConsignment();
                    var carrierConsignmentId = await db.CreateCarrierConsignment(consignmentId);
                    var carrierPackageId = await db.CreateCarrierPackage(carrierConsignmentId, packageRefPerLegMpd: 12345);

                    var packages = await db.QueryAsync("SELECT * FROM dbo.CarrierPackages");

                    Assert.That(packages.Count(), Is.EqualTo(1));
                }

                Marker("Finished");
            }
        }
        
        [Test]
        public async Task ViaCRIU() 
        {
            Marker("Starting");
            
            using (var sqlServer = new SqlServerViaDockerCRIU("sqlservr-tracking")) 
            {
                await sqlServer.Start();
                Marker("Started");

                await Task.Delay(300);
                
                using (var db = new SqlConnection($"Server=localhost,1433;Database=electio_tracking;User Id=sa;Password=Wibble123!")) 
                {
                    await db.OpenAsync();
                    Marker("Opened");

                    var consignmentId = await db.CreateConsignment();
                    var carrierConsignmentId = await db.CreateCarrierConsignment(consignmentId);
                    var carrierPackageId = await db.CreateCarrierPackage(carrierConsignmentId, packageRefPerLegMpd: 12345);

                    var packages = await db.QueryAsync("SELECT * FROM dbo.CarrierPackages");

                    Assert.That(packages.Count(), Is.EqualTo(1));
                }
                
                Marker("Finished");
            }
        }
    }
}