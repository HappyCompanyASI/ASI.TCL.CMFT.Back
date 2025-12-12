using ASI.TCL.CMFT.Domain;
using ASI.TCL.CMFT.Messages.SYS;

namespace ASI.TCL.CMFT.Application.SYS
{
    public class ApplicationService : IApplicationService
    {
        private readonly IAggregateStore _aggregateStore;
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationService(IAggregateStore aggregateStore, IUnitOfWork unitOfWork)
        {
            _aggregateStore = aggregateStore;
            _unitOfWork = unitOfWork;
        }
        public Task Handle(object command) => command switch
        {
            Commands.CreateUser cmd => HandleCreateUser(cmd),

            Commands.UpdateUserDetails cmd =>
                HandleUpdate(_aggregateStore, _unitOfWork, cmd.Id, async user =>
                {
                    
                }),
            Commands.DeleteUser cmd => HandleDelete(_aggregateStore, _unitOfWork, cmd.Id),

            _ => Task.FromException(new ArgumentException($"未知命令型別: {command.GetType().Name}"))
        };

        private Task HandleUpdate(IAggregateStore aggregateStore, IUnitOfWork unitOfWork, string cmdId, Action<object> action)
        {
            throw new NotImplementedException();
        }

        private Task HandleDelete(IAggregateStore aggregateStore, IUnitOfWork unitOfWork, string cmdId)
        {
            throw new NotImplementedException();
        }

        private async Task HandleCreateUser(Commands.CreateUser cmd)
        {
            throw new NotImplementedException();
        }
    }
}
