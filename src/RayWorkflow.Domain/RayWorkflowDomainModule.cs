using System;

namespace RayWorkflow.Domain
{
    using RayWorkflow.Domain.Shared;
    using Volo.Abp.Modularity;

    [DependsOn(typeof(RayWorkflowDomainSharedModule))]
    public class RayWorkflowDomainModule : AbpModule
    {
    }
}
