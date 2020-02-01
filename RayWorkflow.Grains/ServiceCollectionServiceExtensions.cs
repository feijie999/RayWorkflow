using System;
using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ray.Core.Snapshot;
using RayWorkflow.Domain;

namespace RayWorkflow.Grains
{
    public static class ServiceCollectionServiceExtensions
    {
        /// <summary>
        /// 加载CrudGrain
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="connectionString"></param>
        /// <param name="configAction"></param>
        /// <param name="assemblies">AutoMapper</param>
        /// <returns></returns>
        public static IServiceCollection AddCrudGrain<TDbContext>(this IServiceCollection serviceCollection, string connectionString, Action<IMapperConfigurationExpression> configAction, params Assembly[] assemblies) where TDbContext : DbContext
        {
            serviceCollection.AddTransient(typeof(ICrudHandle<,>), typeof(CrudHandle<,>));
            serviceCollection.AddSingleton(typeof(ISnapshotHandler<,>), typeof(CrudHandle<,>));
            if (configAction == null)
            {
                serviceCollection.AddAutoMapper(assemblies);
            }
            else
            {
                serviceCollection.AddAutoMapper(configAction, assemblies);
            }
            serviceCollection.AddTransient(typeof(IGrainRepository<,>), typeof(GrainEfCoreRepositoryBase<,>));
            serviceCollection.AddDbContext<TDbContext>(
                    options =>
                    {
                        options.UseNpgsql(connectionString);
                    }, ServiceLifetime.Transient);
            serviceCollection.AddTransient<DbContext, TDbContext>();

            return serviceCollection;
        }
    }
}
