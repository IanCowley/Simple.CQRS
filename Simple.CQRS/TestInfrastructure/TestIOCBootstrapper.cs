using Simple.CQRS.Command;
using Simple.CQRS.Domain;
using Simple.CQRS.Extensions;
using Simple.CQRS.ModelConfiguration;
using Simple.CQRS.Query;

using StructureMap;
using StructureMap.Graph;

namespace Simple.CQRS.TestInfrastructure
{
    public class TestIOCBootstrapper
    {
        public static void Bootstrap()
        {
            StartBootstrap();
        }

        public static void BootstrapWithRegistries(params Registry[] registries)
        {
            StartBootstrap(registries);
        }

        private static void StartBootstrap(params Registry[] registries)
        {
            SetupIOC(registries);
            SetupRuntimeMappings();
        }

        private static void SetupIOC(params Registry[] registries)
        {
            var setupContainer = new Container(
                container =>
                {
                    container.Scan(
                        scan =>
                        {
                            scan.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.Contains("VA."));
                            scan.TheCallingAssembly();
                            scan.WithDefaultConventions();
                            scan.RegisterConcreteTypesAgainstTheFirstInterface();
                            AddPluggableTypes(scan);
                        });

                    SetupTestDependencies(container);
                });

            SimpleCQRSTestIOC.SetContainer(setupContainer);

            if (registries != null)
            {
                registries.ForEach(
                    registry => SimpleCQRSTestIOC.GetContainer().Configure(container => container.AddRegistry(registry)));
            }
        }

        // IPluggable and look for registries should do this, but it's not, so I've hacked it IACO
        private static void AddPluggableTypes(IAssemblyScanner scan)
        {
            scan.AddAllTypesOf<IPluggable>();
            scan.AddAllTypesOf<IHandleCommandWithoutResult>();
            scan.AddAllTypesOf<IDomainRegistration>();
            scan.AddAllTypesOf<IDiscoverableMapper>();
        }


        private static void SetupTestDependencies(ConfigurationExpression container)
        {
            container.For<IDomainConfigurationStore>().Use<DomainConfigurationStore>().Singleton();
            container.For<IDbContextFactory>().Use<FakeDbContextFactory>();
            container.For<ICommandDispatcher>().Use<TestCommandDispatcher>();

            container.For<IRepository>()
                .Use<FakeRepository>()
                .Ctor<FakeDbContextFactory>()
                .Is(x => (FakeDbContextFactory)x.GetInstance<IDbContextFactory>());
        }

        private static void SetupRuntimeMappings()
        {
            Container container = SimpleCQRSTestIOC.GetContainer();
            container.GetInstance<IDomainRegistrationScanner>().Scan();
            container.GetInstance<IDiscoverableMapScanner>().Scan();
        }
    }
}