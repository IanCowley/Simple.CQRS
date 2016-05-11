using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

using AutoMapper;

using DapperExtensions;

using Simple.CQRS.Domain;
using Simple.CQRS.Exceptions;
using Simple.CQRS.Extensions;
using Simple.CQRS.ModelConfiguration;
using Simple.CQRS.Query;
using Simple.CQRS.Query.Specification;
using Simple.CQRS.TestInfrastructure.DynamicLinqBuilding;

namespace Simple.CQRS.TestInfrastructure
{
    public class FakeRepository : IRepository
    {
        private readonly IDbContext context;

        private readonly IDomainConfigurationStore domainConfigurationStore;

        private readonly IDynamicLinqBuilder dynamicLinqBuilder;

        public FakeRepository(
            IDomainConfigurationStore domainConfigurationStore,
            IDynamicLinqBuilder dynamicLinqBuilder,
            FakeDbContextFactory contextFactory)
        {
            this.context = contextFactory.GetContext();
            this.domainConfigurationStore = domainConfigurationStore;
            this.dynamicLinqBuilder = dynamicLinqBuilder;
        }

        public TView GetById<TView>(Guid id) where TView : class, IView
        {
            return this.GetById(new IdSpecification<TView>(id));
        }

        public IEnumerable<TView> Get<TView>() where TView : class, IView
        {
            return this.GetBySpecification(new EmptySpecification<TView>());
        }

     
        public IEnumerable<TView> Get<TView>(ISpecification<TView> specification) where TView : class, IView
        {
            return this.GetBySpecification<TView>(specification);
        }

        public PagedResultSet<TView> Get<TView>(QueryContext queryContext) where TView : class, IView
        {
            IEnumerable<TView> results = this.ApplySorting(this.Get<TView>(), queryContext);

            var pagedResults = results.Skip(queryContext.CurrentPageIndex * queryContext.ItemsPerPage).Take(queryContext.ItemsPerPage);
            return new PagedResultSet<TView>(pagedResults, queryContext, results.Count(), queryContext.CurrentPageIndex, queryContext.ItemsPerPage);
        }

        public PagedResultSet<TView> Get<TView>(ISpecification<TView> specification, QueryContext queryContext) where TView : class, IView
        {
            IEnumerable<TView> results = ApplySorting(Get(specification), queryContext);

            var pagedResults = results.Skip(queryContext.CurrentPageIndex * queryContext.ItemsPerPage).Take(queryContext.ItemsPerPage);
            return new PagedResultSet<TView>(pagedResults, queryContext, results.Count(), queryContext.CurrentPageIndex, queryContext.ItemsPerPage);
        }

        public int GetCount<TView>(ISpecification<TView> specification) where TView : class, IView
        {
            return Get(specification).Count();
        }

        private TView GetById<TView>(IdSpecification<TView> specification) where TView : class, IView
        {
            var result = GetBySpecification(specification).SingleOrDefault();

            if (result == null)
            {
                throw new EntityNotFoundException(specification.Id, typeof(TView));
            }

            return result;
        }

        private IEnumerable<TView> GetBySpecification<TView>(ISpecification<TView> specification) where TView : class, IView
        {
            return this.GetByPredicate<TView>(specification.ToPredicate());
        }

        private IEnumerable<TView> GetByPredicate<TView>(IPredicate predicate)
            where TView : class, IView
        {
            IEnumerable<IEntity> entityFrameworkEntities = this.GetEntityFrameworkEntities<TView>();
            string whereClause = this.dynamicLinqBuilder.GetWhereClause(predicate);
            var parameters = this.dynamicLinqBuilder.GetParameters().ToArray();
            IEnumerable<TView> views = entityFrameworkEntities.Select(Mapper.Map<TView>).Where(whereClause, parameters).ToList();
            var viewsFiltered = views.Where(whereClause, parameters);
            return viewsFiltered;
        }

        private IEnumerable<IEntity> GetEntityFrameworkEntities<TView>() where TView : class, IView
        {
            var entityMaps = this.domainConfigurationStore.GetEntityMaps<TView>();

            if (!entityMaps.Any())
            {
                throw new CantFindEntityMapException(
                    "Couldn't find entity map for view {0}".FormatString(typeof(TView)));
            }

            return entityMaps.SelectMany(map => map.GetEntities(this.context));
        }

        private IEnumerable<TView> ApplySorting<TView>(IEnumerable<TView> results, QueryContext queryContext) where TView : class, IView
        {
            if (queryContext.SortBy == null)
            {
                return results;
            }

            return results.OrderBy(queryContext.SortBy.ToString());
        }
    }
}