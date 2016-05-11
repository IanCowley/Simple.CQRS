using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Simple.CQRS.Domain
{
    public class EntityContextWrapper
    {
        private readonly IDbContext context;

        public EntityContextWrapper(IDbContext context)
        {
            this.context = context;
        }

        public bool HasAny<TAggregateRoot, TEntity>(
            TAggregateRoot aggregateRoot,
            Expression<Func<TAggregateRoot, ICollection<TEntity>>> navigationProperty,
            Expression<Func<TEntity, bool>> predicate)
            where TAggregateRoot : class, IAggregate
            where TEntity : class, IEntity
        {
            return context.HasAny(aggregateRoot, navigationProperty, predicate);
        }

        public TEntity SingleOrDefault<TAggregateRoot, TEntity>(
            TAggregateRoot aggregateRoot,
            Expression<Func<TAggregateRoot, ICollection<TEntity>>> navigationProperty,
            Expression<Func<TEntity, bool>> predicate)
            where TAggregateRoot : class, IAggregate
            where TEntity : class, IEntity
        {
            return context.SingleOrDefault(aggregateRoot, navigationProperty, predicate);
        }

        public void Attach<TEntity>(TEntity entity, Func<ICollection<TEntity>> getCollection) where TEntity : class, IEntity
        {
            context.Attach(entity, getCollection);
        }
    }
}
