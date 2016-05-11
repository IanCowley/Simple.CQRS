using Simple.CQRS.Command;
using Simple.CQRS.Domain;
using Simple.CQRS.Exceptions;
using Simple.CQRS.Extensions;

namespace Simple.CQRS.TestInfrastructure
{
    public class TestCommandDispatcher : ICommandDispatcher
    {
        private readonly IDbContextFactory dbContextFactory;

        public TestCommandDispatcher(IDbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public void DispatchCommand<TCommand, TAggregate>(TCommand command, IHandleCommandWithoutResult<TCommand, TAggregate> commandHandler)
            where TCommand : class, ICommandWithoutResult<TAggregate>
            where TAggregate : AggregateRoot
        {
            var aggregate = GetAggregate(command);
            aggregate.Id = command.AggregateId;
            commandHandler.HandleWithoutResult(command, aggregate);
            aggregate.Id = command.AggregateId;
        }

        public CommandResult DispatchCommand<TCommand, TAggregate>(TCommand command, IHandleCommandWithResult<TCommand, TAggregate> commandHandler)
            where TCommand : class, ICommandWithResult<TAggregate>
            where TAggregate : AggregateRoot
        {
            var aggregate = GetAggregate(command);
            aggregate.Id = command.AggregateId;
            CommandResult result = commandHandler.HandleWithResult(command, aggregate);
            aggregate.Id = command.AggregateId;
            return result;
        }

        private TAggregate GetAggregate<TAggregate>(ICommand<TAggregate> command)
            where TAggregate : AggregateRoot
        {
            var context = this.dbContextFactory.GetContext();
            TAggregate aggregate;

            if (command is ICreatesAggregate)
            {
                aggregate = context.CreateNew<TAggregate>();
                aggregate.Id = command.AggregateId;
            }
            else
            {
                aggregate = context.GetAggregateRoot<TAggregate>(command.AggregateId);
            }

            if (aggregate == null)
            {
                throw new CommandException("Couldn't find aggregate {0}".FormatString(command.AggregateId));
            }

            this.AddEntityContextWrapper(aggregate, context);

            return aggregate;
        }

        private void AddEntityContextWrapper(AggregateRoot aggregate, IDbContext context)
        {
            aggregate.EntityContextWrapper = new EntityContextWrapper(context);
        }
    }
}