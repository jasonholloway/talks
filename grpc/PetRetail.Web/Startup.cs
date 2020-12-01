using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PetRetail.Services;

namespace PetRetail.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddSingleton<KittenBasket>();
            services.AddSingleton<ParrotProvider>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<PetShopService>();
            });
        }
    }
}