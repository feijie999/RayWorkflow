namespace RayWorkflow.EntityFrameworkCore
{
    using Microsoft.EntityFrameworkCore;
    using RayWorkflow.Domain.Workflow;
    using Volo.Abp.EntityFrameworkCore;

    public class RayWorkflowDbContext : AbpDbContext<RayWorkflowDbContext>, IRayWorkflowDbContext
    {
        public RayWorkflowDbContext(DbContextOptions<RayWorkflowDbContext> options)
            : base(options)
        {
        }

        public DbSet<WorkflowForm> Forms { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ConfigureRayWorkflow();
            base.OnModelCreating(builder);
        }
    }
}