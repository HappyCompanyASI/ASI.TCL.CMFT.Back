namespace ASI.TCL.CMFT.Domain.DMD
{
    public class MessageId : Value<MessageId>
    {
        public Guid Value { get; }

        public MessageId(Guid value)
        {
            if (value == default)
                throw new ArgumentNullException(nameof(value), "Message ID cannot be empty");
            Value = value;
        }
        public static implicit operator Guid(MessageId id) => id.Value;
        public static implicit operator MessageId(Guid value) => new MessageId(value);
        public override string ToString() => Value.ToString();
    }
}
