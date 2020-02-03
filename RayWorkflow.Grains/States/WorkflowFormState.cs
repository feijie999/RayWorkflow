using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Ray.Core.Snapshot;
using RayWorkflow.Domain.Shared.Workflow;

namespace RayWorkflow.Grains.States
{
    using RayWorkflow.Domain.Workflow;

    [Serializable]
    public class WorkflowFormState : WorkflowForm, ICloneable<WorkflowFormState>
    {
        public Guid StateId { get; set; }

        public WorkflowFormState Clone()
        {
            using (var memoryStream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, this);
                memoryStream.Position = 0;
                return (WorkflowFormState)formatter.Deserialize(memoryStream);
            }
        }
    }
}
