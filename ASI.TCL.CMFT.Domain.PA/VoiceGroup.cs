namespace ASI.TCL.CMFT.Domain.PA
{
    public class VoiceGroup : AggregateRoot<VoiceGroupId>
    {
        public string GroupName { get; private set; }
        public virtual ICollection<Voice> Voices { get; private set; } = new List<Voice>();

        [Obsolete("For EF Core use only.")]
        protected VoiceGroup() { }
        public VoiceGroup(VoiceGroupId id, string groupName) => 
            Apply(new Events.VoiceGroupCreated(id,groupName));
        public void Rename(string newName) =>
            Apply(new Events.VoiceGroupRenamed(newName));
        public void DetachAllVoices()
        {
            foreach (var voice in Voices.ToList())
            {
                voice.ChangeGroup(null);
            }
        }
       
        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.VoiceGroupCreated e:
                    Id = e.Id;
                    GroupName = e.GroupName;
                    break;

                case Events.VoiceGroupRenamed e:
                    GroupName = e.NewName;
                    break;
            }
        }
        protected override void EnsureValidState()
        {
            if (Id == null || Id.Value == default)
                throw new InvalidOperationException("群組 ID 為必填欄位。");

            if (string.IsNullOrWhiteSpace(GroupName))
                throw new InvalidOperationException("群組名稱不能為空白。");
        }
    }
}