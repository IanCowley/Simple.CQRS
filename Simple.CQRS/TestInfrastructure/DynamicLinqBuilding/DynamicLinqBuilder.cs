using System.Collections.Generic;
using System.Linq;

using DapperExtensions;

using Simple.CQRS.TestInfrastructure.DynamicLinqBuilding.ExpressionBuilding;

namespace Simple.CQRS.TestInfrastructure.DynamicLinqBuilding
{
    public interface IDynamicLinqBuilder
    {
        string GetWhereClause(IPredicate predicate);

        IEnumerable<object> GetParameters();
    }

    public class DynamicLinqBuilder : IDynamicLinqBuilder
    {
        private readonly IDynamicLinqExpressionBuilder[] expressionBuilders;

        private readonly IDynamicLinqParameterNameTracker paramterNameTracker;

        public DynamicLinqBuilder(IDynamicLinqExpressionBuilder[] expressionBuilders, IDynamicLinqParameterNameTracker paramterNameTracker)
        {
            this.expressionBuilders = expressionBuilders;
            this.paramterNameTracker = paramterNameTracker;
        }

        public IEnumerable<object> GetParameters()
        {
            return this.paramterNameTracker.GetParameters();
        }

        public string GetWhereClause(IPredicate predicate)
        {
            IDynamicLinqExpressionBuilder expressionBuilder = this.expressionBuilders.FirstOrDefault(x => x.Accepts(predicate));

            if (expressionBuilder == null)
            {
                throw new NoDynamicLinqExpressionBuilderExistForThisPredicateTypeException(predicate.GetType());
            }

            return expressionBuilder.GetWhereClause(this.paramterNameTracker, predicate);
        }
    }
}
