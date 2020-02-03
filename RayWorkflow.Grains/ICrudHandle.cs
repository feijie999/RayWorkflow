using RayWorkflow.IGrains.Events;

namespace RayWorkflow.Grains
{
    public interface ICrudHandle<TPrimaryKey, TSnapshot> where TSnapshot : class, new()
    {
        void CreatingSnapshotHandle(TSnapshot snapshotState, CreatingSnapshotEvent<TSnapshot> evt);

        void UpdatingSnapshotHandle(TSnapshot snapshotState, UpdatingSnapshotEvent<TSnapshot> evt);

        void DeletingSnapshotHandle(TSnapshot snapshotState, DeletingSnapshotEvent<TPrimaryKey> evt);
    }
}
