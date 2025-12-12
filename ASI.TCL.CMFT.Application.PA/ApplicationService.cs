using ASI.TCL.CMFT.Domain;
using ASI.TCL.CMFT.Domain.PA;
using ASI.TCL.CMFT.Messages.PA;

namespace ASI.TCL.CMFT.Application.PA
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
                Commands.CreateGroup cmd => this.HandleCreate<VoiceGroup, VoiceGroupId>(_aggregateStore, _unitOfWork, new VoiceGroup(cmd.Id, cmd.Name)),
                Commands.ChangeGroupName cmd => this.HandleUpdate<VoiceGroup, VoiceGroupId>(_aggregateStore, _unitOfWork, cmd.Id, group => group.Rename(cmd.NewName)),
                Commands.DeleteGroupAndDetachVoices cmd => this.HandleDelete<VoiceGroup, VoiceGroupId>(_aggregateStore, _unitOfWork, cmd.Id, group => { group.DetachAllVoices(); }),
                Commands.ChangeVoiceBelongGroup cmd => this.HandleUpdate<Voice, VoiceId>(_aggregateStore, _unitOfWork, cmd.Id, voice =>  voice.ChangeGroup( cmd.NewBelongGroupId) ),
               
                _ => Task.FromException(new ArgumentException($"未知命令型別: {command.GetType().Name}"))
            };
        }

        
    }
}