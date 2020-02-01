using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Ray.Core;
using Ray.EventBus.RabbitMQ;
using Ray.Storage.PostgreSQL;
using Microsoft.Extensions.Hosting;
using RayWorkflow.EntityFrameworkCore;
using RayWorkflow.Grains;

namespace RayWorkflow.Orleans.Host.Test
{
    class Program
    {
        public static string SqlConnection =
            "Server=127.0.0.1;Port=5432;Database=ray_workflow_test;User Id=postgres;Password=123456;maximum pool size=5;";
        public static string RayEventConnection =
            "Server=127.0.0.1;Port=5432;Database=ray_workflow_es_test;User Id=postgres;Password=123456;maximum pool size=5;";


        static Task Main(string[] args)
        {
            InitDb();
            var host = CreateHost();
            return host.RunAsync();
        }

        public static void InitDb()
        {
            var builder = new DbContextOptionsBuilder<RayWorkflowDbContext>();
            builder.UseNpgsql(SqlConnection);
            using (var db = new RayWorkflowDbContext(builder.Options))
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }

        private static IHost CreateHost()
        {
            var builder = new HostBuilder()
                .UseOrleans(
                    (context, siloBuilder) =>
                    {
                        siloBuilder.Configure<ClusterOptions>(options =>
                            {
                                options.ClusterId = "workflow_test";
                                options.ServiceId = "workflow_app";
                            })
                            .UseLocalhostClustering(11116, 30006)
                            .AddRay<RayWorkflowConfig>()
                            .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(WorkflowFormGrain).Assembly).WithReferences());
                    })
                .ConfigureServices((context, serviceCollection) =>
                {
                    serviceCollection.AddCrudGrain<RayWorkflowDbContext>(SqlConnection, null, typeof(GrainDtoMapper).Assembly);
                    //注册postgresql为事件存储库
                    serviceCollection.AddPostgreSQLStorage(config =>
                    {
                        config.ConnectionDict.Add("core_event",
                            RayEventConnection);
                    });
                    serviceCollection.AddPostgreSQLTxStorage(options =>
                    {
                        options.ConnectionKey = "core_event";
                        options.TableName = "Transaction_TemporaryRecord";
                    });
                    serviceCollection.AddRabbitMQ(config =>
                    {
                        config.UserName = "guest";
                        config.Password = "guest";
                        config.Hosts = new[] { "127.0.0.1:5672" };
                        config.VirtualHost = "/test";
                    });
                })
                .ConfigureLogging(logging =>
                {
                    logging.SetMinimumLevel(LogLevel.Information);
                    logging.AddConsole(options => options.IncludeScopes = true);
                });

            var host = builder.Build();
            return host;
//            var builder = new SiloHostBuilder()
//                .Configure<ClusterOptions>(options =>
//                {
//                    options.ClusterId = "unit_test_dev";
//                    options.ServiceId = "workflow_app";
//                })
//                .UseAdoNetClustering(option =>
//                {
//                    option.ConnectionString = OrleansClusterConnection;
//                    option.Invariant = "Npgsql";
//                })
//                .AddRay<RayWorkflowConfig>()
//                .Configure<EndpointOptions>(options =>
//                {
//#if DEBUG
//                    options.GatewayPort = 30001;
//#endif
//                    options.AdvertisedIPAddress = IPAddress.Loopback;
//                })
//                .ConfigureApplicationParts(
//                    parts => parts.AddApplicationPart(typeof(WorkflowFormGrain).Assembly).WithReferences())
//                .ConfigureServices((context, serviceCollection) =>
//                {
//                    serviceCollection.AddCrudGrain<RayWorkflowDbContext>(SqlConnection, null, typeof(GrainDtoMapper).Assembly);
//                    //注册postgresql为事件存储库
//                    serviceCollection.AddPostgreSQLStorage(config =>
//                    {
//                        config.ConnectionDict.Add("core_event",
//                            "Server=127.0.0.1;Port=5432;Database=approval_es_test;User Id=postgres;Password=123456;maximum pool size=20;");
//                    });
//                    serviceCollection.AddPostgreSQLTxStorage(options =>
//                    {
//                        options.ConnectionKey = "core_event";
//                        options.TableName = "Transaction_TemporaryRecord";
//                    });
//                    serviceCollection.AddAutoMapper(WorkFlowDtoMapper.CreateMapping);
//                    serviceCollection.PSQLConfigure();
//                    serviceCollection.AddEntityFrameworkNpgsql().AddDbContext<ApprovalDbContext>(
//                            options => { DbContextOptionsConfigurer.Configure(options, SqlConnection); },
//                            ServiceLifetime.Transient)
//                        .AddAbpRepository();
//                    serviceCollection.AddRabbitMQ(config =>
//                    {
//                        config.UserName = "guest";
//                        config.Password = "guest";
//                        config.Hosts = new[] { "127.0.0.1:5672" };
//                        config.MaxPoolSize = 100;
//                        config.VirtualHost = "/test";
//                    });
//                })
//                .AddIncomingGrainCallFilter<DbContextGrainCallFilter>()
//                .Configure<GrainCollectionOptions>(options => { options.CollectionAge = TimeSpan.FromHours(2); })
//                .ConfigureLogging(logging =>
//                {
//                    logging.SetMinimumLevel(LogLevel.Information);
//                    logging.AddConsole(options => options.IncludeScopes = true);
//                });

//            var host = builder.Build();
//            return host;
        }
    }
}
