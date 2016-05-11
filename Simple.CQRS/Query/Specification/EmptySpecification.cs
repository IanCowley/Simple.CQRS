using DapperExtensions;

namespace Simple.CQRS.Query.Specification
{
    public class EmptySpecification<TView> : ISpecification<TView> where TView : class, IView
    {
        public IPredicate ToPredicate()
        {
            return new EmptyPredicate();
        }
    }
}
