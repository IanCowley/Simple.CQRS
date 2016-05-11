using System;
using System.Linq;

using DapperExtensions;

using Simple.CQRS.Extensions;

namespace Simple.CQRS.TestInfrastructure.DynamicLinqBuilding.OperatorAdapters
{
    public class StandardComparisonDynamicLinqOperatorBuilder : IDynamicLinqOperatorAdapter
    {
        public virtual bool Accepts(Operator @operator, object value)
        {
            return (@operator == Operator.Eq || @operator == Operator.Gt || @operator == Operator.Lt
                    || @operator == Operator.Ge || @operator == Operator.Le)
                   && (value == null || value.GetType().IsValueType || value is string);
        }

        public virtual string GetLinqStatement(string propertyName, Operator @operator, bool not, string parameterName)
        {
            string comparisonOperator = this.GetComparisonOperator(@operator, not);
            return " {0} {1} {2} ".FormatString(
                propertyName,
                comparisonOperator,
                parameterName);
        }

        protected string GetComparisonOperator(Operator @operator, bool not)
        {
            var operatorsList = new[]
                                    {
                                        new { Operator = Operator.Eq, Linq = "=", LinqNot = "!=" },
                                        new { Operator = Operator.Gt, Linq = ">", LinqNot = "<=" },
                                        new { Operator = Operator.Lt, Linq = "<", LinqNot = ">=" },
                                        new { Operator = Operator.Ge, Linq = ">=", LinqNot = "<" },
                                        new { Operator = Operator.Le, Linq = "<=", LinqNot = ">" }
                                    };

            if (operatorsList.All(x => x.Operator != @operator))
            {
                throw new InvalidOperationException("We don't currently support operator {0} not {1}".FormatString(@operator, not));
            }

            var matchingOperator = operatorsList.First(x => x.Operator == @operator);

            return not ? matchingOperator.LinqNot : matchingOperator.Linq;
        }
    }
}