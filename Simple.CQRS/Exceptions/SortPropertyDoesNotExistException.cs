using System;

using Simple.CQRS.Extensions;

namespace Simple.CQRS.Exceptions
{
    public class SortPropertyDoesNotExistException : Exception
    {
        public SortPropertyDoesNotExistException(Type targetType, string targetProperty)
            : base("Sort property {0} doesn't exist on type {1}".FormatString(targetProperty, targetType))
        {
        }
    }
}
