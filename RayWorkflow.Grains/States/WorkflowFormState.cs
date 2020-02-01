using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Ray.Core.Snapshot;
using RayWorkflow.Domain.Shared.Workflow;

namespace RayWorkflow.Grains.States
{

    [Serializable]
    public class WorkflowFormState : WorkflowFormDto, ICloneable<WorkflowFormState>
    {
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
