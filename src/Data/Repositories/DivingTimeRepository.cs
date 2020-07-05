using Dapper;
using Staffinfo.Divers.Data.Poco;
using Staffinfo.Divers.Data.Repositories.Contracts;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Data.Repositories
{
    public class DivingTimeRepository : DapperRepository, IDivingTimeRepository
    {
        public DivingTimeRepository(string connectionString) : base(connectionString)
        {

        }

        public async Task<DivingTimePoco> AddAsync(DivingTimePoco poco)
        {
            var parameters = new
            {
                p_diver_id = poco.DiverId,
                p_year = poco.Year,
                p_working_minutes = poco.WorkingMinutes
            };

            var sqlBuilder = new StringBuilder("with ins as (INSERT into ");
            sqlBuilder.Append("_staffinfo.diving_hours(");
                sqlBuilder.Append("diver_id, ");
                sqlBuilder.Append("year, ");
                sqlBuilder.Append("working_minutes) ");
            sqlBuilder.Append("VALUES(");
                sqlBuilder.Append("@p_diver_id, ");
                sqlBuilder.Append("@p_year, ");
                sqlBuilder.Append("@p_working_minutes) ");
            sqlBuilder.Append("returning *) ");
            sqlBuilder.Append("select * from ");
                sqlBuilder.Append("ins ");
                sqlBuilder.Append("left join _staffinfo.divers d on ins.diver_id = d.diver_id");

            using (IDbConnection conn = Connection)
            {
                var addedTimePoco =
                    (await conn.QueryAsync<DivingTimePoco, DiverPoco, DivingTimePoco>(sqlBuilder.ToString(), (time, diver) =>
                    {
                        time.Diver = diver;

                        return time;
                    },
                    splitOn: "diver_id",
                    param: parameters))
                    .FirstOrDefault();

                return addedTimePoco;
            }
        }

        public async Task<IEnumerable<DivingTimePoco>> AddAsync(IEnumerable<DivingTimePoco> pocos)
        {
            var parameters = pocos.Select(p => new
            {
                p_diver_id = p.DiverId,
                p_year = p.Year,
                p_working_minutes = p.WorkingMinutes
            });

            var sqlBuilder = new StringBuilder("INSERT into ");
            sqlBuilder.Append("_staffinfo.diving_hours(");
                sqlBuilder.Append("diver_id, ");
                sqlBuilder.Append("year, ");
                sqlBuilder.Append("working_minutes) ");
            sqlBuilder.Append("VALUES(");
                sqlBuilder.Append("@p_diver_id, ");
                sqlBuilder.Append("@p_year, ");
                sqlBuilder.Append("@p_working_minutes)");

            using (IDbConnection conn = Connection)
            {
                await conn.ExecuteAsync(sqlBuilder.ToString(), parameters);

                sqlBuilder = new StringBuilder("select * from ");
                    sqlBuilder.Append("_staffinfo.diving_hours dh ");
                    sqlBuilder.Append("left join _staffinfo.divers d on dh.diver_id = d.diver_id ");
                sqlBuilder.Append("where ");
                    sqlBuilder.Append("dh.diver_id = any(@p_diver_ids) ");
                    sqlBuilder.Append("and year = any(@p_years)");

                var addedTimePocos =
                    (await conn.QueryAsync<DivingTimePoco, DiverPoco, DivingTimePoco>(sqlBuilder.ToString(), (time, diver) =>
                    {
                        time.Diver = diver;

                        return time;
                    },
                    splitOn: "diver_id",
                    param: new { p_diver_ids = pocos.Select(p => p.DiverId).Distinct().ToList(), p_years = pocos.Select(p => p.Year).Distinct().ToList()}));

                return addedTimePocos;
            }
        }

        public async Task DeleteAsync(int diverId, int year)
        {
            var parameters = new
            {
                p_diver_id = diverId,
                p_year = year,
            };

            var sql = 
            "delete from " +
                "_staffinfo.diving_hours " +
            "where " +
                "diver_id = @p_diver_id " +
                "and year = @p_year";


            using (IDbConnection conn = Connection)
            {
                await conn.ExecuteAsync(sql, parameters);
            }
        }

        public async Task DeleteAsync(int diverId, IEnumerable<int> years)
        {
            var parameters = new
            {
                p_diver_id = diverId,
                p_years = years.ToList(),
            };

            var sql = 
            "delete from " +
                "_staffinfo.diving_hours " +
            "where " +
                "diver_id = @p_diver_id " +
                "and year = any(@p_years)";



            using (IDbConnection conn = Connection)
            {
                await conn.ExecuteAsync(sql, parameters);
            }
        }

        public async Task<DivingTimePoco> GetAsync(int diverId, int year)
        {
            var parameters = new
            {
                p_diver_id = diverId,
                p_year = year,
            };

            string sql = 
            "select * from " +
                "_staffinfo.diving_hours dh " +
                "left join _staffinfo.divers d on dh.diver_id = d.diver_id " +
            "where " +
                "dh.diver_id = @p_diver_id " +
                "and year = @p_year";

            using (IDbConnection conn = Connection)
            {
                var timePoco =
                    (await conn.QueryAsync<DivingTimePoco, DiverPoco, DivingTimePoco>(sql, (time, diver) =>
                    {
                        time.Diver = diver;

                        return time;
                    },
                    splitOn: "diver_id",
                    param: parameters))
                    .FirstOrDefault();

                return timePoco;
            }
        }

        public async Task<IEnumerable<DivingTimePoco>> GetListAsync(int diverId)
        {
            var parameters = new
            {
                p_diver_id = diverId
            };

            string sql = 
            "select * from " +
                "_staffinfo.diving_hours dh " +
                "left join _staffinfo.divers d on dh.diver_id = d.diver_id " +
            "where " +
                "dh.diver_id = @p_diver_id";

            using (IDbConnection conn = Connection)
            {
                var timePocos = (await conn.QueryAsync<DivingTimePoco, DiverPoco, DivingTimePoco>(sql, (time, diver) =>
                {
                    time.Diver = diver;

                    return time;
                },
                splitOn: "diver_id",
                param: parameters));

                return timePocos;
            }
        }

        public async Task<DivingTimePoco> UpdateAsync(DivingTimePoco poco)
        {
            var parameters = new
            {
                p_diver_id = poco.DiverId,
                p_year = poco.Year,
                p_working_minutes = poco.WorkingMinutes
            };

            var sqlBuilder = new StringBuilder("UPDATE ");
                sqlBuilder.Append("_staffinfo.diving_hours ");
            sqlBuilder.Append("set ");
                sqlBuilder.Append("working_minutes = @p_working_minutes ");
            sqlBuilder.Append("where ");
                sqlBuilder.Append("diver_id = @p_diver_id ");
                sqlBuilder.Append("and year = @p_year returning *;");

            using (IDbConnection conn = Connection)
            {
                var updatedTimePoco = (await conn.QueryAsync<DivingTimePoco>(sqlBuilder.ToString(), parameters)).FirstOrDefault();

                return updatedTimePoco;
            }
        }
    }
}
