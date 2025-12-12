namespace ASI.TCL.CMFT.Domain.SYS
{
    public class StationGroup : AggregateRoot<StationGroupId>
    {
        private readonly List<StationGroupMembership> _members = new();
        public IReadOnlyCollection<StationGroupMembership> Members => _members;

        public string Name { get; private set; } = string.Empty;

        private StationGroup() { }
        public StationGroup(StationGroupId id, string name)
        {
            Apply(new Events.StationGroupCreated
            {
                Id = id,
                Name = name
            });
        }

        public void AddStation(Station station)
        {
            if (_members.Any(m => m.StationId == station.Id)) return;
            Apply(new Events.StationAddedToGroup
            {
                GroupId = Id,
                StationId = station.Id,
                JoinedAt = DateTime.UtcNow
            });
        }

        public void RemoveStation(StationId stationId)
        {
            if (_members.All(m => m.StationId != stationId)) return;
            Apply(new Events.StationRemovedFromGroup
            {
                GroupId = Id,
                StationId = stationId
            });
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.StationGroupCreated e:
                    Id = e.Id;
                    Name = e.Name;
                    break;

                case Events.StationAddedToGroup e:
                    _members.Add(new StationGroupMembership(e.StationId, e.GroupId, e.JoinedAt));
                    break;

                case Events.StationRemovedFromGroup e:
                    _members.RemoveAll(m => m.StationId == e.StationId);
                    break;
            }
        }

        protected override void EnsureValidState()
        {
            if (Id == null) throw new InvalidOperationException("StationGroup ID 不可為 null");
            if (string.IsNullOrWhiteSpace(Name)) throw new InvalidOperationException("StationGroup Name 不可為空");
        }
    }
}