namespace Simple.CQRS.Query
{
    public class QueryContext
    {
        public const int DefaultItemsPerPage = 10;

        public const int MaxItemsInResultSet = 9000;

        private bool isEmpty = false;

        public QueryContext()
        {
            this.ItemsPerPage = QueryContext.DefaultItemsPerPage;
        }
        public QueryContext(int pageIndex, ISortBy sortBy, int itemsPerPage = QueryContext.DefaultItemsPerPage)
            : this(pageIndex, itemsPerPage)
        {
            this.SortBy = sortBy;
        }

        public QueryContext(int pageIndex, int itemsPerPage = QueryContext.DefaultItemsPerPage)
            : this()
        {
            this.CurrentPageIndex = pageIndex;
            this.ItemsPerPage = itemsPerPage;
        }

        public int CurrentPageIndex { get; set; }

        public int ItemsPerPage { get; set; }

        public ISortBy SortBy { get; set; }

        public static QueryContext Empty()
        {
            return new QueryContext()
            {
                isEmpty = true
            };
        }

        public static bool operator ==(QueryContext x, QueryContext y)
        {
            return AreEqual(x, y);
        }

        public static bool operator !=(QueryContext x, QueryContext y)
        {
            return !AreEqual(x, y);
        }

        public override bool Equals(object obj)
        {
            var otherContext = obj as QueryContext;

            if (otherContext != null)
            {
                return AreEqual(this, otherContext);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hash = 23;
            hash = hash * 37 + this.CurrentPageIndex;
            hash = hash * 37 + this.ItemsPerPage;
            hash = hash * 37 + this.isEmpty.GetHashCode();
            return hash;
        }

        private static bool AreEqual(QueryContext x, QueryContext y)
        {
            if ((object)x == null || (object)y == null)
            {
                return NullCheckComparison(x, y);
            }

            return x.isEmpty == y.isEmpty &&
                   x.CurrentPageIndex == y.CurrentPageIndex &&
                   x.ItemsPerPage == y.ItemsPerPage;
        }

        private static bool NullCheckComparison(QueryContext x, QueryContext y)
        {
            return (object)x == null && (object)y == null;
        }
    }
}
