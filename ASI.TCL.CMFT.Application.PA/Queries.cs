using ASI.TCL.CMFT.Messages.PA;

namespace ASI.TCL.CMFT.Application.PA
{
    public static class Queries
    {
        public static async Task<IEnumerable<ReadModels.VoiceGroup>> Query(
            this IQueryService queryService,
            QueryModels.GetAllPreRecordVoiceGroup query)
        {
            const string sql = @"SELECT 
                                    id, 
                                    group_name AS GroupName 
                                    FROM dbo.voice_group;";
            return await queryService.QueryAsync<ReadModels.VoiceGroup>(sql);
        }
        public static async Task<IEnumerable<ReadModels.PreRecordVoice>> Query(
            this IQueryService queryService,
            QueryModels.GetAllPreRecordVoice query)
        {
            const string sql = @"SELECT 
                                    id, 
                                    name AS VoiceName,
                                    content AS VoiceContent,
                                    is_chn AS IsIncludeCHN,
                                    is_twn AS IsIncludeTWN,
                                    is_hakka AS IsIncludeHAKKA,
                                    is_eng AS IsIncludeENG,
                                    belong_group_id AS BelongGroupId
                                    FROM dbo.voice;";
            return await queryService.QueryAsync<ReadModels.PreRecordVoice>(sql);
        }
    }
}