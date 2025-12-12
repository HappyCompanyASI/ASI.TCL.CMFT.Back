namespace ASI.TCL.CMFT.Domain.SYS
{
    public static class Events
    {
        public record UserRenamed(string NewName);
        public record DescriptionChanged(string? NewDescription);
        public record RoleChanged(RoleId NewRoleId);
        public record UserCreated(UserId Id, string Name, string PasswordHash, RoleId RoleId, string? Description);

        public class StationGroupCreated
        {
            public StationGroupId Id { get; set; } = null!;
            public string Name { get; set; } = string.Empty;
        }

        public class StationAddedToGroup
        {
            public StationGroupId GroupId { get; set; } = null!;
            public StationId StationId { get; set; } = null!;
            public DateTime JoinedAt { get; set; }
        }

        public class StationRemovedFromGroup
        {
            public StationGroupId GroupId { get; set; } = null!;
            public StationId StationId { get; set; } = null!;
        }
    }
}