using System;

namespace Simple.CQRS.TestInfrastructure.DynamicLinqBuilding.ExpressionBuilding
{
    public class NoDynamicLinqExpressionBuilderExistForThisPredicateTypeException : Exception
    {
        public NoDynamicLinqExpressionBuilderExistForThisPredicateTypeException(Type prediateType)
            : base("No Dynamic Linq Predicate Adapters Exist For this predicate Type " + prediateType.ToString())
        {
        }
    }
}