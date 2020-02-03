using Orleans;
using System;
using System.Threading.Tasks;

namespace RayWorkflow.IGrains
{
    public interface IWorkflowFormGrain<TSnapshotDto> : IGrainWithGuidKey, ICrudGrain<TSnapshotDto>
        where TSnapshotDto : class, new()
    {
        Task Disable(bool disabled);
    }
}