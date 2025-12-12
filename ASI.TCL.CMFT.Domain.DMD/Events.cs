namespace ASI.TCL.CMFT.Domain.DMD
{
    public static class Events
    {
        public record MessageGroupCreated(Guid Id, string GroupName);
        public record MessageCreated(Guid Id, string Name, string Content, Guid? GroupId, bool IsUseDu);
        public record MessageGroupChanged(Guid? NewGroupId);
        public record MessageGroupRenamed(string NewName);
        public record MessageRenamed(string NewName);
        public record MessageContentChanged(string NewContent);
        public record MessageUsageToggled(bool IsUseDu);
    }
}