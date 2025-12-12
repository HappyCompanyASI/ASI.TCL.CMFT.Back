namespace ASI.TCL.CMFT.Application
{
    /// <summary>
    /// 提供目前登入的使用者（CurrentUser）的資訊，如帳號、名稱、權限等
    /// </summary>
    public interface IOperatorAccessor
    {
        /// <summary>
        /// 目前登入的使用者資訊
        /// </summary>
        OperatorContext Current { get; }

        /// <summary>
        /// 登入成功後設定目前使用者
        /// </summary>
        void SetOperator(string userId, string userName, List<string> authorities);

        /// <summary>
        /// 登出或清除目前使用者
        /// </summary>
        void Clear();
    }

}