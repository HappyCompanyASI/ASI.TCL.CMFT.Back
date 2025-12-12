using ASI.TCL.CMFT.Domain;
using ASI.TCL.CMFT.Domain.DMD;
using ASI.TCL.CMFT.Messages.DMD;

namespace ASI.TCL.CMFT.Application.DMD
{
    public class ApplicationService : IApplicationService
    {
        private readonly IAggregateStore _aggregateStore;
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationService(IAggregateStore store, IUnitOfWork unitOfWork)
        {
            _aggregateStore = store;
            _unitOfWork = unitOfWork;
        }

        public Task Handle(object command)
        {
            return command switch
            {
                Commands.CreateGroup cmd => this.HandleCreate<MessageGroup, MessageGroupId>(_aggregateStore, _unitOfWork, new MessageGroup(cmd.Id, cmd.Name)),
                Commands.ChangeGroupName cmd => this.HandleUpdate<MessageGroup, MessageGroupId>(_aggregateStore, _unitOfWork, cmd.Id, group => group.Rename(cmd.NewName)),
                Commands.DeleteGroupWithMessages cmd => this.HandleDelete<MessageGroup, MessageGroupId>(_aggregateStore, _unitOfWork, cmd.Id, async group =>
                {
                    foreach (var message in group.Messages.ToList())
                    {
                        await _aggregateStore.Remove<Message, MessageId>(message);
                    }
                }),
                Commands.DeleteGroupAndDetachMessages cmd => this.HandleDelete<MessageGroup, MessageGroupId>(_aggregateStore, _unitOfWork, cmd.Id, async group =>
                {
                    foreach (var message in group.Messages.ToList())
                    {
                        message.ChangeGroup(null);
                        await _aggregateStore.Save<Message, MessageId>(message);
                    }
                }),
                Commands.CreateMessage cmd => this.HandleCreate<Message, MessageId>(_aggregateStore, _unitOfWork, new Message(cmd.Id, cmd.Name, cmd.Content, cmd.BelongGroupId, cmd.IsUseDu)),
                Commands.ChangeMessageName cmd => this.HandleUpdate<Message, MessageId>(_aggregateStore, _unitOfWork, cmd.Id, message => message.Rename(cmd.NewName)),
                Commands.ChangeMessageContent cmd => this.HandleUpdate<Message, MessageId>(_aggregateStore, _unitOfWork, cmd.Id, message => message.ChangeContent(cmd.NewContent)),
                Commands.ChangeMessageDUState cmd => this.HandleUpdate<Message, MessageId>(_aggregateStore, _unitOfWork, cmd.Id, message => message.ChangeDuState(cmd.NewDUState)),
                Commands.ChangeMessageBelongGroup cmd => this.HandleUpdate<Message, MessageId>(_aggregateStore, _unitOfWork, cmd.Id, message => message.ChangeGroup(cmd.NewBelongGroupId)),
                Commands.UpdateMessageDetails cmd => this.HandleUpdate<Message, MessageId>(_aggregateStore, _unitOfWork, cmd.Id, message =>
                {
                    if (!string.IsNullOrWhiteSpace(cmd.NewName)) message.Rename(cmd.NewName);
                    if (!string.IsNullOrWhiteSpace(cmd.NewContent)) message.ChangeContent(cmd.NewContent);
                    message.ChangeDuState(cmd.NewDUState);
                    if (cmd.NewBelongGroupId != null) message.ChangeGroup(cmd.NewBelongGroupId);
                }),
                Commands.DeleteMessage cmd => this.HandleDelete<Message, MessageId>(_aggregateStore, _unitOfWork, cmd.Id),
                _ => Task.FromException(new ArgumentException($"未知命令型別: {command.GetType().Name}"))
            };
        }
    }
}