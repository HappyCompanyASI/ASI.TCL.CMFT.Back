namespace ASI.TCL.CMFT.Domain.SYS
{
    public class Station : Entity<StationId>
    {
        public string Name { get; private set; } = string.Empty;

        // EF Core 使用
        private Station() { }
        public Station(StationId id, string name, Action<object> applier) : base(applier)
        {
            Id = id;
            Name = name;
        }

        protected override void When(object @event) { /* Station 無行為，略 */ }
    }
}