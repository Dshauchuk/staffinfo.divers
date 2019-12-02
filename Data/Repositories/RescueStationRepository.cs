using Dapper;
using Staffinfo.Divers.Data.Poco;
using Staffinfo.Divers.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Data.Repositories
{
    public class RescueStationRepository : DapperRepository, IRescueStationRepository
    {
        public RescueStationRepository(string connectionString): base(connectionString)
        {
        }

        public async Task<RescueStationPoco> AddAsync(RescueStationPoco poco)
        {
            var parameters = new
            {
                p_station_name = poco.StationName
            };

            string sql = "insert into rescue_stations(station_name) values(@p_station_name) returning *;";

            using (IDbConnection conn = Connection)
            {
                var addedStationPoco = await conn.QueryFirstOrDefaultAsync<RescueStationPoco>(sql, parameters);

                return addedStationPoco;
            }
        }

        public async Task DeleteAsync(int stationId)
        {
            var parameters = new
            {
                p_station_id = stationId
            };

            string sql = "delete from rescue_stations where station_id = @p_station_id";

            using (IDbConnection conn = Connection)
            {
                await conn.ExecuteAsync(sql, parameters);
            }
        }

        public async Task<RescueStationPoco> GetAsync(int stationId)
        {
            var parameters = new
            {
                p_station_id = stationId
            };

            string sql = "select * from rescue_stations where station_id = @p_station_id";

            using (IDbConnection conn = Connection)
            {
                var stationPoco = await conn.QueryFirstOrDefaultAsync<RescueStationPoco>(sql, parameters);

                return stationPoco;
            }
        }

        public async Task<IEnumerable<RescueStationPoco>> GetListAsync()
        {
            string sql = "select * from rescue_stations";

            using (IDbConnection conn = Connection)
            {
                var stationPocos = await conn.QueryAsync<RescueStationPoco>(sql);

                return stationPocos;
            }
        }

        public async Task<RescueStationPoco> UpdateAsync(RescueStationPoco poco)
        {
            var parameters = new
            {
                p_station_id = poco.StationId,
                p_station_name = poco.StationName,
                p_updated_at = DateTimeOffset.UtcNow
            };

            var sqlBuilder = new StringBuilder("update rescue_stations set station_name = @p_station_name, ");
            sqlBuilder.Append("updated_at = @p_updated_at ");
            sqlBuilder.Append("where station_id = @p_station_id; ");
            sqlBuilder.Append("select * from rescue_stations where station_id = @p_station_id;");

            using (IDbConnection conn = Connection)
            {
                var updatedStationPoco = await conn.QueryFirstOrDefaultAsync<RescueStationPoco>(sqlBuilder.ToString(), parameters);

                return updatedStationPoco;
            }
        }
    }
}