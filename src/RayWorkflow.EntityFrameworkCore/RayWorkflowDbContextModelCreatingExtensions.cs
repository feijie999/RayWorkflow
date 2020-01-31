namespace RayWorkflow.EntityFrameworkCore
{
    using Microsoft.EntityFrameworkCore;
    using RayWorkflow.Domain.Workflow;
    using System;
    using Volo.Abp;
    using Volo.Abp.EntityFrameworkCore.Modeling;

    public static class RayWorkflowDbContextModelCreatingExtensions
    {
        public static void ConfigureRayWorkflow(
           this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<WorkflowForm>(x =>
            {
                x.ConfigureAudited();
            });
        }
    }
}