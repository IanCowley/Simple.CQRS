using Simple.CQRS.Domain;
using Simple.CQRS.Exceptions;

namespace Simple.CQRS.ModelConfiguration
{
    public interface IDomainConfigurer
    {
        DomainConfiguration<TDbContext> ForDbContext<TDbContext>() where TDbContext : IDbContext;
    }

    public class DomainConfigurer : IDomainConfigurer
    {
        private readonly IDomainConfigurationStore domainConfigurationStore;

        public DomainConfigurer(IDomainConfigurationStore domainConfigurationStore)
        {
            this.domainConfigurationStore = domainConfigurationStore;
        }

        public DomainConfiguration<TDbContext> ForDbContext<TDbContext>() where TDbContext : IDbContext
        {
            CheckDBContextTypeIsInterface<TDbContext>();
            var configuration = this.domainConfigurationStore.GetDomainConfiguration<TDbContext>();
            return (DomainConfiguration<TDbContext>)configuration;
        }

        private void CheckDBContextTypeIsInterface<T>()
        {
            if (!typeof(T).IsInterface)
            {
                throw new ConfigurationMappingException("The db context type must be an interface so it can be faked later");
            }
        }
    }
}
