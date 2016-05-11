using Simple.CQRS.Extensions;

namespace Simple.CQRS.ModelConfiguration
{
    public interface IDomainRegistrationScanner
    {
        void Scan();
    }

    public class DomainRegistrationScanner : IDomainRegistrationScanner
    {
        private readonly IDomainRegistration[] domainRegistrations;

        public DomainRegistrationScanner(IDomainRegistration[] domainRegistrations)
        {
            this.domainRegistrations = domainRegistrations;
        }

        public void Scan()
        {
            this.domainRegistrations.ForEach(registration => registration.RegisterDomain());
        }
    }
}
