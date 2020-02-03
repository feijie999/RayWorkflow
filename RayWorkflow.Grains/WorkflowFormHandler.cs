using AutoMapper;
using RayWorkflow.IGrains.Events;
using RayWorkflow.Grains.States;
using System;

namespace RayWorkflow.Grains
{
    public class WorkflowFormHandler : CrudHandle<Guid, WorkflowFormState>, IWorkflowFormHandler
    {
        public WorkflowFormHandler(IMapper mapper)
            : base(mapper)
        {
        }

        public void EventHandle(WorkflowFormState state, DisabledEvent evt)
        {
            state.Disabled = true;
            state.LastModificationTime = evt.LastModificationTime;
        }

        public void EventHandle(WorkflowFormState state, EnabledEvent evt)
        {
            state.Disabled = false;
            state.LastModificationTime = evt.LastModificationTime;
        }
    }
}