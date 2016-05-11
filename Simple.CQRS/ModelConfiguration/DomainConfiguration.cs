using System;
using System.Collections.Generic;
using System.Linq;

using Simple.CQRS.Domain;
using Simple.CQRS.Query;

namespace Simple.CQRS.ModelConfiguration
{
    public interface IDomainConfiguration
    {
        IEnumerable<IEntityMap> GetEntityMaps<TView>()
            where TView : IView;
    }

    public class DomainConfiguration<TDbContext> : IDomainConfiguration
         where TDbContext : IDbContext
    {
        private readonly Dictionary<Type, IAggregateMap> aggregateMaps;

        public DomainConfiguration()
        {
            this.aggregateMaps = new Dictionary<Type, IAggregateMap>();
        }

        public AggregateMap<TDbContext, TAggregate> ForAggregate<TAggregate>()
            where TAggregate : AggregateRoot
        {
            var aggregateMap = this.GetDomainConfiguration<TAggregate>();
            return aggregateMap;
        }

        public IEnumerable<IEntityMap> GetEntityMaps<TView>() where TView : IView
        {
            var entityMaps = this
                                .aggregateMaps
                                .Where(x => x.Value.HasEntityMapFor<TView>())
                                .Select(x => x.Value)
                                .Select(x => x.GetEntityMap<TView>());

            return entityMaps;
        }

        private AggregateMap<TDbContext, TAggregate> GetDomainConfiguration<TAggregate>()
            where TAggregate : AggregateRoot
        {
            if (!this.aggregateMaps.ContainsKey(typeof(TAggregate)))
            {
                this.aggregateMaps[typeof(TAggregate)] = new AggregateMap<TDbContext, TAggregate>();
            }

            return this.aggregateMaps[typeof(TAggregate)] as AggregateMap<TDbContext, TAggregate>;
        }
    }
}
