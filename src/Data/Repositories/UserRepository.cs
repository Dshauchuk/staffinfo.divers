using Dapper;
using Microsoft.Extensions.Options;
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
        private Settings _settings;

        public UserRepository(string connectionString, IOptions<Settings> settings) : base(connectionString)
        {
            _settings = settings.Value;
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
                p_key = _settings.SecurityKey
            };

            var sqlBuilder = new StringBuilder("INSERT into ");
            sqlBuilder.Append("_staffinfo.users(");
                sqlBuilder.Append("last_name, ");
                sqlBuilder.Append("first_name, ");
                sqlBuilder.Append("middle_name,");
                sqlBuilder.Append("login, ");
                sqlBuilder.Append("pwd_hash, ");
                sqlBuilder.Append("need_to_change_pwd,");
                sqlBuilder.Append("refresh_token, ");
                sqlBuilder.Append("token_refresh_timestamp, ");
                sqlBuilder.Append("role, ");
                sqlBuilder.Append("registration_timestamp) ");
            sqlBuilder.Append("VALUES(");
                sqlBuilder.Append("encrypt(@p_last_name::bytea, @p_key::bytea, 'aes'), ");
                sqlBuilder.Append("encrypt(@p_first_name::bytea, @p_key::bytea, 'aes'), ");
                sqlBuilder.Append("encrypt(@p_middle_name::bytea, @p_key::bytea, 'aes'),");
                sqlBuilder.Append("@p_login, ");
                sqlBuilder.Append("@pwd_hash, ");
                sqlBuilder.Append("@p_need_to_change_pwd,");
                sqlBuilder.Append("@p_refresh_token, ");
                sqlBuilder.Append("@p_token_refresh_timestamp, ");
                sqlBuilder.Append("@p_role, ");
                sqlBuilder.Append("@p_registration_timestamp) ");
            sqlBuilder.Append("returning *; ");

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
                p_key = _settings.SecurityKey
            };

            var sqlBuilder = new StringBuilder("select ");
                sqlBuilder.Append("user_id, ");
                sqlBuilder.Append("convert_from(decrypt(last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, ");
                sqlBuilder.Append("convert_from(decrypt(first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, ");
                sqlBuilder.Append("convert_from(decrypt(middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, ");
                sqlBuilder.Append("login, ");
                sqlBuilder.Append("pwd_hash, ");
                sqlBuilder.Append("need_to_change_pwd, ");
                sqlBuilder.Append("refresh_token, ");
                sqlBuilder.Append("token_refresh_timestamp, ");
                sqlBuilder.Append("role, ");
                sqlBuilder.Append("registration_timestamp ");
            sqlBuilder.Append("from ");
                sqlBuilder.Append("_staffinfo.users ");
            sqlBuilder.Append("where ");
                sqlBuilder.Append("user_id = @p_user_id");

            using (IDbConnection conn = Connection)
            {
                var userPoco = await conn.QueryFirstOrDefaultAsync<UserPoco>(sqlBuilder.ToString(), parameters);

                return userPoco;
            }
        }

        public async Task<IEnumerable<UserPoco>> GetListAsync()
        {
            var parameters = new
            {
                p_key = _settings.SecurityKey
            };

            var sqlBuilder = new StringBuilder("select ");
                sqlBuilder.Append("user_id, ");
                sqlBuilder.Append("convert_from(decrypt(last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, ");
                sqlBuilder.Append("convert_from(decrypt(first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, ");
                sqlBuilder.Append("convert_from(decrypt(middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, ");
                sqlBuilder.Append("login, ");
                sqlBuilder.Append("pwd_hash, ");
                sqlBuilder.Append("need_to_change_pwd, ");
                sqlBuilder.Append("refresh_token, ");
                sqlBuilder.Append("token_refresh_timestamp, ");
                sqlBuilder.Append("role, ");
                sqlBuilder.Append("registration_timestamp ");
            sqlBuilder.Append("from _staffinfo.users");

            using (IDbConnection conn = Connection)
            {
                var userPocos = await conn.QueryAsync<UserPoco>(sqlBuilder.ToString(), parameters);

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
                p_key = _settings.SecurityKey
            };

            var sqlBuilder = new StringBuilder("update ");
                sqlBuilder.Append("_staffinfo.users ");
            sqlBuilder.Append("set ");
                sqlBuilder.Append("last_name = encrypt(@p_last_name::bytea, @p_key::bytea, 'aes'), ");
                sqlBuilder.Append("first_name = encrypt(@p_first_name::bytea, @p_key::bytea, 'aes'), ");
                sqlBuilder.Append("middle_name = encrypt(@p_middle_name::bytea, @p_key::bytea, 'aes'), ");
                sqlBuilder.Append("login = @p_login, ");
                sqlBuilder.Append("need_to_change_pwd = @p_need_to_change_pwd::boolean, ");
                sqlBuilder.Append("pwd_hash = @p_pwd_hash, ");
                sqlBuilder.Append("refresh_token = @p_refresh_token, ");
                sqlBuilder.Append("token_refresh_timestamp = @p_token_refresh_timestamp, ");
                sqlBuilder.Append("role = @p_role, ");
                sqlBuilder.Append("registration_timestamp = @p_registration_timestamp ");
                sqlBuilder.Append("where user_id = @p_user_id; ");
            sqlBuilder.Append("select ");
                sqlBuilder.Append("user_id, ");
                sqlBuilder.Append("convert_from(decrypt(last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, ");
                sqlBuilder.Append("convert_from(decrypt(first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, ");
                sqlBuilder.Append("convert_from(decrypt(middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, ");
                sqlBuilder.Append("login, ");
                sqlBuilder.Append("pwd_hash, ");
                sqlBuilder.Append("need_to_change_pwd, ");
                sqlBuilder.Append("refresh_token, ");
                sqlBuilder.Append("token_refresh_timestamp, ");
                sqlBuilder.Append("role, ");
                sqlBuilder.Append("registration_timestamp ");
            sqlBuilder.Append("from ");
                sqlBuilder.Append("_staffinfo.users ");
            sqlBuilder.Append("where ");
                sqlBuilder.Append("user_id = @p_user_id;");

            using (IDbConnection conn = Connection)
            {
                var updatedUserPoco = await conn.QueryFirstOrDefaultAsync<UserPoco>(sqlBuilder.ToString(), parameters);

                return updatedUserPoco;
            }
        }
    }
}