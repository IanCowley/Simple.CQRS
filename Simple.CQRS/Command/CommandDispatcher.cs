using Simple.CQRS.Domain;
using Simple.CQRS.Exceptions;
using Simple.CQRS.Extensions;

namespace Simple.CQRS.Command
{
    public interface ICommandDispatcher
    {
        void DispatchCommand<TCommand, TAggregate>(
            TCommand command,
            IHandleCommandWithoutResult<TCommand, TAggregate> commandHandler)
            where TCommand : class, ICommandWithoutResult<TAggregate>
            where TAggregate : AggregateRoot;

        CommandResult DispatchCommand<TCommand, TAggregate>(
            TCommand command,
            IHandleCommandWithResult<TCommand, TAggregate> commandHandler)
            where TCommand : class, ICommandWithResult<TAggregate>
            where TAggregate : AggregateRoot;
    }

    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IDbContextFactory dbContextFactory;

        public CommandDispatcher(IDbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public void DispatchCommand<TCommand, TAggregate>(TCommand command, IHandleCommandWithoutResult<TCommand, TAggregate> commandHandler)
            where TCommand : class,  ICommandWithoutResult<TAggregate>
            where TAggregate : AggregateRoot
        {
            using (var context = GetNewContext<TCommand, TAggregate>(command))
            {
                var aggregate = GetOrCreateAggregate(command, context);
                commandHandler.HandleWithoutResult(command, aggregate);
                aggregate.Id = command.AggregateId;
                context.Commit();
            }
        }

        public CommandResult DispatchCommand<TCommand, TAggregate>(TCommand command, IHandleCommandWithResult<TCommand, TAggregate> commandHandler)
            where TCommand : class,  ICommandWithResult<TAggregate>
            where TAggregate : AggregateRoot
        {
            using (var context = GetNewContext<TCommand, TAggregate>(command))
            {
                var aggregate = GetOrCreateAggregate(command, context);
                CommandResult result = commandHandler.HandleWithResult(command, aggregate);

                if (result.WasSuccessful())
                {
                    context.Commit();
                }

                return result;
            }
        }

        public IDbContext GetNewContext<TCommand, TAggregate>(TCommand command)
            where TCommand : class, ICommand<TAggregate>
            where TAggregate : AggregateRoot
        {
            return dbContextFactory.GetContext();
        }

        private TAggregate GetOrCreateAggregate<TAggregate>(ICommand<TAggregate> command, IDbContext context)
            where TAggregate : AggregateRoot
        {
            TAggregate aggregate;

            if (command is ICreatesAggregate)
            {
                aggregate = this.CreateNewAggregate(command, context);
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

      
        private TAggregate CreateNewAggregate<TAggregate>(
            ICommand<TAggregate> command,
            IDbContext context) where TAggregate : AggregateRoot
        {
            var aggregate = context.CreateNew<TAggregate>();
            aggregate.Id = command.AggregateId;
            return aggregate;
        }

        private void AddEntityContextWrapper(AggregateRoot aggregate, IDbContext context)
        {
            aggregate.EntityContextWrapper = new EntityContextWrapper(context);
        }
    }
}
