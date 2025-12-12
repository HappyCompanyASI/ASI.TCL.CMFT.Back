using ASI.TCL.CMFT.Messages.DMD;

namespace ASI.TCL.CMFT.Application.DMD
{
    public static class Queries
    {
        public static async Task<IEnumerable<ReadModels.MessageGroup>> Query(
            this IQueryService queryService,
            QueryModels.GetAllPreRecordMessageGroup query)
        {
            const string sql = @"SELECT 
                                    id, 
                                    group_name AS GroupName 
                                    FROM dbo.message_group;";
            return await queryService.QueryAsync<ReadModels.MessageGroup>(sql);
        }
        public static async Task<IEnumerable<ReadModels.PreRecordMessage>> Query(
            this IQueryService queryService,
            QueryModels.GetAllPreRecordMessage query)
        {
            const string sql = @"SELECT 
                                    id, 
                                    name AS MessageName,
                                    content AS MessageContent,
                                    belong_group_id AS BelongGroupId,
                                    is_use_du AS IsUseDU 
                                    FROM dbo.message;";
            return await queryService.QueryAsync<ReadModels.PreRecordMessage>(sql);
        }
    }
}