using Dapper;
using Staffinfo.Divers.Data.Poco;
using Staffinfo.Divers.Data.Repositories.Contracts;
using Staffinfo.Divers.Models;
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
                p_registration_timestamp = poco.RegistrationTimestamp,
                p_key = Settings.SecurityKey
            };

            var sqlBuilder = new StringBuilder("INSERT into _staffinfo.users(");
            sqlBuilder.Append("last_name, first_name, middle_name,");
            sqlBuilder.Append("login, pwd_hash, need_to_change_pwd,");
            sqlBuilder.Append("refresh_token, token_refresh_timestamp, role, registration_timestamp) ");
            sqlBuilder.Append("VALUES(encrypt(@p_last_name::bytea, @p_key::bytea, 'aes'), encrypt(@p_first_name::bytea, @p_key::bytea, 'aes'), encrypt(@p_middle_name::bytea, @p_key::bytea, 'aes'),");
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

            string sql = "delete from _staffinfo.users where user_id = @p_user_id";

            using (IDbConnection conn = Connection)
            {
                await conn.ExecuteAsync(sql, parameters);
            }
        }

        public async Task<UserPoco> GetAsync(int userId)
        {
            var parameters = new
            {
                p_user_id = userId,
                p_key = Settings.SecurityKey
            };

            string sql = "select user_id, convert_from(decrypt(last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, " +
                "convert_from(decrypt(first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, " +
                "convert_from(decrypt(middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, login, pwd_hash, need_to_change_pwd, " +
                "refresh_token, token_refresh_timestamp, role, registration_timestamp from _staffinfo.users where user_id = @p_user_id";

            using (IDbConnection conn = Connection)
            {
                var userPoco = await conn.QueryFirstOrDefaultAsync<UserPoco>(sql, parameters);

                return userPoco;
            }
        }

        public async Task<IEnumerable<UserPoco>> GetListAsync()
        {
            var parameters = new
            {
                p_key = Settings.SecurityKey
            };

            string sql = "select user_id, convert_from(decrypt(last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, " +
                "convert_from(decrypt(first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, " +
                "convert_from(decrypt(middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, login, pwd_hash, need_to_change_pwd, " +
                "refresh_token, token_refresh_timestamp, role, registration_timestamp from _staffinfo.users";

            using (IDbConnection conn = Connection)
            {
                var userPocos = await conn.QueryAsync<UserPoco>(sql, parameters);

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
                p_registration_timestamp = poco.RegistrationTimestamp,
                p_key = Settings.SecurityKey
            };

            var sqlBuilder = new StringBuilder("update _staffinfo.users set last_name = encrypt(@p_last_name::bytea, @p_key::bytea, 'aes'), ");
            sqlBuilder.Append("first_name = encrypt(@p_first_name::bytea, @p_key::bytea, 'aes'), middle_name = encrypt(@p_middle_name::bytea, @p_key::bytea, 'aes'), login = @p_login, need_to_change_pwd = @p_need_to_change_pwd::boolean, ");
            sqlBuilder.Append("pwd_hash = @p_pwd_hash, refresh_token = @p_refresh_token, token_refresh_timestamp = @p_token_refresh_timestamp, role = @p_role, registration_timestamp = @p_registration_timestamp ");
            sqlBuilder.Append("where user_id = @p_user_id; ");
            sqlBuilder.Append("select user_id, convert_from(decrypt(last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, ");
            sqlBuilder.Append("convert_from(decrypt(first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, ");
            sqlBuilder.Append("convert_from(decrypt(middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, login, pwd_hash, need_to_change_pwd, ");
            sqlBuilder.Append("refresh_token, token_refresh_timestamp, role, registration_timestamp from _staffinfo.users where user_id = @p_user_id;");

            using (IDbConnection conn = Connection)
            {
                var updatedUserPoco = await conn.QueryFirstOrDefaultAsync<UserPoco>(sqlBuilder.ToString(), parameters);

                return updatedUserPoco;
            }
        }
    }
}