namespace ASI.TCL.CMFT.Domain.SYS
{
    public class StationGroupId : Value<StationGroupId>
    {
        public Guid Value { get; }
        public StationGroupId(Guid value)
        {
            if (value == default) throw new ArgumentNullException(nameof(value));
            Value = value;
        }
        public static implicit operator Guid(StationGroupId self) => self.Value;
        public static implicit operator StationGroupId(Guid value) => new StationGroupId(value);
        public override string ToString() => Value.ToString();
    }
}