using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RayWorkflow.EntityFrameworkCore
{
    public class RayWorkflowMigrationsDbContextFactory : IDesignTimeDbContextFactory<RayWorkflowDbContext>
    {
        public RayWorkflowDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<RayWorkflowDbContext>().UseNpgsql("host=localhost;port=5432;database=ray_workflow;userid=datum;pwd=123456;");
            return new RayWorkflowDbContext(builder.Options);
        }
    }
}
