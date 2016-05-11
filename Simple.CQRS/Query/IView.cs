using System;

namespace Simple.CQRS.Query
{
    public interface IView 
    {
        Guid Id { get; }
    }
}
