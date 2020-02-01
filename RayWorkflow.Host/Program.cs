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
using Microsoft.Extensions.DependencyInjection;
using Ray.EventBus.RabbitMQ;
using RayWorkflow.EntityFrameworkCore;

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
                    (context, siloBuilder) =>
                    {
                        siloBuilder.Configure<ClusterOptions>(Configuration.GetSection("ClusterOptions"))
                            .UseAdoNetClustering(options =>
                            {
                                options.ConnectionString = Configuration["ConnectionStrings:OrleansCluster"];
                                options.Invariant = "Npgsql";
                            })
                            .UseDashboard()
                            .AddRay<RayWorkflowConfig>()
                            .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(WorkflowFormGrain).Assembly).WithReferences());
                    })
                .ConfigureServices((context, serviceCollection) =>
                {
                    serviceCollection.AddCrudGrain<RayWorkflowDbContext>(Configuration.GetConnectionString("RayWorkflow"), null, typeof(GrainDtoMapper).Assembly);
                    //注册postgresql为事件存储库
                    serviceCollection.AddPostgreSQLStorage(config =>
                    {
                        config.ConnectionDict.Add("core_event",
                            Configuration.GetConnectionString("Event"));
                    });
                    serviceCollection.AddPostgreSQLTxStorage(options =>
                    {
                        options.ConnectionKey = "core_event";
                        options.TableName = "Transaction_TemporaryRecord";
                    });
                    serviceCollection.Configure<RabbitOptions>(Configuration.GetSection("RabbitConfig"));
                    serviceCollection.AddRabbitMQ(_ => { });
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
