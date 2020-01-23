using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RayWorkflow.Domain.Shared;

namespace RayWorkflow.Domain
{
    public class GrainEfCoreRepositoryBase<TEntity, TPrimaryKey> : IGrainRepository<TEntity, TPrimaryKey>
          where TEntity : class, IEntity<TPrimaryKey>
    {
        public GrainEfCoreRepositoryBase(DbContext context)
        {
            Context = context;
        }

        protected DbContext Context { get; set; }

        public virtual DbSet<TEntity> DbSet => Context.Set<TEntity>();

        #region Implementation of IDisposable

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;
            Context.Dispose();
        }

        protected bool IsDisposed;

        #endregion

        public TEntity FirstOrDefault(TPrimaryKey id)
        {
            return DbSet.FirstOrDefault(CreateEqualityExpressionForId(id));
        }

        public Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return DbSet.FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
        }

        public void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            var entityEntry = await DbSet.AddAsync(entity);
            return entityEntry.Entity;
        }

        public TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public void Delete(TPrimaryKey id)
        {
            var entity = GetFromChangeTrackerOrNull(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            entity = FirstOrDefault(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            DbSet.Remove(entity);
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public Task CommitAsync()
        {
            return Context.SaveChangesAsync();
        }

        protected virtual void AttachIfNot(TEntity entity)
        {
            var entry = Context.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null)
            {
                return;
            }

            DbSet.Attach(entity);
        }

        private TEntity GetFromChangeTrackerOrNull(TPrimaryKey id)
        {
            var entry = Context.ChangeTracker.Entries()
                .FirstOrDefault(
                    ent =>
                        ent.Entity is TEntity entity &&
                        EqualityComparer<TPrimaryKey>.Default.Equals(id, entity.Id)
                );

            return entry?.Entity as TEntity;
        }

        protected virtual Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));
            var leftExpression = Expression.PropertyOrField(lambdaParam, "Id");
            Expression<Func<object>> closure = () => id;
            var rightExpression = Expression.Convert(closure.Body, leftExpression.Type);
            var lambdaBody = Expression.Equal(leftExpression, rightExpression);
            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
    }
}
