using System;
using RayWorkflow.Domain.Shared.Workflow;
using RayWorkflow.Domain.Workflow;
using RayWorkflow.Grains.States;
using RayWorkflow.IGrains;
using Ray.Core;
using Ray.Core.Observer;

namespace RayWorkflow.Grains
{
    using Ray.Core.Event;
    using System.Threading.Tasks;

    [Observer(DefaultObserverGroup.primary, "db", typeof(WorkflowFormGrain))]
    public class WorkflowFormDbGrain : CrudDbGrain<WorkflowFormGrain, WorkflowFormState, Guid, WorkflowForm>, IWorkflowFormDbGrain
    {
        public override Task Process(FullyEvent<Guid> @event)
        {
            return Task.CompletedTask;
        }
    }
}
