using AutoMapper;
using Ray.Core.Event;
using Ray.Core.Snapshot;
using Ray.DistributedTx;
using RayWorkflow.IGrains.Events;

namespace RayWorkflow.Grains
{
    public class CrudHandle<TPrimaryKey, TSnapshot> : DTxSnapshotHandler<TPrimaryKey, TSnapshot>,
           ICrudHandle<TPrimaryKey, TSnapshot>
           where TSnapshot : class, new()
    {
        protected readonly IMapper Mapper;

        public CrudHandle(IMapper mapper)
        {
            Mapper = mapper;
        }

        public override void CustomApply(Snapshot<TPrimaryKey, TSnapshot> snapshot, FullyEvent<TPrimaryKey> fullyEvent)
        {
            switch (fullyEvent.Event)
            {
                case CreatingSnapshotEvent<TSnapshot> evt:
                    CreatingSnapshotHandle(snapshot.State, evt);
                    break;
                case UpdatingSnapshotEvent<TSnapshot> evt:
                    UpdatingSnapshotHandle(snapshot.State, evt);
                    break;
                case DeletingSnapshotEvent<TPrimaryKey> evt:
                    DeletingSnapshotHandle(snapshot.State, evt);
                    break;
                default:
                    base.CustomApply(snapshot, fullyEvent);
                    break;
            }
        }

        public void CreatingSnapshotHandle(TSnapshot snapshotState, CreatingSnapshotEvent<TSnapshot> evt)
        {
            Mapper.Map(evt.Snapshot, snapshotState);
        }

        public void UpdatingSnapshotHandle(TSnapshot snapshotState, UpdatingSnapshotEvent<TSnapshot> evt)
        {
            Mapper.Map(evt.Snapshot, snapshotState);
        }

        public void DeletingSnapshotHandle(TSnapshot snapshotState, DeletingSnapshotEvent<TPrimaryKey> evt)
        {
            var defaultSnapshot = new TSnapshot();
            Mapper.Map(defaultSnapshot, snapshotState);
        }
    }
}
