using System.Linq;

using Simple.CQRS.Domain;
using Simple.CQRS.Exceptions;
using Simple.CQRS.Extensions;

namespace Simple.CQRS.Command
{
    public interface ICommandHandlerFactory
    {
        THandlerType GetHandlerWithResult<TCommand, TAggregate, THandlerType>()
            where TAggregate : AggregateRoot
            where TCommand : class, ICommandWithResult<TAggregate>
            where THandlerType : IHandleCommandWithResult<TCommand, TAggregate>;

        THandlerType GetHandler<TCommand, TAggregate, THandlerType>()
            where TAggregate : AggregateRoot
            where TCommand : class, ICommandWithoutResult<TAggregate>
            where THandlerType : IHandleCommandWithoutResult<TCommand, TAggregate>;
    }

    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IHandleCommandWithoutResult[] commandHandlers;

        public CommandHandlerFactory(IHandleCommandWithoutResult[] commandHandlers)
        {
            this.commandHandlers = commandHandlers;
        }

        public THandlerType GetHandlerWithResult<TCommand, TAggregate, THandlerType>()
            where TAggregate : AggregateRoot
            where TCommand : class, ICommandWithResult<TAggregate>
            where THandlerType : IHandleCommandWithResult<TCommand, TAggregate>
        {
            var handler = (THandlerType)this.commandHandlers.SingleOrDefault(x => x is THandlerType);

            if (handler == null)
            {
                throw new CommandException("Couldn't find a command handler with result for THandlerType {0}.  As this is a Handler with Result, it could that your ".FormatString(typeof(THandlerType)
                                           + " command handler is implementing a void version instead of result version"));
            }

            return handler;
        }

        public THandlerType GetHandler<TCommand, TAggregate, THandlerType>()
            where TAggregate : AggregateRoot
            where TCommand : class, ICommandWithoutResult<TAggregate>
            where THandlerType : IHandleCommandWithoutResult<TCommand, TAggregate>
        {
            var handler = (THandlerType)this.commandHandlers.SingleOrDefault(x => x is THandlerType);

            if (handler == null)
            {
                throw new CommandException("Couldn't find a command handler for THandlerType {0}".FormatString(typeof(THandlerType)));
            }

            return handler;
        }
    }
}
