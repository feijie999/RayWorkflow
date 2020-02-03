using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Ray.Core;
using Ray.Core.Event;
using RayWorkflow.Domain;
using RayWorkflow.Domain.Shared;
using RayWorkflow.IGrains;
using RayWorkflow.IGrains.Events;

namespace RayWorkflow.Grains
{
    using System;

    public abstract class
          CrudDbGrain<TMain, TSnapshot, TPrimaryKey, TEntityType> :
              ObserverGrain<TPrimaryKey, TMain>
           where TSnapshot : class, TEntityType, new()
           where TEntityType : class, IEntity<TPrimaryKey>, new()
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
                    evt.Snapshot.SetId(fullyEvent.StateId);
                    await CreatingSnapshotHandle(evt);
                    return;
                case UpdatingSnapshotEvent<TSnapshot> evt:
                    evt.Snapshot.SetId(fullyEvent.StateId);
                    await UpdatingSnapshotHandle(evt);
                    return;
                case DeletingSnapshotEvent<TPrimaryKey> evt:
                    await DeletingSnapshotHandle(evt);
                    return;
            }

            await base.OnEventDelivered(fullyEvent);
        }

        public async Task CreatingSnapshotHandle(CreatingSnapshotEvent<TSnapshot> evt)
        {
            using var repository = ServiceProvider.GetService<IGrainRepository<TEntityType, TPrimaryKey>>();
            var entity = Mapper.Map<TEntityType>(evt.Snapshot);
            repository.Insert(entity);
            await repository.CommitAsync();
        }

        public async Task UpdatingSnapshotHandle(UpdatingSnapshotEvent<TSnapshot> evt)
        {
            using var repository = ServiceProvider.GetService<IGrainRepository<TEntityType, TPrimaryKey>>();
            var entity = Mapper.Map<TEntityType>(evt.Snapshot);
            repository.Update(entity);
            await repository.CommitAsync();
        }

        public async Task DeletingSnapshotHandle(DeletingSnapshotEvent<TPrimaryKey> evt)
        {
            using var repository = ServiceProvider.GetService<IGrainRepository<TEntityType, TPrimaryKey>>();
            repository.Delete(evt.PrimaryKey);
            await repository.CommitAsync();
        }
        #endregion
    }
}