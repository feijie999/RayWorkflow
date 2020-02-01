using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Ray.Core;
using Ray.Core.Event;
using RayWorkflow.Domain;
using RayWorkflow.Domain.Shared;
using RayWorkflow.Grains.Events;
using RayWorkflow.IGrains;

namespace RayWorkflow.Grains
{
    public abstract class
          CrudDbGrain<TMain, TSnapshot, TPrimaryKey, TEntityType> :
              ObserverGrain<TPrimaryKey, TMain>, ICrudDbGrain<TPrimaryKey>
           where TSnapshot : class, new()
           where TEntityType : class, IEntity<TPrimaryKey>
    {
        protected ICrudHandle<TPrimaryKey, TSnapshot> CrudHandle;
        protected IMapper Mapper;

        protected override ValueTask DependencyInjection()
        {
            CrudHandle = ServiceProvider.GetService<ICrudHandle<TPrimaryKey, TSnapshot>>();
            Mapper = ServiceProvider.GetService<IMapper>();
            return base.DependencyInjection();
        }

        #region Overrides of ObserverGrain<TMain,TPrimaryKey>

        protected override async ValueTask OnEventDelivered(FullyEvent<TPrimaryKey> fullyEvent)
        {
            switch (fullyEvent.Event)
            {
                case CreatingSnapshotEvent<TSnapshot> evt:
                    await CreatingSnapshotHandle(evt);
                    break;
                case UpdatingSnapshotEvent<TSnapshot> evt:
                    await UpdatingSnapshotHandle(evt);
                    break;
                case DeletingSnapshotEvent<TPrimaryKey> evt:
                    await DeletingSnapshotHandle(evt);
                    break;
            }

            await Process(fullyEvent);
        }

        private async Task CreatingSnapshotHandle(CreatingSnapshotEvent<TSnapshot> evt)
        {
            using var repository = ServiceProvider.GetService<IGrainRepository<TEntityType, TPrimaryKey>>();
            var entity = Mapper.Map<TEntityType>(evt.Snapshot);
            repository.Insert(entity);
            await repository.CommitAsync();
        }

        private async Task UpdatingSnapshotHandle(UpdatingSnapshotEvent<TSnapshot> evt)
        {
            using var repository = ServiceProvider.GetService<IGrainRepository<TEntityType, TPrimaryKey>>();
            var entity = Mapper.Map<TEntityType>(evt.Snapshot);
            repository.Update(entity);
            await repository.CommitAsync();
        }

        private async Task DeletingSnapshotHandle(DeletingSnapshotEvent<TPrimaryKey> evt)
        {
            using var repository = ServiceProvider.GetService<IGrainRepository<TEntityType, TPrimaryKey>>();
            repository.Delete(evt.PrimaryKey);
            await repository.CommitAsync();
        }
        #endregion

        public abstract Task Process(FullyEvent<TPrimaryKey> @event);
    }
}