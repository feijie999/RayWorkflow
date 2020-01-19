using System;

namespace RayWorkflow.Domain.Shared
{
    using Volo.Abp.Domain;
    using Volo.Abp.Modularity;

    [DependsOn(typeof(AbpDddDomainModule))]
    public class RayWorkflowDomainSharedModule : AbpModule
    {
    }
}
