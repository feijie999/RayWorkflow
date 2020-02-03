using Orleans;
using Ray.Core.Observer;
using System;

namespace RayWorkflow.IGrains
{
    public interface IWorkflowFormDbGrain : IObserver, IGrainWithGuidKey
    {
        
    }
}