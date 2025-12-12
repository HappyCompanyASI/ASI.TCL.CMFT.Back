namespace ASI.TCL.CMFT.Domain.DMD
{
    public class MessageGroup : AggregateRoot<MessageGroupId>
    {
        public string GroupName { get; private set; }

        public virtual ICollection<Message> Messages { get; private set; } = new List<Message>();

        [Obsolete("For EF Core use only.")]
        protected MessageGroup() { }

        public MessageGroup(MessageGroupId id, string groupName) =>
            Apply(new Events.MessageGroupCreated(id, groupName));

        public void Rename(string newName) => 
            Apply(new Events.MessageGroupRenamed(newName));

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.MessageGroupCreated e:
                    Id = e.Id;
                    GroupName = e.GroupName;
                    break;

                case Events.MessageGroupRenamed e:
                    GroupName = e.NewName;
                    break;
            }
        }

        protected override void EnsureValidState()
        {
            if (Id == null || Id.Value == default)
                throw new InvalidOperationException("群組 ID 為必填項目。");

            if (string.IsNullOrWhiteSpace(GroupName))
                throw new InvalidOperationException("群組名稱不可為空。");
        }
    }
}