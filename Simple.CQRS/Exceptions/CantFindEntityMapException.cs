using System;

namespace Simple.CQRS.TestInfrastructure
{
    public class CantFindEntityMapException : Exception
    {
        public CantFindEntityMapException(string message)
            : base(message)
        {
            
        }
    }
}