using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Simple.CQRS.Domain
{
    public interface IDbContext : IDisposable
    {
        TAggregate GetAggregateRoot<TAggregate>(Guid id) where TAggregate : class, IAggregate;

        TAggregate CreateNew<TAggregate>() where TAggregate : class, IAggregate;

        void Commit();

        void Attach<TEntity>(TEntity entity, Func<ICollection<TEntity>> getCollection) where TEntity : class, IEntity;

        bool HasAny<TAggregateRoot, TEntity>(
            TAggregateRoot aggregateRoot,
            Expression<Func<TAggregateRoot, ICollection<TEntity>>> navigationProperty,
            Expression<Func<TEntity, bool>> predicate) where TAggregateRoot : class, IAggregate
            where TEntity : class, IEntity;

        TEntity SingleOrDefault<TAggregateRoot, TEntity>(
            TAggregateRoot aggregateRoot,
            Expression<Func<TAggregateRoot, ICollection<TEntity>>> navigationProperty,
            Expression<Func<TEntity, bool>> predicate) where TAggregateRoot : class, IAggregate
            where TEntity : class, IEntity;
    }
}