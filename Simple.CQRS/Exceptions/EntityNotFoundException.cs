using System;

using Simple.CQRS.Extensions;

namespace Simple.CQRS.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(Guid id, Type type)
            : base("Entity not found id {0} and type {1}".FormatString(id, type))
        {

        }

        public EntityNotFoundException(string message)
            : base(message)
        {

        }
    }
}
