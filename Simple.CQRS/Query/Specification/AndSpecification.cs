using System.Collections.Generic;
using System.Linq;

using DapperExtensions;

namespace Simple.CQRS.Query.Specification
{
    public class AndSpecification<TView> : ISpecification<TView> where TView : class, IView
    {
        private readonly List<ISpecification<TView>> specifications;

        public AndSpecification(params ISpecification<TView>[] specifications)
        {
            this.specifications = specifications.ToList();
        }

        public void AddSpecification(ISpecification<TView> specification)
        {
            this.specifications.Add(specification);
        }

        public IPredicate ToPredicate()
        {
            return new PredicateGroup() { Operator = GroupOperator.And, Predicates = specifications.Select(x => x.ToPredicate()).ToArray() };
        }
    }
}
