using RayWorkflow.Domain.Shared.Workflow;
using RayWorkflow.Domain.Workflow;
using RayWorkflow.IGrains;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RayWorkflow.Orleans.Test.Crud
{
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

        [Fact]
        public async Task CanRead()
        {
            var entity = new WorkflowForm(Guid.NewGuid()) { Name = "Name2" };
            UsingDbContext(dbContext => { dbContext.Set<WorkflowForm>().Add(entity); });
            var grain = ClusterClient.GetGrain<IWorkflowFormGrain<WorkflowFormDto>>(entity.Id);
            var result = await grain.Get();
            result.Name.ShouldBe(entity.Name);
        }

        [Fact]
        public async Task CanUpdated()
        {
            var entity = new WorkflowForm(Guid.NewGuid()) { Name = "Name2" };
            UsingDbContext(dbContext => { dbContext.Set<WorkflowForm>().Add(entity); });
            var grain = ClusterClient.GetGrain<IWorkflowFormGrain<WorkflowFormDto>>(entity.Id);
            var result = await grain.Get();
            result.Name.ShouldBe(entity.Name);
            await grain.Update(new WorkflowFormDto()
                         {
                             Id = entity.Id,
                             Name = "Name3"
                         });
            var updateResult = await grain.Get();
            updateResult.Name.ShouldBe("Name3");
            await Task.Delay(500);
            UsingDbContext(dbContext =>
            {
                dbContext.Set<WorkflowForm>().Any(x => x.Id == entity.Id).ShouldBeTrue();
                var workflowForm = dbContext.Set<WorkflowForm>().First(x => x.Id == entity.Id);
                workflowForm.Name.ShouldBe("Name3");
            });
        }

        [Fact]
        public async Task CanDeleted()
        {
            var entity = new WorkflowForm(Guid.NewGuid()) { Name = "Name4" };
            UsingDbContext(dbContext => { dbContext.Set<WorkflowForm>().Add(entity); });
            var grain = ClusterClient.GetGrain<IWorkflowFormGrain<WorkflowFormDto>>(entity.Id);
            var result = await grain.Get();
            result.Name.ShouldBe(entity.Name);
            await grain.Delete();
            await grain.Over();
            var updateResult = await grain.Get();
            updateResult.Id.ShouldBe(default);
            await Task.Delay(500);
            UsingDbContext(dbContext =>
            {
                dbContext.Set<WorkflowForm>().Any(x => x.Id == entity.Id).ShouldBeFalse();
            });
        }
    }
}