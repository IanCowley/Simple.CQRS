using System.Collections.Generic;

using Simple.CQRS.Extensions;

namespace Simple.CQRS.TestInfrastructure.DynamicLinqBuilding
{
    public interface IDynamicLinqParameterNameTracker
    {
        string TrackParameterAndGetParameterName(object value);

        IEnumerable<object> GetParameters();
    }

    public class DynamicLinqParameterNameTracker : IDynamicLinqParameterNameTracker
    {
        private int parameterIndex;

        private readonly List<object> parameters;

        public DynamicLinqParameterNameTracker()
        {
            this.parameters = new List<object>();
        }

        public IEnumerable<object> GetParameters()
        {
            return this.parameters;
        }

        public string TrackParameterAndGetParameterName(object value)
        {
            string parameter = "@{0}".FormatString(this.parameterIndex);
            this.parameters.Add(value);
            this.parameterIndex++;
            return parameter;
        }
    }
}
