using System.Collections.Generic;
using System.Linq;

namespace Simple.CQRS.Query
{
    public class PagedResultSet<TView> where TView : IView
    {
        public PagedResultSet(IEnumerable<TView> results, QueryContext queryContext, int totalRows, int currentPage, int pageSize)
        {
            this.Results = results.ToList();
            this.QueryContext = queryContext;
            this.TotalRows = totalRows;
            this.PageCount = GetPageCount(totalRows, queryContext);
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
        }

        public List<TView> Results { get; private set; }

        public QueryContext QueryContext { get; private set; }

        public int CurrentPage { get; private set; }

        public int PageSize { get; private set; }

        public int PageCount { get; private set; }

        public int TotalRows { get; private set; }

        private int GetPageCount(int totalRows, QueryContext queryContext)
        {
            if (queryContext == QueryContext.Empty())
            {
                return 0;
            }

            return CalculatePageCount(totalRows, queryContext.ItemsPerPage);
        }

        private int CalculatePageCount(int totalRows, int resultsPerPage)
        {
            return ((totalRows - 1) / resultsPerPage) + 1;
        }
    }
}
