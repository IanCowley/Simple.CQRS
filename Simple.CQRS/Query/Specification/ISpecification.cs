using DapperExtensions;

namespace Simple.CQRS.Query.Specification
{
    public interface ISpecification<TView> where TView : class, IView
    {
        IPredicate ToPredicate();
    }
}
