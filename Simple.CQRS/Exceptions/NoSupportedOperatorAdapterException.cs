using System;

using DapperExtensions;

namespace Simple.CQRS.TestInfrastructure.DynamicLinqBuilding.ExpressionBuilding
{
    public class NoSupportedOperatorAdapterException : Exception
    {
        public NoSupportedOperatorAdapterException(Operator @operator) : base("There si no configured support for operation " + @operator.GetType())
        {
        }
    }
}