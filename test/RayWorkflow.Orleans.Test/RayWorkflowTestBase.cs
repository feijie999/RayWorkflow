using System;
using Orleans;
using Orleans.Runtime;
using Microsoft.EntityFrameworkCore;
using RayWorkflow.EntityFrameworkCore;
using RayWorkflow.IGrains;
using System.Threading.Tasks;

namespace RayWorkflow.Orleans.Test
{
    public class RayWorkflowTestBase
    {
        protected readonly IClusterClient ClusterClient;
        public static string SqlConnection =
            "Server=127.0.0.1;Port=5432;Database=ray_workflow_test;User Id=postgres;Password=123456;maximum pool size=5;";

        public RayWorkflowTestBase()
        {
            ClusterClient = StartClientWithRetries().GetAwaiter().GetResult();
        }
        public static async Task<IClusterClient> StartClientWithRetries(int initializeAttemptsBeforeFailing = 5)
        {
            var attempt = 0;
            IClusterClient client;
            while (true)
            {
                try
                {
                    var builder = new ClientBuilder()
                        .UseLocalhostClustering(30006)
                        .ConfigureApplicationParts(parts =>
                            parts.AddApplicationPart(typeof(IWorkflowFormGrain<>).Assembly).WithReferences());
                    client = builder.Build();
                    await client.Connect();
                    Console.WriteLine("Client successfully connect to silo host");
                    break;
                }
                catch (SiloUnavailableException)
                {
                    attempt++;
                    Console.WriteLine(
                        $"Attempt {attempt} of {initializeAttemptsBeforeFailing} failed to initialize the Orleans client.");
                    if (attempt > initializeAttemptsBeforeFailing)
                    {
                        throw;
                    }

                    await Task.Delay(TimeSpan.FromSeconds(4));
                }
            }

            return client;
        }

        protected virtual void UsingDbContext(Action<RayWorkflowDbContext> action)
        {
            using (var context = CreateDbContext())
            {
                action(context);
                context.SaveChanges();
            }
        }

        protected virtual T UsingDbContext<T>(Func<RayWorkflowDbContext, T> func)
        {
            T result;

            using (var context = CreateDbContext())
            {
                result = func(context);
                context.SaveChanges();
            }

            return result;
        }

        protected virtual async Task UsingDbContextAsync(Func<RayWorkflowDbContext, Task> action)
        {
            using (var context = CreateDbContext())
            {
                await action(context);
                await context.SaveChangesAsync(true);
            }
        }

        protected virtual async Task<T> UsingDbContextAsync<T>(Func<RayWorkflowDbContext, Task<T>> func)
        {
            T result;

            using (var context = CreateDbContext())
            {
                result = await func(context);
                context.SaveChanges();
            }

            return result;
        }

        protected RayWorkflowDbContext CreateDbContext()
        {
            var builder = new DbContextOptionsBuilder<RayWorkflowDbContext>();
            builder.UseNpgsql(SqlConnection);
            return new RayWorkflowDbContext(builder.Options);
        }
    }
}
