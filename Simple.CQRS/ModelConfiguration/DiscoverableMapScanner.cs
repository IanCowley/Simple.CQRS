using Simple.CQRS.Extensions;

namespace Simple.CQRS.ModelConfiguration
{
    public interface IDiscoverableMapScanner
    {
        void Scan();
    }

    public class DiscoverableMapScanner : IDiscoverableMapScanner
    {
        private readonly IDiscoverableMapper[] discoverableMappers;

        public DiscoverableMapScanner(IDiscoverableMapper[] discoverableMappers)
        {
            this.discoverableMappers = discoverableMappers;
        }

        public void Scan()
        {
            discoverableMappers.ForEach(mapper => mapper.Map());
        }
    }
}
