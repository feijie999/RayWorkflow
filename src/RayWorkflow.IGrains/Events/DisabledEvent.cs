using Ray.Core.Event;
using System;

namespace RayWorkflow.IGrains.Events
{
    [Serializable]
    public class DisabledEvent : IEvent
    {
        public Guid Id { get; set; }

        public DateTime LastModificationTime { get; set; }
    }
}