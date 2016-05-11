using System;

using Simple.CQRS.Domain;

namespace Simple.CQRS.Command
{
    public interface ICommand<TAggregate> where TAggregate : AggregateRoot
    {
        Guid AggregateId { get; }
    }

    public interface ICreatesAggregate
    {
    }

    public interface ICommandWithoutResult<TAggregate> : ICommand<TAggregate> where TAggregate : AggregateRoot
    {
    }

    public interface ICommandWithResult<TAggregate> : ICommand<TAggregate> where TAggregate : AggregateRoot
    {
    }

    public interface ICreateCommandWithoutResult<TAggregate> : ICommandWithoutResult<TAggregate>, ICreatesAggregate
        where TAggregate : AggregateRoot
    {
    }

    public interface IUpdateCommandWithoutResult<TAggregate> : ICommandWithoutResult<TAggregate>
        where TAggregate : AggregateRoot
    {
    }

    public interface ICreateCommandWithResult<TAggregate> : ICommandWithResult<TAggregate>, ICreatesAggregate
       where TAggregate : AggregateRoot
    {
    }

    public interface IUpdateCommandWithResult<TAggregate> : ICommandWithResult<TAggregate>
        where TAggregate : AggregateRoot
    {
    }
}
