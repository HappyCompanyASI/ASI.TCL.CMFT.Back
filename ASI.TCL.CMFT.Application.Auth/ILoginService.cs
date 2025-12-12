namespace ASI.TCL.CMFT.Application.Auth
{
    public interface ILoginService
    {
        /// <summary>
        /// 嘗試登入使用者帳號密碼
        /// </summary>
        Task<LoginResult> LoginAsync(string account, string password);

        /// <summary>
        /// 登出目前登入的使用者
        /// </summary>
        void Logout();
    }
}