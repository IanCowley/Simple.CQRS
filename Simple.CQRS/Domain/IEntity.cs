using System;

namespace Simple.CQRS.Domain
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
