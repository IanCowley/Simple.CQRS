using Simple.CQRS.Domain;

namespace Simple.CQRS.Command
{
    public interface IHandleCommandWithoutResult
    {
    }

    public interface IHandleCommandWithoutResult<TCommand, TAggregate> : IHandleCommandWithoutResult
        where TCommand : class,  ICommandWithoutResult<TAggregate>
        where TAggregate : AggregateRoot
    {
        void HandleWithoutResult(TCommand command, TAggregate aggregate);
    }

    public interface IHandleCommandWithResult<TCommand, TAggregate> : IHandleCommandWithoutResult
        where TCommand : class, ICommandWithResult<TAggregate>
        where TAggregate : AggregateRoot
    {
        CommandResult HandleWithResult(TCommand command, TAggregate aggregate);
    }
}
