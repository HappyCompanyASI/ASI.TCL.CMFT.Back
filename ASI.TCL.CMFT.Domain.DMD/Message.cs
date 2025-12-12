namespace ASI.TCL.CMFT.Domain.DMD
{
    public class Message : AggregateRoot<MessageId>
    {
        public string Name { get; private set; }
        public string Content { get; private set; }
        public MessageGroupId? BelongGroupId { get; private set; }
        public bool IsUseDu { get; private set; }
        public virtual MessageGroup Group { get; private set; }

        [Obsolete("For EF Core use only.")]
        protected Message() { }
        public Message(MessageId id, string name, string content, MessageGroupId? groupId, bool isUseDu = false) => 
            Apply(new Events.MessageCreated(id, name, content, groupId?.Value, isUseDu));
        public void Rename(string newName) =>
            Apply(new Events.MessageRenamed(newName));
        public void ChangeContent(string newContent) =>
            Apply(new Events.MessageContentChanged(newContent));
        public void ChangeGroup(MessageGroupId? newGroupId)
        {
            if (BelongGroupId != newGroupId)
                Apply(new Events.MessageGroupChanged(newGroupId?.Value));
        }

        public void ChangeDuState(bool duState) =>
            Apply(new Events.MessageUsageToggled(duState));

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.MessageCreated e:
                    Id = e.Id;
                    Name = e.Name;
                    Content = e.Content;
                    BelongGroupId = e.GroupId;
                    IsUseDu = e.IsUseDu;
                    break;

                case Events.MessageRenamed e:
                    Name = e.NewName;
                    break;

                case Events.MessageContentChanged e:
                    Content = e.NewContent;
                    break;

                case Events.MessageGroupChanged e:
                    BelongGroupId = e.NewGroupId;
                    break;

                case Events.MessageUsageToggled e:
                    IsUseDu = e.IsUseDu;
                    break;
            }
        }
        protected override void EnsureValidState()
        {
            if (Id == null || Id.Value == default)
                throw new InvalidOperationException("訊息 ID 為必填項目。");

            if (string.IsNullOrWhiteSpace(Name))
                throw new InvalidOperationException("訊息名稱不可為空。");

            if (string.IsNullOrWhiteSpace(Content))
                throw new InvalidOperationException("訊息內容不可為空。");
        }
    }
}