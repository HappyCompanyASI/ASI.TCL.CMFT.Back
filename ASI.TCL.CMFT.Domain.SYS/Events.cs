namespace ASI.TCL.CMFT.Domain.SYS
{
    public static class Events
    {
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