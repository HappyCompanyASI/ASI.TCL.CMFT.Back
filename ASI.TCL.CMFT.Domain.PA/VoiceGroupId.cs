namespace ASI.TCL.CMFT.Domain.PA
{
    public class VoiceGroupId : Value<VoiceGroupId>
    {
        public Guid Value { get; }
        public VoiceGroupId(Guid value)
        {
            if (value == default)
                throw new ArgumentNullException(nameof(value), "VoiceGroup ID cannot be empty");
            Value = value;
        }
        public static implicit operator Guid(VoiceGroupId id) => id.Value;
        public static implicit operator VoiceGroupId(Guid value) => new(value);
        public static implicit operator VoiceGroupId?(Guid? value)
        {
            return value.HasValue && value.Value != Guid.Empty
                ? new VoiceGroupId(value.Value)
                : null;
        }
        public override string ToString() => Value.ToString();
    }
}