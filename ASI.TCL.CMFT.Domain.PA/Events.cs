namespace ASI.TCL.CMFT.Domain.PA
{
    public static class Events
    {
        public record VoiceGroupCreated(Guid Id, string GroupName);
        public record VoiceGroupRenamed(string NewName);
        public record VoiceGroupChanged(string VoiceId, Guid? NewGroupId);
    }
}
