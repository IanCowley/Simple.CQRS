using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

using DapperExtensions;

using Simple.CQRS.Exceptions;
using Simple.CQRS.Query.Specification;

namespace Simple.CQRS.Query
{
    public class DapperRepository : IRepository
    {
        private readonly string connectionString;

        public DapperRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public TView GetById<TView>(Guid id) where TView : class, IView
        {
            return this.GetById(new IdSpecification<TView>(id));
        }

        public IEnumerable<TView> Get<TView>() where TView : class, IView
        {
            using (var cn = GetConnection())
            {
                cn.Open();
                var results = cn.GetList<TView>();
                cn.Close();
                return results;
            }
        }

       
        public IEnumerable<TView> Get<TView>(ISpecification<TView> specification) where TView : class, IView
        {
            using (var cn = GetConnection())
            {
                cn.Open();
                var results = cn.GetList<TView>(specification.ToPredicate());
                cn.Close();
                return results;
            }
        }

        public PagedResultSet<TView> Get<TView>(QueryContext queryContext) where TView : class, IView
        {
            using (var cn = GetConnection())
            {
                cn.Open();
                var results = cn.GetPage<TView>(null, GetSorting(queryContext), queryContext.CurrentPageIndex, queryContext.ItemsPerPage);
                var totalRows = cn.Count<TView>(null);
                cn.Close();
                return new PagedResultSet<TView>(results, queryContext, totalRows, queryContext.CurrentPageIndex, queryContext.ItemsPerPage);
            }
        }

        public PagedResultSet<TView> Get<TView>(ISpecification<TView> specification, QueryContext queryContext) where TView : class, IView
        {
            using (var cn = GetConnection())
            {
                cn.Open();

                IEnumerable<TView> results;
                var predicate = specification.ToPredicate();

                if (queryContext == QueryContext.Empty())
                {
                    results = cn.GetList<TView>(predicate);
                }
                else
                {
                    results = cn.GetPage<TView>(predicate, GetSorting(queryContext), queryContext.CurrentPageIndex, queryContext.ItemsPerPage);
                }

                var totalRows = cn.Count<TView>(predicate);
                cn.Close();
                return new PagedResultSet<TView>(results, queryContext, totalRows, queryContext.CurrentPageIndex, queryContext.ItemsPerPage);
            }
        }

        public int GetCount<TView>(ISpecification<TView> specification) where TView : class, IView
        {
            using (var cn = GetConnection())
            {
                cn.Open();
                var results = cn.Count<TView>(specification.ToPredicate());
                cn.Close();
                return results;
            }
        }

        private TView GetById<TView>(IdSpecification<TView> specification) where TView : class, IView
        {
            using (var cn = GetConnection())
            {
                cn.Open();
                var result = cn.GetList<TView>(specification.ToPredicate()).SingleOrDefault();

                if (result == null)
                {
                    throw new EntityNotFoundException(specification.Id, typeof(TView));
                }

                cn.Close();
                return result;
            }
        }

        private DbConnection GetConnection()
        {
            return new SqlConnection(this.connectionString);
        }

        private IList<ISort> GetSorting(QueryContext queryContext)
        {
            if (queryContext.SortBy == null)
            {
                return new ISort[] { };
            }

            return new ISort[]
                       {
                           new Sort()
                               {
                                   Ascending = queryContext.SortBy.SortDirection == SortDirection.Ascending,
                                   PropertyName = queryContext.SortBy.TargetProperty
                               }
                       };
        }
    }
}
