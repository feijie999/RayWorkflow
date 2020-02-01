using System;
using RayWorkflow.Domain.Shared.Workflow;
using RayWorkflow.Domain.Workflow;
using RayWorkflow.Grains.States;
using RayWorkflow.IGrains;
using Ray.EventBus.RabbitMQ;

namespace RayWorkflow.Grains
{
    [Producer(lBCount: 4)]
    public class WorkflowFormGrain : CrudGrain<Guid, WorkflowFormState, WorkflowForm, WorkflowFormDto>, IWorkflowFormGrain<WorkflowFormDto>
    {
    }
}
