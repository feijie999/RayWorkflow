using RayWorkflow.Grains.States;
using RayWorkflow.IGrains.Events;

namespace RayWorkflow.Grains
{
    public interface IWorkflowFormHandler
    {
        void EventHandle(WorkflowFormState state, DisabledEvent evt);

        void EventHandle(WorkflowFormState state, EnabledEvent evt);
    }
}