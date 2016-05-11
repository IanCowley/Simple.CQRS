using Simple.CQRS.Exceptions;

using StructureMap;

namespace Simple.CQRS.TestInfrastructure
{
    public class SimpleCQRSTestIOC
    {
        private static Container container = null;

        public static Container GetContainer()
        {
            if (container == null)
            {
                throw new EmptyContainerException();
            }

            return container;
        }

        public static TInstance Get<TInstance>()
        {
            return GetContainer().GetInstance<TInstance>();
        }

        public static void SetContainer(Container container)
        {
            SimpleCQRSTestIOC.container = container;
        }
    }
}
