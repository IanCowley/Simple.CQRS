using System;

namespace Simple.CQRS.Domain
{
    public interface IAggregate : IEntity
    {
        new Guid Id { get; set; }
    }
}
