using Linovative.Frontend.Services;
using Linovative.Frontend.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Linovative.Frontent.WebBlazor.Extensions
{
    public static class ServicesExtensions
    {
        public static void RegisterAllServices(this WebAssemblyHostBuilder builder)
        {
            var frontEndServices = typeof(RegisterAllServices).Assembly;
            var frontEndBlazorServices = typeof(RegisterAllWebBlazorService).Assembly;
            var frontendShare = typeof(RegisterShared).Assembly;

            builder.Services.Scan(scan => scan
                .FromAssemblies(frontEndServices, frontEndBlazorServices, frontendShare)
                .AddClasses(c => c.Where(t => t.Name.EndsWith("Service")))
                .AsImplementedInterfaces()
                .AsSelf()
                .WithScopedLifetime());
        }
    }
}
