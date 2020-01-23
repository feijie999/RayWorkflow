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
        public static IServiceCollection AddCrudGrain<TDbContext>(this IServiceCollection serviceCollection, string connetionString, Action<IMapperConfigurationExpression> configAction) where TDbContext : DbContext
        {
            serviceCollection.AddTransient(typeof(ICrudHandle<,>), typeof(CrudHandle<,>));
            serviceCollection.AddSingleton(typeof(ISnapshotHandler<,>), typeof(CrudHandle<,>));
            serviceCollection.AddAutoMapper(configAction, new Assembly[0]);
            serviceCollection.AddTransient(typeof(IGrainRepository<,>), typeof(GrainEfCoreRepositoryBase<,>));
            serviceCollection.AddDbContext<TDbContext>(
                    options =>
                    {
                        options.UseNpgsql(connetionString);
                    }, ServiceLifetime.Transient);

            return serviceCollection;
        }
    }
}
