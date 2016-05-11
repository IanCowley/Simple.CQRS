

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Simple.CQRS.Domain;

namespace Simple.CQRS.TestInfrastructure.DynamicLinqBuilding
{
    public class FakeContext : IDbContext
    {
        public IDbSet<T> Set<T>() where T : class
        {
            foreach (PropertyInfo property in this.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(IDbSet<T>))
                {
                    return property.GetValue(this, null) as IDbSet<T>;
                }
            }

            throw new Exception("Type collection not found");
        }

        public virtual IDbSet<TAggregate> GetSet<TAggregate>() where TAggregate : class, IAggregate
        {
            return this.Set<TAggregate>();
        }

        public TAggregate GetAggregateRoot<TAggregate>(Guid id) where TAggregate : class, IAggregate
        {
            return this.GetSet<TAggregate>().FirstOrDefault(x => x.Id == id);
        }

        public TAggregate CreateNew<TAggregate>() where TAggregate : class, IAggregate
        {
            var aggregate = Activator.CreateInstance<TAggregate>();
            this.Set<TAggregate>().Add(aggregate);
            return aggregate;
        }

        public void Commit()
        {
        }

        public void Attach<TEntity>(TEntity entity, Func<ICollection<TEntity>> getCollection) where TEntity : class, IEntity
        {
            getCollection().Add(entity);
        }

        public bool HasAny<TAggregateRoot, TEntity>(
            TAggregateRoot aggregateRoot, 
            Expression<Func<TAggregateRoot, ICollection<TEntity>>> navigationProperty, 
            Expression<Func<TEntity, bool>> predicate) 
            where TAggregateRoot : class, IAggregate where TEntity : class, IEntity
        {
            return  GetCollection(aggregateRoot, navigationProperty).Any(predicate.Compile());
        }

        private static ICollection<TEntity> GetCollection<TAggregateRoot, TEntity>(
            TAggregateRoot aggregateRoot,
            Expression<Func<TAggregateRoot, ICollection<TEntity>>> navigationProperty)
            where TAggregateRoot : class, IAggregate 
            where TEntity : class, IEntity
        {
            return (ICollection<TEntity>)navigationProperty.Compile()(aggregateRoot);
        }

        public TEntity SingleOrDefault<TAggregateRoot, TEntity>(
            TAggregateRoot aggregateRoot,
            Expression<Func<TAggregateRoot, ICollection<TEntity>>> navigationProperty,
            Expression<Func<TEntity, bool>> predicate)
            where TAggregateRoot : class, IAggregate
            where TEntity : class, IEntity
        {
            return GetCollection(aggregateRoot, navigationProperty).SingleOrDefault(predicate.Compile());
        }

        public void Dispose()
        {
        }
    }
}