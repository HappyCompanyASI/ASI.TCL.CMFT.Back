namespace ASI.TCL.CMFT.Domain.PA
{
    public class VoiceId : Value<VoiceId>
    {
        public string Value { get; }

        public VoiceId(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value), "User id cannot be empty");
        }
        public static implicit operator string(VoiceId id) => id.Value;
        public static implicit operator VoiceId(string value) => new VoiceId(value);
        public override string ToString() => Value.ToString();
    }
}