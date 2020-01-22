using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Ray.Core;
using Ray.Storage.PostgreSQL;
using RayWorkflow.Grains;

namespace RayWorkflow.Host
{
    class Program
    {
        public static IConfigurationRoot Configuration;
        static Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            var host = CreateHost();
            return host.RunAsync();
        }

        private static IHost CreateHost()
        {
            var builder = new HostBuilder()
                .UseOrleans(
                    (Microsoft.Extensions.Hosting.HostBuilderContext context, ISiloBuilder siloBuilder) =>
                    {
                        siloBuilder.Configure<ClusterOptions>(Configuration.GetSection("ClusterOptions"))
                            .UseLocalhostClustering(11115, 30005)
                            .UseDashboard()
                            .AddRay<RayWorkflowConfig>()
                            .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(WorkflowFormGrain).Assembly).WithReferences());
                    })
                .ConfigureServices((context, serviceCollection) =>
                {
                    //注册postgresql为事件存储库
                    serviceCollection.AddPostgreSQLStorage(config =>
                    {
                        config.ConnectionDict.Add("core_event",
                            Configuration.GetConnectionString("EventConnection"));
                    });
                    serviceCollection.AddPostgreSQLTxStorage(options =>
                    {
                        options.ConnectionKey = "core_event";
                        options.TableName = "Transaction_TemporaryRecord";
                    });
                })
                .ConfigureLogging(logging =>
                {
                    logging.SetMinimumLevel(LogLevel.Information);
                    logging.AddConsole(options => options.IncludeScopes = true);
                });

            var host = builder.Build();
            return host;
        }
    }
}
