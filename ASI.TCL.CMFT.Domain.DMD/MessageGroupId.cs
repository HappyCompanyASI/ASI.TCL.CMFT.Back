namespace ASI.TCL.CMFT.Domain.DMD
{
    public class MessageGroupId : Value<MessageGroupId>
    {
        public Guid Value { get; }

        public MessageGroupId(Guid value)
        {
            if (value == default)
                throw new ArgumentNullException(nameof(value), "MessageGroup ID cannot be empty");
            Value = value;
        }
        public static implicit operator Guid(MessageGroupId id) => id.Value;
        public static implicit operator MessageGroupId(Guid value) => new MessageGroupId(value);
        public static implicit operator MessageGroupId?(Guid? value)
        {
            return value.HasValue && value.Value != Guid.Empty
                ? new MessageGroupId(value.Value)
                : null;
        }
        public override string ToString() => Value.ToString();
    }
}