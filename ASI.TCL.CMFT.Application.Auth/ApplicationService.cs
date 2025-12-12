using ASI.TCL.CMFT.Domain;
using ASI.TCL.CMFT.Messages.Auth;

namespace ASI.TCL.CMFT.Application.Auth
{
    public class ApplicationService : IApplicationService
    {
       
        private readonly IAggregateStore _aggregateStore;
        private readonly IUnitOfWork _unitOfWork;
        private readonly  IApplicationEventBus _applicationEventBus;

        public ApplicationService(IAggregateStore store, IUnitOfWork unitOfWork, IApplicationEventBus applicationEventBus)
        {
            _aggregateStore = store;
            _unitOfWork = unitOfWork;
            _applicationEventBus = applicationEventBus;
        }

        public Task Handle(object command) =>
            command switch
            {
            

                _ => throw new ArgumentException($"未知的命令類型: {command.GetType().Name}")
            };

        private async Task HandleLogin(Commands.LoginCommand command)
        {
            //var result = await _loginService.LoginAsync(command.Account, command.Password);
            //_applicationEventBus.Publish(new Notifications.LoginResultApplication(
            //    success: result.Success,
            //    userName: result.UserName,
            //    userRole: result.Role.Name));
        }
        private Task HandleLogout(Commands.LogoutCommand logout)
        {
            //_loginService.Logout();
            //_applicationEventBus.Publish(new Notifications.LogoutResultApplication(true));
            return Task.CompletedTask;
        }
    }
}
