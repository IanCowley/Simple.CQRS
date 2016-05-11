using System;

using Simple.CQRS.Domain;

namespace Simple.CQRS.TestInfrastructure
{
    public class FakeDbContextFactory : IDbContextFactory
    {
        private static IDbContext context = null;

        private static Type contextType = null;

        public IDbContext GetContext()
        {
            CheckContextTypeSet();

            if (context == null)
            {
                Reset();
            }

            return context;
        }

        public static void Reset()
        {
            context = (IDbContext)Activator.CreateInstance(contextType);
        }

        public static void SetDbContextType(Type dbContextType) 
        {
            contextType = dbContextType;
        }

        private static void CheckContextTypeSet()
        {
            if (contextType == null)
            {
                throw new TestInfrastructureException(
                    "This operation requires bootstrapping of the context type, please call the SetDbContextType<TDbContext>() calling this method");
            }
        }
    }

}
