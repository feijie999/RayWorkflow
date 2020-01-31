using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RayWorkflow.Domain.Workflow;

namespace RayWorkflow.EntityFrameworkCore
{

    [ConnectionStringName("RayWorkflow")]
    public interface IRayWorkflowDbContext : IEfCoreDbContext
    {
        DbSet<WorkflowForm> Forms { get; }
    }
}