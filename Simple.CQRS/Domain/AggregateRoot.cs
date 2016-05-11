using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

using Simple.CQRS.Exceptions;

namespace Simple.CQRS.Domain
{
    public abstract class AggregateRoot : Entity, IAggregate
    {
        [NotMapped]
        public EntityContextWrapper EntityContextWrapper { get; set; }

        protected void AttachEntityToContext<TEntity>(TEntity entity, Func<ICollection<TEntity>> getCollection)
            where TEntity : class, IEntity
        {
            this.EntityContextWrapper.Attach(entity, getCollection);
        }

        public bool HasAny<TAggregateRoot, TEntity>(
            TAggregateRoot aggregateRoot,
            Expression<Func<TAggregateRoot, ICollection<TEntity>>> navigationProperty,
            Expression<Func<TEntity, bool>> predicate)
            where TAggregateRoot : class, IAggregate
            where TEntity : class, IEntity
        {
            return this.EntityContextWrapper.HasAny(aggregateRoot, navigationProperty, predicate);
        }

        public TEntity FindEntity<TAggregateRoot, TEntity>(
            TAggregateRoot aggregateRoot,
            Expression<Func<TAggregateRoot, ICollection<TEntity>>> navigationProperty,
            Expression<Func<TEntity, bool>> predicate)
            where TAggregateRoot : class, IAggregate
            where TEntity : class, IEntity
        {
            return EntityContextWrapper.SingleOrDefault(aggregateRoot, navigationProperty, predicate);
        }

        public TEntity GetEntity<TAggregateRoot, TEntity>(
           TAggregateRoot aggregateRoot,
           Guid id,
           Expression<Func<TAggregateRoot, ICollection<TEntity>>> navigationProperty)
            where TAggregateRoot : class, IAggregate
            where TEntity : class, IEntity
        {
            var entity = EntityContextWrapper.SingleOrDefault(aggregateRoot, navigationProperty, x => x.Id == id);

            if (entity == null)
            {
                throw new EntityNotFoundException(aggregateRoot.Id, typeof(TEntity));
            }

            return entity;
        }
    }
}
