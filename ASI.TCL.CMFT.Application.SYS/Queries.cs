using ASI.TCL.CMFT.Messages.SYS;

namespace ASI.TCL.CMFT.Application.SYS
{
    public static class Queries
    {
        public static async Task<IEnumerable<ReadModels.Role>> Query(
            this IQueryService connection,
            QueryModels.GetAllRoles query)
        {
            const string sql = @"SELECT 
                                    id, 
                                    name, 
                                    authority_codes AS AuthorityList
                                    FROM dbo.role;";

            var readModels = await connection.QueryAsync<ReadModels.Role>(sql);
            return readModels.ToList();
        }

        public static async Task<IEnumerable<ReadModels.Role>> Query(
            this IQueryService connection,
            QueryModels.GetPagedRoles query)
        {
            string sql = $@"";

            var readModels = await connection.QueryAsync<ReadModels.Role>(sql);
            return readModels.ToList();
        }

        public static async Task<ReadModels.Role> Query(
            this IQueryService connection,
            QueryModels.GetRole query)
        {
            const string sql = @"";

            var readModels = await connection.QueryAsync<ReadModels.Role>(sql);
            return readModels.First();
        }

        public static async Task<IEnumerable<ReadModels.User>> Query(
            this IQueryService connection,
            QueryModels.GetAllUsers query)
        {
            const string sql = @"SELECT 
                                    id,
                                    user_name As Name,
                                    description,
                                    belong_role_id AS BelongRoleId
                                    FROM dbo.user;";

            var readModels = await connection.QueryAsync<ReadModels.User>(sql);
            return readModels.ToList();
        }
    }
}
