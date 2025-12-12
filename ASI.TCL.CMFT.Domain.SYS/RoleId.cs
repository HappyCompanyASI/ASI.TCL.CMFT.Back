namespace ASI.TCL.CMFT.Domain.SYS
{
    public class RoleId : Value<RoleId>
    {

        public Guid Value { get; }
        public RoleId(Guid value)
        {
            if (value == default)
                throw new ArgumentNullException(nameof(value), "Role id cannot be empty");
            Value = value;
        }
        public static implicit operator Guid(RoleId self) => self.Value;
        public static implicit operator RoleId(Guid value) => new(value);
        public override string ToString() => Value.ToString();
    }
}