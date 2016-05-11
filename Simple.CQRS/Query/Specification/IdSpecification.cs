using System;

using DapperExtensions;

namespace Simple.CQRS.Query.Specification
{
    public class IdSpecification<TView> : ISpecification<TView>
        where TView : class, IView
    {
        public Guid Id { get; private set; }

        public IdSpecification(Guid id)
        {
            this.Id = id;
        }

        public IPredicate ToPredicate()
        {
            return Predicates.Field<TView>(x => x.Id, Operator.Eq, this.Id);
        }
    }
}
