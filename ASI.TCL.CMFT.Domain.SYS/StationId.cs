namespace ASI.TCL.CMFT.Domain.SYS
{
    public class StationId : Value<StationId>
    {
        public string Value { get; }
        public StationId(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
            Value = value;
        }
        public static implicit operator string(StationId self) => self.Value;
        public static implicit operator StationId(string value) => new StationId(value);
        public override string ToString() => Value.ToString();
    }

    // Station Entity（只讀）
}