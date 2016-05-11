using System;

namespace Simple.CQRS.Exceptions
{
    public class CommandException : Exception
    {
        public CommandException(string message)
            : base(message)
        {

        }
    }
}
