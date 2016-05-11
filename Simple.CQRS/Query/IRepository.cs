using System;
using System.Collections.Generic;

using Simple.CQRS.Query.Specification;

namespace Simple.CQRS.Query
{
    public interface IRepository
    {
        TView GetById<TView>(Guid id) where TView : class, IView;

        IEnumerable<TView> Get<TView>() where TView : class, IView;

        IEnumerable<TView> Get<TView>(ISpecification<TView> specification) where TView : class, IView;

        PagedResultSet<TView> Get<TView>(QueryContext queryContext) where TView : class, IView;

        PagedResultSet<TView> Get<TView>(ISpecification<TView> specification, QueryContext queryContext) where TView : class, IView;

        int GetCount<TView>(ISpecification<TView> specification) where TView : class, IView;
    }
}
