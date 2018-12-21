using System;
using System.Threading;
using System.Threading.Tasks;

using CoherentSolutions.Extensions.Configuration.AnyWhere;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Code
{
    public class ConfigurationPrinter : IHostedService
    {
        private readonly IConfiguration config;

        public ConfigurationPrinter(
            IConfiguration config)
        {
            this.config = config;
        }

        public Task StartAsync(
            CancellationToken cancellationToken)
        {
            foreach (var kv in this.config.AsEnumerable())
            {
                Console.WriteLine($"{kv.Key} = {kv.Value}");
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            new HostBuilder()
               .ConfigureAppConfiguration(
                    config =>
                    {
                        config.AddAnyWhereConfiguration();
                    })
               .ConfigureServices(
                    services =>
                    {
                        services.AddTransient<IHostedService, ConfigurationPrinter>();
                    })
               .UseConsoleLifetime()
               .Build()
               .Run();
        }
    }
}
