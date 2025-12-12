namespace ASI.TCL.CMFT.Domain.PA
{
    public class Voice : AggregateRoot<VoiceId>
    {
        public string Name { get; private set; }
        public string Content { get; private set; }
        public bool IsChn { get; private set; }
        public bool IsTwn { get; private set; }
        public bool IsHakka { get; private set; }
        public bool IsEng { get; private set; }
        public VoiceGroupId? BelongGroupId { get; private set; }

        [Obsolete("For EF Core use only.")]
        protected Voice() { }
        public Voice(VoiceId id, string name, string content, bool isCHN, bool isTWN, bool isHAKKA, bool isENG)  
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Content = content ?? throw new ArgumentNullException(nameof(content));
            IsChn = isCHN;
            IsTwn = isTWN;
            IsHakka = isHAKKA;
            IsEng = isENG;
            BelongGroupId = null; // 預設為未分類
        }
     
        public void ChangeGroup(VoiceGroupId? newGroupId)
        {
            if (BelongGroupId != newGroupId)
                Apply(new Events.VoiceGroupChanged(Id.Value, newGroupId?.Value));
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.VoiceGroupChanged e:
                    BelongGroupId = e.NewGroupId;
                    break;
            }
        }
        protected override void EnsureValidState()
        {
        }
    }
}
