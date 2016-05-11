using System.Collections.Generic;
using System.Linq;

using Simple.CQRS.Domain;
using Simple.CQRS.Exceptions;
using Simple.CQRS.Extensions;
using Simple.CQRS.Query;

namespace Simple.CQRS.ModelConfiguration
{
    public interface IAggregateMap
    {
        IEntityMap GetEntityMap<TView>() where TView : IView;

        bool HasEntityMapFor<TView>() where TView : IView;
    }

    public class AggregateMap<TDbContext, TAggregate> : IAggregateMap
        where TDbContext : IDbContext
        where TAggregate : AggregateRoot
    {
        private readonly List<IEntityMap> entityMaps;

        public AggregateMap()
        {
            this.entityMaps = new List<IEntityMap>();
        }

        public EntityMap<TDbContext, TAggregate, TEntity> Entity<TEntity>()
            where TEntity : class, IEntity
        {
            if (this.EntityMapAlreadyExists<TEntity>())
            {
                throw new ConfigurationMappingException(
                    "Duplicate Map.  Entity {0} has already been mapped".FormatString(typeof(TEntity)));
            }

            var map = new EntityMap<TDbContext, TAggregate, TEntity>(this);
            this.entityMaps.Add(map);
            return map;
        }

        public IEntityMap GetEntityMap<TView>() where TView : IView
        {
            return this.GetMapForType<TView>().FirstOrDefault();
        }

        public bool HasEntityMapFor<TView>() where TView : IView
        {
            return this.ViewMapAlreadyExists<TView>();
        }

        internal bool ViewMapAlreadyExists<TView>() where TView : IView
        {
            return this.GetMapForType<TView>().Any();
        }

        private bool EntityMapAlreadyExists<TEntity>() where TEntity : class, IEntity
        {
            return this.entityMaps.Exists(x => x.EntityType == typeof(TEntity));
        }

        private IEnumerable<IEntityMap> GetMapForType<TView>() where TView : IView
        {
            return this.entityMaps.Where(x => x.ViewType == typeof(TView));
        }
    }
}
