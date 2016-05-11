using System;

using DapperExtensions;

using Simple.CQRS.Extensions;

namespace Simple.CQRS.TestInfrastructure.DynamicLinqBuilding.ExpressionBuilding
{
    public class CompoundPredicateExpressionBuilder : IDynamicLinqExpressionBuilder
    {
        private readonly IFieldPredicateExpressionBuilder fieldPredicateExpressionBuilder;

        public CompoundPredicateExpressionBuilder(IFieldPredicateExpressionBuilder fieldPredicateExpressionBuilder)
        {
            this.fieldPredicateExpressionBuilder = fieldPredicateExpressionBuilder;
        }

        public bool Accepts(IPredicate predicate)
        {
            return predicate is IPredicateGroup;
        }

        public string GetWhereClause(IDynamicLinqParameterNameTracker paramterNameTracker, IPredicate predicate)
        {
            var groupPredicate = predicate as IPredicateGroup;
            if (groupPredicate == null)
            {
                throw new InvalidOperationException("Predicate must be a field predicate");
            }

            string whereClause = this.GetGroupPredicateWhere(paramterNameTracker, groupPredicate);
            return whereClause;
        }

        private string GetGroupPredicateWhere(
            IDynamicLinqParameterNameTracker paramterNameTracker,
            IPredicateGroup groupPredicate)
        {
            string whereClause = string.Empty;

            groupPredicate
                .Predicates
                .ForEach(predicate =>
                    {
                        if (whereClause != string.Empty)
                        {
                            whereClause += " {0} ".FormatString(groupPredicate.Operator.ToString());
                        }

                        if (predicate is IPredicateGroup)
                        {
                            whereClause += " ({0}) ".FormatString(this.GetGroupPredicateWhere(paramterNameTracker, predicate as IPredicateGroup));
                        }
                        else
                        {
                            whereClause += this.fieldPredicateExpressionBuilder.GetWhereClause(paramterNameTracker, predicate);   
                        }
                    });
            return whereClause;
        }
    }
}
