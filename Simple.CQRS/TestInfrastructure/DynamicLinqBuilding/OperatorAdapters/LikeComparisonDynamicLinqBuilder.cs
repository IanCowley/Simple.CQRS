using DapperExtensions;

using Simple.CQRS.Extensions;

namespace Simple.CQRS.TestInfrastructure.DynamicLinqBuilding.OperatorAdapters
{
    public class LikeComparisonDynamicLinqBuilder : IDynamicLinqOperatorAdapter
    {
        public bool Accepts(Operator @operator, object value)
        {
            return @operator == Operator.Like && (value.GetType().IsValueType || value is string);
        }

        public string GetLinqStatement(string propertyName, Operator @operator, bool not, string parameterName)
        {
            var notToString = not ? "!" : string.Empty;
            return " {0}{1}.ToLower().Contains({2}.Replace(\"%\", \"\").ToLower()) ".FormatString(notToString, propertyName, parameterName);
        }
    }
}