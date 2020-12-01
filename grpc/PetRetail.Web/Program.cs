using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace PetRetail.Web
{
    internal class Program
    {
        static Task Main(string[] args)
            => Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseStartup<Startup>();
                })
                .Build()
                .RunAsync();
    }
}