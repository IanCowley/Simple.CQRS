using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using Simple.CQRS.Domain;
using Simple.CQRS.Exceptions;
using Simple.CQRS.Extensions;
using Simple.CQRS.Query;

namespace Simple.CQRS.ModelConfiguration
{
    public interface IEntityMap
    {
        Type ViewType { get; }

        Type EntityType { get; }

        IEnumerable<IEntity> GetEntities(IDbContext context);
    }

    public class EntityMap<TDbContext, TAggregate, TEntity> : IEntityMap
        where TDbContext : IDbContext
        where TAggregate : AggregateRoot
        where TEntity : IEntity
    {
        private readonly AggregateMap<TDbContext, TAggregate> aggregateMap;

        public EntityMap(AggregateMap<TDbContext, TAggregate> aggregateMap)
        {
            this.aggregateMap = aggregateMap;
        }

        public Type ViewType { get; private set; }

        public Type EntityType
        {
            get
            {
                return typeof(TEntity);
            }
        }

        public Func<TDbContext, IEnumerable<TEntity>> Map { get; set; }

        public AggregateMap<TDbContext, TAggregate> IsMappedTo<TView>(Func<TDbContext, IEnumerable<TEntity>> map)
            where TView : IView
        {
            if (this.AggregateMapAlreadyContainsDefinitionForView<TView>())
            {
                throw new ConfigurationMappingException(
                    "There are more than one entity maps for view type {0}".FormatString(typeof(TView)));
            }

            this.ViewType = typeof(TView);
            this.Map = map;
            Mapper.CreateMap<TEntity, TView>();
            Mapper.CreateMap<TView, TEntity>();
            return this.aggregateMap;
        }

        private bool AggregateMapAlreadyContainsDefinitionForView<TView>()
            where TView : IView
        {
            return this.aggregateMap.ViewMapAlreadyExists<TView>();
        }

        public IEnumerable<IEntity> GetEntities(IDbContext context)
        {
            var entities = this.Map((TDbContext)context).ToList();
            return entities.Select(x => x as IEntity);
        }
    }
}
