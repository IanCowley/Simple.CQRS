using System;
using System.Collections.Generic;
using System.Linq;

using Simple.CQRS.Domain;
using Simple.CQRS.Exceptions;
using Simple.CQRS.Extensions;
using Simple.CQRS.Query;

namespace Simple.CQRS.ModelConfiguration
{
    public interface IDomainConfigurationStore
    {
        IDomainConfiguration GetDomainConfiguration<TDbContext>() where TDbContext : IDbContext;

        IEnumerable<IDomainConfiguration> GetDomainConfigurationsForView<TView>() where TView : IView;

        IEnumerable<IEntityMap> GetEntityMaps<TView>() where TView : IView;
    }

    public class DomainConfigurationStore : IDomainConfigurationStore
    {
        private readonly Dictionary<Type, IDomainConfiguration> domainConfigurations;

        public DomainConfigurationStore()
        {
            this.domainConfigurations = new Dictionary<Type, IDomainConfiguration>();
        }

        public IEnumerable<IDomainConfiguration> GetDomainConfigurationsForView<TView>() where TView : IView
        {
            var result = this
                .domainConfigurations
                .Values
                .Where(x => x.GetEntityMaps<TView>().Any());

            return result;
        }

        public IEnumerable<IEntityMap> GetEntityMaps<TView>() where TView : IView
        {
            var domainConfiguration = this.GetDomainConfigurationsForView<TView>();

            if (domainConfiguration.Any())
            {
                var result = domainConfiguration.SelectMany(map => map.GetEntityMaps<TView>());
                return result;
            }

            throw new ConfigurationMappingException("Entity Map has not been created for map type {0}".FormatString(typeof(TView)));
        }

        public IDomainConfiguration GetDomainConfiguration<TDbContext>() where TDbContext : IDbContext
        {
            if (!this.domainConfigurations.ContainsKey(typeof(TDbContext)))
            {
                this.domainConfigurations[typeof(TDbContext)] = new DomainConfiguration<TDbContext>();
            }

            return this.domainConfigurations[typeof(TDbContext)];
        }
    }
}
