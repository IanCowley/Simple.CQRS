using System;

namespace Simple.CQRS.TestInfrastructure
{
    public class TestInfrastructureException : Exception
    {
        public TestInfrastructureException(string message) : base(message)
        {
        }
    }
}