using Simple.CQRS.Domain;
using Simple.CQRS.Exceptions;
using Simple.CQRS.Extensions;

namespace Simple.CQRS.Command
{
    public class InMemoryCommandBus : ICommandBus
    {
        private readonly ICommandDispatcher commandDispatcher;

        private readonly ICommandHandlerFactory commandHandlerFactory;

        public InMemoryCommandBus(ICommandDispatcher commandDispatcher, ICommandHandlerFactory commandHandlerFactory)
        {
            this.commandDispatcher = commandDispatcher;
            this.commandHandlerFactory = commandHandlerFactory;
        }

        public CommandResult SendWithResult<TCommand, TAggregate>(TCommand command)
            where TAggregate : AggregateRoot
            where TCommand : class, ICommandWithResult<TAggregate>
        {
            var handler =
                commandHandlerFactory.GetHandlerWithResult<TCommand, TAggregate, IHandleCommandWithResult<TCommand, TAggregate>>();

            if (handler == null)
            {
                throw new CommandException(
                    "A command handler with result doesn't exist for command {0}".FormatString(typeof(TCommand).ToString()));
            }

            return commandDispatcher.DispatchCommand(command, handler);
        }

        public void SendWithoutResult<TCommand, TAggregate>(TCommand command)
            where TAggregate : AggregateRoot
            where TCommand : class, ICommandWithoutResult<TAggregate>
        {
            var handler =
                commandHandlerFactory.GetHandler<TCommand, TAggregate, IHandleCommandWithoutResult<TCommand, TAggregate>>();

            if (handler == null)
            {
                throw new CommandException(
                    "A command handler doesn't exist for command {0}".FormatString(typeof(TCommand).ToString()));
            }

            commandDispatcher.DispatchCommand(command, handler);
        }
    }
}
