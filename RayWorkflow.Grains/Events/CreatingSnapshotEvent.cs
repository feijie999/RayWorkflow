using Ray.Core.Event;
using System;

namespace RayWorkflow.Grains.Events
{
    [Serializable]
    public class CreatingSnapshotEvent<TSnapshot> : IEvent
        where TSnapshot : class, new()
    {
        public TSnapshot Snapshot { get; set; }

        public CreatingSnapshotEvent()
        {
        }

        public CreatingSnapshotEvent(TSnapshot snapshot) : this()
        {
            Snapshot = snapshot;
        }
    }
}