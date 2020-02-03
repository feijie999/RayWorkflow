using System;
using RayWorkflow.Domain.Shared.Workflow;
using RayWorkflow.Domain.Workflow;
using RayWorkflow.Grains.States;
using RayWorkflow.IGrains;
using Ray.EventBus.RabbitMQ;
using RayWorkflow.IGrains.Events;
using System.Threading.Tasks;

namespace RayWorkflow.Grains
{
    [Producer(lBCount: 4)]
    public class WorkflowFormGrain : CrudGrain<Guid, WorkflowFormState, WorkflowForm, WorkflowFormDto>, IWorkflowFormGrain<WorkflowFormDto>
    {
        public async Task Disable(bool disabled)
        {
            if (disabled)
            {
                await RaiseEvent(new DisabledEvent()
                {
                    Id = GrainId,
                    LastModificationTime = DateTime.Now
                });
            }
            else
            {
                await RaiseEvent(new EnabledEvent()
                {
                    Id = GrainId,
                    LastModificationTime = DateTime.Now
                });
            }
        }
    }
}
