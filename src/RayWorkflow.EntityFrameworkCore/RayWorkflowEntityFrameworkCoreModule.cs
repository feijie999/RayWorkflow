using Microsoft.Extensions.DependencyInjection;
using RayWorkflow.Domain;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace RayWorkflow.EntityFrameworkCore
{
    [DependsOn(
        typeof(RayWorkflowDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class RayWorkflowEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<RayWorkflowDbContext>(options => { options.AddDefaultRepositories(true); });
        }
    }
}