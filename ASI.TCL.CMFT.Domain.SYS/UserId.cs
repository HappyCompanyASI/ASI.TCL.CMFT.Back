namespace ASI.TCL.CMFT.Domain.SYS
{
    public class UserId : Value<UserId>
    {
        public string Value { get; }
        public UserId(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value), "User id cannot be empty");
        }
        public static implicit operator string(UserId self) => self.Value;
        public static implicit operator UserId(string value) => new UserId(value);
        public override string ToString() => Value.ToString();
    }
}