namespace ASI.TCL.CMFT.Application.Auth
{
    public static class Notifications
    {
        public class LoginResultApplication : IApplicationEvent
        {
            public bool Success { get; }
            public string UserName { get; }
            public string UserRole { get; }

            public LoginResultApplication(bool success,  string userName, string userRole)
            {
                Success = success;
                UserName = userName;
                UserRole = userRole;
            }
        }

        public class LogoutResultApplication : IApplicationEvent
        {
            public bool Success { get; }
            
            public LogoutResultApplication(bool success)
            {
                Success = success;
            }
        }
    }
}
