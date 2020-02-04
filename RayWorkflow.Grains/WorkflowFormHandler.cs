using AutoMapper;
using RayWorkflow.IGrains.Events;
using RayWorkflow.Grains.States;
using System;

namespace RayWorkflow.Grains
{
    using RayWorkflow.Domain.Workflow;

    public class WorkflowFormHandler : CrudHandle<Guid, WorkflowFormState>, IWorkflowFormHandler
    {
        public WorkflowFormHandler(IMapper mapper)
            : base(mapper)
        {
        }

        public void EventHandle(WorkflowFormState state, DisabledEvent evt)
        {
            WorkflowFormHandler.EventHandle(state, evt);
        }

        public static void EventHandle(WorkflowForm entity, DisabledEvent evt)
        {
            entity.Disabled = true;
            entity.LastModificationTime = evt.LastModificationTime;
        }

        public void EventHandle(WorkflowFormState state, EnabledEvent evt)
        {
            WorkflowFormHandler.EventHandle(state, evt);
        }

        public static void EventHandle(WorkflowForm entity, EnabledEvent evt)
        {
            entity.Disabled = false;
            entity.LastModificationTime = evt.LastModificationTime;
        }
    }
}