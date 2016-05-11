using DapperExtensions;

namespace Simple.CQRS.TestInfrastructure.DynamicLinqBuilding.ExpressionBuilding
{
    public interface IDynamicLinqExpressionBuilder
    {
        bool Accepts(IPredicate predicate);

        string GetWhereClause(IDynamicLinqParameterNameTracker paramterNameTracker, IPredicate predicate);
    }
}
