using System.ComponentModel.DataAnnotations.Schema;

namespace ASI.TCL.CMFT.Domain.SYS
{
    public class Role : AggregateRoot<RoleId>
    {
        public static readonly string SuperAdminRoleName = "超級角色";
        public static readonly RoleId SuperAdminRoleId = Guid.Parse("00000000-0000-0000-0000-111111111111");

        public string Name { get; private set; }
        public string AuthorityCodes { get; private set; }

        [NotMapped]
        public IReadOnlyCollection<Authority> Authorities =>
            AuthorityCodes?.Split(',').Select(code => new Authority(code)).ToList() ?? new List<Authority>();
        public List<string> AuthorityStringList => Authorities.Select(a => a.Code).ToList();

        [Obsolete("For EF Core use only.")]
        protected Role() { }

        public Role(RoleId id, string name, IEnumerable<Authority> authorities)
        {
            if (id == null || id.Value == default)
                throw new ArgumentNullException(nameof(id), "角色 ID 不可為空");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("角色名稱不可為空", nameof(name));

            Id = id;
            Name = name;
            SetAuthorities(authorities ?? new List<Authority>());
        }

        public bool HasAuthority(string code)
            => Authorities.Any(a => a.Code == code);

        public void SetAuthorities(IEnumerable<Authority> authorities)
        {
            AuthorityCodes = string.Join(",", authorities.Select(a => a.Code));
        }

        protected override void When(object @event)
        {
            // 若日後加入事件，可處理還原邏輯
        }

        protected override void EnsureValidState()
        {
            if (Id == null || Id.Value == default)
                throw new InvalidOperationException("角色 ID 不可為空。");

            if (string.IsNullOrWhiteSpace(Name))
                throw new InvalidOperationException("角色名稱不可為空。");
        }
    }
}