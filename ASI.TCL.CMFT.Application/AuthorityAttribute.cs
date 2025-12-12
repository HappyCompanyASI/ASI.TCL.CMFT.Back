namespace ASI.TCL.CMFT.Application
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class AuthorityAttribute : Attribute
    {
        public string Code { get; }

        public AuthorityAttribute(string code)
        {
            Code = code;
        }
    }
}