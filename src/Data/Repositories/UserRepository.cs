using Dapper;
using Microsoft.Extensions.Options;
using Staffinfo.Divers.Data.Poco;
using Staffinfo.Divers.Data.Repositories.Contracts;
using Staffinfo.Divers.Models;
using System.Collections.Generic;
using System.Data;
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

            string sql = "INSERT into " +
            "_staffinfo.users(" +
                "last_name, " +
                "first_name, " +
                "middle_name," +
                "login, " +
                "pwd_hash, " +
                "need_to_change_pwd," +
                "refresh_token, " +
                "token_refresh_timestamp, " +
                "role, " +
                "registration_timestamp) " +
            "VALUES(" +
                "encrypt(@p_last_name::bytea, @p_key::bytea, 'aes'), " +
                "encrypt(@p_first_name::bytea, @p_key::bytea, 'aes'), " +
                "encrypt(@p_middle_name::bytea, @p_key::bytea, 'aes')," +
                "@p_login, " +
                "@pwd_hash, " +
                "@p_need_to_change_pwd," +
                "@p_refresh_token, " +
                "@p_token_refresh_timestamp, " +
                "@p_role, " +
                "@p_registration_timestamp) " +
            "returning *; ";

            using (IDbConnection conn = Connection)
            {
                var addedDiverPoco = await conn.QueryFirstOrDefaultAsync<UserPoco>(sql, parameters);

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

            string sql = "select " +
                "user_id, " +
                "convert_from(decrypt(last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, " +
                "convert_from(decrypt(first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, " +
                "convert_from(decrypt(middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, " +
                "login, " +
                "pwd_hash, " +
                "need_to_change_pwd, " +
                "refresh_token, " +
                "token_refresh_timestamp, " +
                "role, " +
                "registration_timestamp " +
            "from " +
                "_staffinfo.users " +
            "where " +
                "user_id = @p_user_id";

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
                p_key = _settings.SecurityKey
            };

            string sql = "select " +
                "user_id, " +
                "convert_from(decrypt(last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, " +
                "convert_from(decrypt(first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, " +
                "convert_from(decrypt(middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, " +
                "login, " +
                "pwd_hash, " +
                "need_to_change_pwd, " +
                "refresh_token, " +
                "token_refresh_timestamp, " +
                "role, " +
                "registration_timestamp " +
            "from _staffinfo.users";

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
                p_key = _settings.SecurityKey
            };

            string sql = "update " +
                "_staffinfo.users " +
            "set " +
                "last_name = encrypt(@p_last_name::bytea, @p_key::bytea, 'aes'), " +
                "first_name = encrypt(@p_first_name::bytea, @p_key::bytea, 'aes'), " +
                "middle_name = encrypt(@p_middle_name::bytea, @p_key::bytea, 'aes'), " +
                "login = @p_login, " +
                "need_to_change_pwd = @p_need_to_change_pwd::boolean, " +
                "pwd_hash = @p_pwd_hash, " +
                "refresh_token = @p_refresh_token, " +
                "token_refresh_timestamp = @p_token_refresh_timestamp, " +
                "role = @p_role, " +
                "registration_timestamp = @p_registration_timestamp " +
                "where user_id = @p_user_id; " +
            "select " +
                "user_id, " +
                "convert_from(decrypt(last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, " +
                "convert_from(decrypt(first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, " +
                "convert_from(decrypt(middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, " +
                "login, " +
                "pwd_hash, " +
                "need_to_change_pwd, " +
                "refresh_token, " +
                "token_refresh_timestamp, " +
                "role, " +
                "registration_timestamp " +
            "from " +
                "_staffinfo.users " +
            "where " +
                "user_id = @p_user_id;";

            using (IDbConnection conn = Connection)
            {
                var updatedUserPoco = await conn.QueryFirstOrDefaultAsync<UserPoco>(sql, parameters);

                return updatedUserPoco;
            }
        }
    }
}