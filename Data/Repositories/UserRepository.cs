using Dapper;
using Staffinfo.Divers.Data.Poco;
using Staffinfo.Divers.Data.Repositories.Contracts;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Data.Repositories
{
    public class UserRepository : DapperRepository, IUserRepository
    {
        public UserRepository(string connectionString): base(connectionString)
        {

        }

        public async Task<UserPoco> AddAsync(UserPoco poco)
        {
            var parameters = new
            {
                p_last_name = poco.LastName,
                p_first_name = poco.FirstName,
                p_middle_name = poco.MiddleName,
                p_login = poco.Login,
                p_need_to_change_pwd = poco.NeedToChangePwd,
                pwd_hash = poco.PwdHash,
                p_refresh_token = poco.RefreshToken,
                p_token_refresh_timestamp = poco.TokenRefreshTimestamp,
                p_role = poco.Role,
                p_registration_timestamp = poco.RegistrationTimestamp
            };

            var sqlBuilder = new StringBuilder("INSERT into users(");
            sqlBuilder.Append("last_name, first_name, middle_name,");
            sqlBuilder.Append("login, pwd_hash, need_to_change_pwd,");
            sqlBuilder.Append("refresh_token, token_refresh_timestamp, role, registration_timestamp) ");
            sqlBuilder.Append("VALUES(@p_last_name, @p_first_name, @p_middle_name,");
            sqlBuilder.Append("@p_login, @pwd_hash, @p_need_to_change_pwd,");
            sqlBuilder.Append("@p_refresh_token, @p_token_refresh_timestamp, @p_role, @p_registration_timestamp) returning *; ");

            using (IDbConnection conn = Connection)
            {
                var addedDiverPoco = await conn.QueryFirstOrDefaultAsync<UserPoco>(sqlBuilder.ToString(), parameters);

                return addedDiverPoco;
            }
        }

        public async Task DeleteAsync(int userId)
        {
            var parameters = new
            {
                p_user_id = userId
            };

            string sql = "delete from users where user_id = @p_user_id";

            using (IDbConnection conn = Connection)
            {
                await conn.ExecuteAsync(sql, parameters);
            }
        }

        public async Task<UserPoco> GetAsync(int userId)
        {
            var parameters = new
            {
                p_user_id = userId
            };

            string sql = "select * from users where user_id = @p_user_id";

            using (IDbConnection conn = Connection)
            {
                var userPoco = await conn.QueryFirstOrDefaultAsync<UserPoco>(sql, parameters);

                return userPoco;
            }
        }

        public async Task<IEnumerable<UserPoco>> GetListAsync()
        {
            string sql = "select * from users";

            using (IDbConnection conn = Connection)
            {
                var userPocos = await conn.QueryAsync<UserPoco>(sql);

                return userPocos;
            }
        }

        public async Task<UserPoco> UpdateAsync(UserPoco poco)
        {
            var parameters = new
            {
                p_user_id = poco.UserId,
                p_last_name = poco.LastName,
                p_first_name = poco.FirstName,
                p_middle_name = poco.MiddleName,
                p_login = poco.Login,
                p_need_to_change_pwd = poco.NeedToChangePwd,
                p_pwd_hash = poco.PwdHash,
                p_refresh_token = poco.RefreshToken,
                p_token_refresh_timestamp = poco.TokenRefreshTimestamp,
                p_role = poco.Role,
                p_registration_timestamp = poco.RegistrationTimestamp
            };

            var sqlBuilder = new StringBuilder("update users set last_name = @p_last_name, ");
            sqlBuilder.Append("first_name = @p_first_name, middle_name = @p_middle_name, login = @p_login, need_to_change_pwd = @p_need_to_change_pwd::boolean, ");
            sqlBuilder.Append("pwd_hash = @p_pwd_hash, refresh_token = @p_refresh_token, token_refresh_timestamp = @p_token_refresh_timestamp, role = @p_role, registration_timestamp = @p_registration_timestamp ");
            sqlBuilder.Append("where user_id = @p_user_id; ");
            sqlBuilder.Append("select * from users where user_id = @p_user_id;");

            using (IDbConnection conn = Connection)
            {
                var updatedUserPoco = await conn.QueryFirstOrDefaultAsync<UserPoco>(sqlBuilder.ToString(), parameters);

                return updatedUserPoco;
            }
        }
    }
}