using System;
using System.Linq;

using DapperExtensions;

using Simple.CQRS.TestInfrastructure.DynamicLinqBuilding.OperatorAdapters;

namespace Simple.CQRS.TestInfrastructure.DynamicLinqBuilding.ExpressionBuilding
{
    public interface IFieldPredicateExpressionBuilder
    {
        string GetWhereClause(IDynamicLinqParameterNameTracker paramterNameTracker, IPredicate predicate);
    }

    public class FieldPredicateExpressionBuilder : IDynamicLinqExpressionBuilder, IFieldPredicateExpressionBuilder
    {
        private readonly IDynamicLinqOperatorAdapter[] operatorAdapters;

        public FieldPredicateExpressionBuilder(IDynamicLinqOperatorAdapter[] operatorAdapters)
        {
            this.operatorAdapters = operatorAdapters;
        }

        public bool Accepts(IPredicate predicate)
        {
            return predicate is IFieldPredicate;
        }

        public string GetWhereClause(IDynamicLinqParameterNameTracker paramterNameTracker, IPredicate predicate)
        {
            var fieldPredicate = predicate as IFieldPredicate;

            if (fieldPredicate == null)
            {
                throw new InvalidOperationException("Predicate must be a field predicate");
            }

            return
               this.GetWhereBuilder(fieldPredicate)
                   .GetLinqStatement(this.GetPropertyName(fieldPredicate), fieldPredicate.Operator, fieldPredicate.Not, paramterNameTracker.TrackParameterAndGetParameterName(fieldPredicate.Value));
        }

        private string GetPropertyName(IFieldPredicate fieldPredicate)
        {
            return fieldPredicate.PropertyName;
        }

        private IDynamicLinqOperatorAdapter GetWhereBuilder(IFieldPredicate predicate)
        {
            if (this.operatorAdapters.All(x => !x.Accepts(predicate.Operator, predicate.Value)))
            {
                throw new NoSupportedOperatorAdapterException(predicate.Operator);
            }

            var operatorAdapter = this.operatorAdapters.First(x => x.Accepts(predicate.Operator, predicate.Value));

            return operatorAdapter;
        }
    }
}