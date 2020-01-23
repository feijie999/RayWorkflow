using System;
using Ray.Core.Event;
using RayWorkflow.Grains.Events;

namespace RayWorkflow.Grains
{
    public interface ICrudHandle<TPrimaryKey, TSnapshot> where TSnapshot : class, new()
    {
        void Apply(TSnapshot snapshot, IEvent evt);

        void CreatingSnapshotHandle(TSnapshot snapshotState, CreatingSnapshotEvent<TSnapshot> evt);
    }
}
