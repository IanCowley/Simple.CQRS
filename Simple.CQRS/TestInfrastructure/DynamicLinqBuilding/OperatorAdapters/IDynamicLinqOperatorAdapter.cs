using DapperExtensions;

namespace Simple.CQRS.TestInfrastructure.DynamicLinqBuilding.OperatorAdapters
{
    public interface IDynamicLinqOperatorAdapter
    {
        bool Accepts(Operator @operator, object value);

        string GetLinqStatement(string propertyName, Operator @operator, bool not, string parameterName);
    }
}