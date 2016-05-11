using System;

namespace Simple.CQRS.Exceptions
{
    public class ConfigurationMappingException : Exception
    {
        public ConfigurationMappingException(string message)
            : base(message)
        {
        }
    }
}
