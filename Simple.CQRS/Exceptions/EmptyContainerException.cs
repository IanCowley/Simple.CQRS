using System;

namespace Simple.CQRS.Exceptions
{
    public class EmptyContainerException : Exception
    {
        public EmptyContainerException()
            : base("IOC hasn't been initialised as yet, probably haven't run the bootstrapper.")
        {
        }
    }
}
