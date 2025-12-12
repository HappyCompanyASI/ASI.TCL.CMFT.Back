namespace ASI.TCL.CMFT.Domain.SYS
{
    public class StationGroupMembershipId : Value<StationGroupMembershipId>
    {
        public Guid Value { get; }

        public StationGroupMembershipId(Guid value)
    {
        if (value == default) throw new ArgumentNullException(nameof(value));
        Value = value;
    }

        public static implicit operator Guid(StationGroupMembershipId id) => id.Value;
        public static implicit operator StationGroupMembershipId(Guid value) => new StationGroupMembershipId(value);

        public override string ToString() => Value.ToString();
    }
}