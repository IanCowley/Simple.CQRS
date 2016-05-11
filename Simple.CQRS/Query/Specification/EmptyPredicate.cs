using System.Collections.Generic;

using DapperExtensions;
using DapperExtensions.Sql;

namespace Simple.CQRS.Query.Specification
{
    public class EmptyPredicate : IPredicate
    {
        public string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters)
        {
            return "1 = 1";
        }
    }
}
