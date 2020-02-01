namespace RayWorkflow.Orleans.Test.Crud
{
    using RayWorkflow.Domain.Shared.Workflow;
    using RayWorkflow.Domain.Workflow;
    using RayWorkflow.IGrains;
    using Shouldly;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class CrudGrainTests : RayWorkflowTestBase
    {
        [Fact]
        public async Task CanCreated()
        {
            var id = Guid.NewGuid();
            var grain = ClusterClient.GetGrain<IWorkflowFormGrain<WorkflowFormDto>>(id);
            var dto = new WorkflowFormDto { Id = id, Name = "Name1", Sort = 1 };
            await grain.Create(dto);
            var result = await grain.Get();
            result.Name.ShouldBe(dto.Name);
            await Task.Delay(500);
            UsingDbContext(dbContext =>
            {
                dbContext.Set<WorkflowForm>().Any(x => x.Id == id).ShouldBeTrue();
                var workflowForm = dbContext.Set<WorkflowForm>().First(x => x.Id == id);
                workflowForm.Name.ShouldBe(dto.Name);
            });
        }

    }
}