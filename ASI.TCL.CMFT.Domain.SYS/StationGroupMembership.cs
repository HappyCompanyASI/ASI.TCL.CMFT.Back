namespace ASI.TCL.CMFT.Domain.SYS
{
    public class StationGroupMembership : Entity<StationGroupMembershipId>
    {
        public StationId StationId { get; private set; }
        public StationGroupId GroupId { get; private set; }
        public DateTime JoinedAt { get; private set; }

        private StationGroupMembership() { }
        public StationGroupMembership(StationId stationId, StationGroupId groupId, DateTime joinedAt)
            : base(_ => { }) // 不會產生事件
        {
            Id = Guid.NewGuid();
            StationId = stationId;
            GroupId = groupId;
            JoinedAt = joinedAt;
        }

        protected override void When(object @event) { /* Membership 為純資料，不含內部狀態轉換 */ }
    }
}