using ASI.TCL.CMFT.Domain.SYS;

namespace ASI.TCL.CMFT.Application.Auth
{
    public class LoginResult
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public UserId UserId { get; private set; }
        public string UserName { get; private set; }
        public Role Role { get; private set; }

        private LoginResult(bool success, string message, UserId userId = null, string userName = null, Role role = null)
        {
            Success = success;
            Message = message;
            UserId = userId;
            UserName = userName;
            Role = role;
        }

        public static LoginResult Fail(string message) =>
            new LoginResult(false, message);

        public static LoginResult SuccessResult(UserId userId, string userName, Role role) =>
            new LoginResult(true, "Login succeeded", userId, userName, role);
    }
}