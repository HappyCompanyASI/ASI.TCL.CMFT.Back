namespace ASI.TCL.CMFT.Domain.SYS
{
    public class User : AggregateRoot<UserId>
    {
        public static readonly string SuperAdminUserName = "超級管理員";
        public static readonly UserId SuperAdminUserId = "superadmin";
        public static readonly UserId SuperAdminUserPassward = "superadmin";

        public string UserName { get; private set; }
        public string? Description { get; private set; }
        private string PasswordHash { get; set; }
        public RoleId BelongRoleId { get; private set; }
        public SYS.Role Role { get; private set; }
        public bool IsLogin { get; private set; }

        [Obsolete("For EF Core use only.")]
        protected User() { }
        public User(UserId id, string userName, string? description, string passwordHash, SYS.Role role)
        {

            if (role == null)
                throw new ArgumentNullException(nameof(role));

            Apply(new Events.UserCreated(
                id,
                userName,
                passwordHash,
                role.Id,
                description
            ));

            Role = role; // Role 實體注入，不走事件
        }

        public void Rename(string newName) => Apply(new Events.UserRenamed(newName));

        public void ChangeDescription(string? newDescription) => Apply(new Events.DescriptionChanged(newDescription));

        public void ChangeRole(RoleId newRoleId)
        {
            if (newRoleId == null || newRoleId.Value == default)
                throw new ArgumentException("角色 ID 不可為空", nameof(newRoleId));

            if (BelongRoleId == newRoleId)
                return;

            Apply(new Events.RoleChanged(newRoleId));
        }
        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.UserCreated e:
                    Id = e.Id;
                    UserName = e.Name;
                    PasswordHash = e.PasswordHash;
                    BelongRoleId = e.RoleId;
                    Description = e.Description;
                    IsLogin = false;
                    Role = null!;
                    break;

                case Events.UserRenamed e:
                    UserName = e.NewName;
                    break;

                case Events.DescriptionChanged e:
                    Description = e.NewDescription;
                    break;

                case Events.RoleChanged e:
                    BelongRoleId = e.NewRoleId;
                    Role = null!;
                    break;

                default:
                    throw new InvalidOperationException($"無法處理的事件型別：{@event.GetType().Name}");
            }
        }
        protected override void EnsureValidState()
        {
            if (string.IsNullOrWhiteSpace(Id))
                throw new InvalidOperationException("使用者帳號不得為空。");

            if (string.IsNullOrWhiteSpace(UserName))
                throw new InvalidOperationException("使用者名稱不得為空。");

            if (string.IsNullOrWhiteSpace(PasswordHash))
                throw new InvalidOperationException("密碼尚未設定。");

            // 不再檢查 Role == null，因為 Role 是外部載入的，若檢查會在Create的時候遇到 EnsureValidState 拋出錯誤（因為尚未載入 Role 實體
            //if (Role == null)
            //    throw new InvalidOperationException("使用者角色尚未設定。");
        }

        public bool VerifyPassword(string password, Func<string, string, bool> verify)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("密碼不可為空");

            return verify(password, PasswordHash);
        }
        public bool HasAuthority(string code)
            => Role.HasAuthority(code);
        public void SetLoginState(bool isLogin) => IsLogin = isLogin;
        public void LoadRole(SYS.Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role), "角色不可為 null");

            if (role.Id != BelongRoleId)
                throw new InvalidOperationException($"角色不符，User 所屬 RoleId 為 {BelongRoleId}，但欲載入的是 {role.Id}");

            Role = role;
        }
    }
}