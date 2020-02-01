using Microsoft.EntityFrameworkCore;
using RayWorkflow.Domain.Workflow;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace RayWorkflow.EntityFrameworkCore
{
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