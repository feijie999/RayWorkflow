using System;
using RayWorkflow.Domain.Shared.Workflow;
using RayWorkflow.Domain.Workflow;
using RayWorkflow.Grains.States;
using RayWorkflow.IGrains;

namespace RayWorkflow.Grains
{
    public class WorkflowFormGrain : CrudGrain<Guid, WorkflowFormState, WorkflowForm, WorkflowFormDto>, IWorkflowFormGrain<WorkflowFormDto>
    {
        public WorkflowFormGrain()
        {
        }
    }
}
