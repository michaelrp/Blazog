using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Blazog.Services;

namespace Blazog
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

            builder.Services.AddScoped<IDataLoader, DataLoader>();
            builder.Services.AddSingleton<ILocalStorage, LocalStorage>();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            var host = builder.Build();

            var dataLoader = host.Services.GetRequiredService<IDataLoader>();
            await dataLoader.InitializeBlazogAppAsync();

            await host.RunAsync();
        }
    }
}
