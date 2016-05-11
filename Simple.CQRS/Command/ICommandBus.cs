using Simple.CQRS.Domain;

namespace Simple.CQRS.Command
{
    public interface ICommandBus
    {
        CommandResult SendWithResult<TCommand, TAggregate>(TCommand command)
            where TAggregate : AggregateRoot
            where TCommand : class, ICommandWithResult<TAggregate>;

        void SendWithoutResult<TCommand, TAggregate>(TCommand command)
            where TAggregate : AggregateRoot
            where TCommand : class, ICommandWithoutResult<TAggregate>;
    }
}
