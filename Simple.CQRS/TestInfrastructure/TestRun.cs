using Simple.CQRS.Domain;

using StructureMap;

namespace Simple.CQRS.TestInfrastructure
{
    public class TestRun
    {
        public static void Start()
        {
            TestIOCBootstrapper.Bootstrap();
        }

        public static void StartWithIOCRegistries(params Registry[] registries)
        {
            TestIOCBootstrapper.BootstrapWithRegistries(registries);
        }

        public static void StartWithDbSupport<TDbContext>()
            where TDbContext : IDbContext
        {
            Start();
            FakeDbContextFactory.SetDbContextType(typeof(TDbContext));
            FakeDbContextFactory.Reset();
        }

        public static void StartWithDbSupportAndIOCRegistries<TDbContext>(params Registry[] registries)
            where TDbContext : IDbContext
        {
            StartWithIOCRegistries(registries);
            FakeDbContextFactory.SetDbContextType(typeof(TDbContext));
            FakeDbContextFactory.Reset();
        }

    }
}