namespace ASI.TCL.CMFT.Application
{
    public class OperatorContext
    {
        public string UserId { get; }
        public string UserName { get; }
        public List<string> Authorities { get; }

        public OperatorContext(string userId, string userName, List<string> authorities)
        {
            UserId = userId;
            UserName = userName;
            Authorities = authorities ?? new List<string>();
        }

        public bool HasAuthority(string code) => Authorities.Contains(code);
    }
}