using ASI.TCL.CMFT.Domain;
using ASI.TCL.CMFT.Domain.SYS;
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
                this.HandleUpdate<User, UserId>(_aggregateStore, _unitOfWork, cmd.Id, async user =>
                {
                    if (!string.IsNullOrWhiteSpace(cmd.NewName))
                        user.Rename(cmd.NewName);

                    if (!string.IsNullOrWhiteSpace(cmd.NewDescription))
                        user.ChangeDescription(cmd.NewDescription);

                    if (cmd.BelongRoleId != Guid.Empty)
                    {
                        user.ChangeRole(new RoleId(cmd.BelongRoleId));

                        // 載入新的角色（跨聚合查詢）
                        var role = await _aggregateStore.Load<Role, RoleId>(cmd.BelongRoleId);
                        user.LoadRole(role);
                    }
                }),
            Commands.DeleteUser cmd => this.HandleDelete<User, UserId>(_aggregateStore, _unitOfWork, cmd.Id),

            _ => Task.FromException(new ArgumentException($"未知命令型別: {command.GetType().Name}"))
        };

        private async Task HandleCreateUser(Commands.CreateUser cmd)
        {
            var role = await _aggregateStore.Load<Role, RoleId>(cmd.BelongRoleId);
            var user = new User(cmd.Id, cmd.Name, cmd.Description, "123", role);
            await this.HandleCreate<User, UserId>(_aggregateStore, _unitOfWork, user);
        }
    }
}
