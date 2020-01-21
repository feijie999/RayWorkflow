using System;
using System.Threading.Tasks;

namespace RayWorkflow.Domain
{
    using RayWorkflow.Domain.Shared;

    public interface IGrainRepository<TEntity, in TPrimaryKey> : IDisposable where TEntity : class, IEntity<TPrimaryKey>
    {
        TEntity FirstOrDefault(TPrimaryKey id);

        Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id);

        void Insert(TEntity entity);

        ValueTask<TEntity> InsertAsync(TEntity entity);

        TEntity Update(TEntity entity);

        void Delete(TPrimaryKey entity);

        void Commit();

        Task CommitAsync();
    }
}