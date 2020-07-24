using Dapper;
using Microsoft.Extensions.Options;
using Staffinfo.Divers.Data.Poco;
using Staffinfo.Divers.Data.Repositories.Contracts;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Data.Repositories
{
    public class DiverRepository : DapperRepository, IDiverRepository
    {
        private Settings _settings;


        public DiverRepository(string connectionString, IOptions<Settings> settings) : base(connectionString)
        {
            _settings = settings.Value;
        }

        public async Task<DiverPoco> AddAsync(DiverPoco poco)
        {
            var parameters = new
            {
                p_last_name = poco.LastName,
                p_first_name = poco.FirstName,
                p_middle_name = poco.MiddleName,
                p_photo_url = poco.PhotoUrl,
                p_birth_date = poco.BirthDate?.ToString("yyyy-MM-dd"),
                p_station_id = poco.RescueStationId,
                p_medical_exam_date = poco.MedicalExaminationDate,
                p_address = poco.Address,
                p_qualification = poco.Qualification,
                p_book_number = poco.PersonalBookNumber,
                p_book_issue_date = poco.PersonalBookIssueDate,
                p_book_protocol_number = poco.PersonalBookProtocolNumber,
                p_key = _settings.SecurityKey
            };

            var sqlBuilder = new StringBuilder("with ins as (insert into ");
            sqlBuilder.Append("_staffinfo.divers(");
            sqlBuilder.Append("last_name, ");
            sqlBuilder.Append("first_name, ");
            sqlBuilder.Append("middle_name,");
            sqlBuilder.Append("photo_url, ");
            sqlBuilder.Append("birth_date, ");
            sqlBuilder.Append("rescue_station_id,");
            sqlBuilder.Append("medical_examination_date, ");
            sqlBuilder.Append("address, ");
            sqlBuilder.Append("qualification,");
            sqlBuilder.Append("personal_book_number, ");
            sqlBuilder.Append("personal_book_issue_date, ");
            sqlBuilder.Append("personal_book_protocol_number)");
            sqlBuilder.Append("values(");
            sqlBuilder.Append("encrypt(@p_last_name::bytea, @p_key::bytea, 'aes'), ");
            sqlBuilder.Append("encrypt(@p_first_name::bytea, @p_key::bytea, 'aes'), ");
            sqlBuilder.Append("encrypt(@p_middle_name::bytea, @p_key::bytea, 'aes'),");
            sqlBuilder.Append("@p_photo_url, ");
            sqlBuilder.Append("to_date(@p_birth_date, 'YYYY-MM-DD'), ");
            sqlBuilder.Append("@p_station_id,");
            sqlBuilder.Append("@p_medical_exam_date, ");
            sqlBuilder.Append("@p_address, ");
            sqlBuilder.Append("@p_qualification,");
            sqlBuilder.Append("encrypt(@p_book_number::bytea, @p_key::bytea, 'aes'), ");
            sqlBuilder.Append("@p_book_issue_date, ");
            sqlBuilder.Append("encrypt(@p_book_protocol_number::bytea, @p_key::bytea, 'aes')) returning *) ");
            sqlBuilder.Append("select ");
            sqlBuilder.Append("diver_id, ");
            sqlBuilder.Append("convert_from(decrypt(last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, ");
            sqlBuilder.Append("convert_from(decrypt(first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, ");
            sqlBuilder.Append("convert_from(decrypt(middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, ");
            sqlBuilder.Append("photo_url, ");
            sqlBuilder.Append("birth_date, ");
            sqlBuilder.Append("rescue_station_id, ");
            sqlBuilder.Append("medical_examination_date, ");
            sqlBuilder.Append("address, ");
            sqlBuilder.Append("qualification, ");
            sqlBuilder.Append("convert_from(decrypt(personal_book_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_number, ");
            sqlBuilder.Append("personal_book_issue_date, ");
            sqlBuilder.Append("convert_from(decrypt(personal_book_protocol_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_protocol_number, ");
            sqlBuilder.Append("ins.created_at, ");
            sqlBuilder.Append("ins.updated_at, ");
            sqlBuilder.Append("station_id, ");
            sqlBuilder.Append("station_name, ");
            sqlBuilder.Append("rs.created_at, ");
            sqlBuilder.Append("rs.updated_at ");
            sqlBuilder.Append("from ");
            sqlBuilder.Append("ins ");
            sqlBuilder.Append("left join _staffinfo.rescue_stations rs on rs.station_id = ins.rescue_station_id");

            using (IDbConnection conn = Connection)
            {
                var addedDiverPoco = 
                    (await conn.QueryAsync<DiverPoco, RescueStationPoco, DiverPoco>(sqlBuilder.ToString(), (diver, station) =>
                        {
                            diver.RescueStation = station;

                            return diver;
                        },
                        splitOn: "station_id",
                        param: parameters))
                    .FirstOrDefault();

                return addedDiverPoco;
            }
        }

        public async Task DeleteAsync(int diverId)
        {
            var parameters = new
            {
                p_diver_id = diverId
            };

            string sql = 
            "delete from " +
                "_staffinfo.divers " +
            "where diver_id = @p_diver_id";

            using (IDbConnection conn = Connection)
            {
                await conn.ExecuteAsync(sql, parameters);
            }
        }

        public async Task<DiverPoco> GetAsync(int diverId)
        {
            var parameters = new
            {
                p_diver_id = diverId,
                p_key = _settings.SecurityKey
            };

            var sqlBuilder = new StringBuilder("select ");
            sqlBuilder.Append("d.diver_id, ");
            sqlBuilder.Append("convert_from(decrypt(d.last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, ");
            sqlBuilder.Append("convert_from(decrypt(d.first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, ");
            sqlBuilder.Append("convert_from(decrypt(d.middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, ");
            sqlBuilder.Append("d.photo_url, ");
            sqlBuilder.Append("d.birth_date, ");
            sqlBuilder.Append("d.rescue_station_id, ");
            sqlBuilder.Append("d.medical_examination_date, ");
            sqlBuilder.Append("d.address, ");
            sqlBuilder.Append("d.qualification, ");
            sqlBuilder.Append("convert_from(decrypt(d.personal_book_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_number, ");
            sqlBuilder.Append("d.personal_book_issue_date, ");
            sqlBuilder.Append("convert_from(decrypt(d.personal_book_protocol_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_protocol_number, ");
            sqlBuilder.Append("d.created_at, ");
            sqlBuilder.Append("d.updated_at, ");
            sqlBuilder.Append("rs.*, ");
            sqlBuilder.Append("dh.diver_id, ");
            sqlBuilder.Append("dh.year, ");
            sqlBuilder.Append("dh.working_minutes ");
            sqlBuilder.Append("from ");
            sqlBuilder.Append("_staffinfo.divers d ");
            sqlBuilder.Append("left join _staffinfo.rescue_stations rs on station_id = rescue_station_id ");
            sqlBuilder.Append("left join _staffinfo.diving_hours dh on d.diver_id = dh.diver_id ");
            sqlBuilder.Append("where ");
            sqlBuilder.Append("d.diver_id = @p_diver_id");

            using (IDbConnection conn = Connection)
            {
                var lookup = new Dictionary<int, DiverPoco>();

                var diverPoco = 
                    (await conn.QueryAsync<DiverPoco, RescueStationPoco, DivingTimePoco, DiverPoco>(sqlBuilder.ToString(), (diver, station, time) =>
                        {
                            DiverPoco diverItem;

                            if (!lookup.TryGetValue(diver.DiverId, out diverItem))
                                lookup.Add(diver.DiverId, diverItem = diver);
                            if (diverItem.WorkingTime == null)
                                diverItem.WorkingTime = new List<DivingTimePoco>();
                            diverItem.WorkingTime.Add(time);

                            if (diverItem.RescueStation == null)
                                diverItem.RescueStation = station;

                            return diverItem;
                        },
                        splitOn: "station_id,diver_id",
                        param: parameters))
                    .FirstOrDefault();

                return diverPoco;
            }
        }

        public async Task<IEnumerable<DiverPoco>> GetListAsync()
        {
            var parameters = new
            {
                p_key = _settings.SecurityKey
            };

            var sqlBuilder = new StringBuilder("select ");
            sqlBuilder.Append("d.diver_id, ");
            sqlBuilder.Append("convert_from(decrypt(d.last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, ");
            sqlBuilder.Append("convert_from(decrypt(d.first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, ");
            sqlBuilder.Append("convert_from(decrypt(d.middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, ");
            sqlBuilder.Append("d.photo_url, ");
            sqlBuilder.Append("d.birth_date, ");
            sqlBuilder.Append("d.rescue_station_id, ");
            sqlBuilder.Append("d.medical_examination_date, ");
            sqlBuilder.Append("d.address, ");
            sqlBuilder.Append("d.qualification, ");
            sqlBuilder.Append("convert_from(decrypt(d.personal_book_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_number, ");
            sqlBuilder.Append("d.personal_book_issue_date, ");
            sqlBuilder.Append("convert_from(decrypt(d.personal_book_protocol_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_protocol_number, ");
            sqlBuilder.Append("d.created_at, ");
            sqlBuilder.Append("d.updated_at, ");
            sqlBuilder.Append("rs.*, ");
            sqlBuilder.Append("dh.diver_id, ");
            sqlBuilder.Append("dh.year, ");
            sqlBuilder.Append("dh.working_minutes ");
            sqlBuilder.Append("from ");
            sqlBuilder.Append("_staffinfo.divers d ");
            sqlBuilder.Append("left join _staffinfo.rescue_stations rs on station_id = rescue_station_id ");
            sqlBuilder.Append("left join _staffinfo.diving_hours dh on d.diver_id = dh.diver_id ");

            using (IDbConnection conn = Connection)
            {
                var lookup = new Dictionary<int, DiverPoco>();

                await conn.QueryAsync<DiverPoco, RescueStationPoco, DivingTimePoco, DiverPoco>(sqlBuilder.ToString(), (diver, station, time) =>
                {
                    DiverPoco diverItem;

                    if (!lookup.TryGetValue(diver.DiverId, out diverItem))
                        lookup.Add(diver.DiverId, diverItem = diver);
                    if (diverItem.WorkingTime == null)
                        diverItem.WorkingTime = new List<DivingTimePoco>();
                    if (time != null)
                        diverItem.WorkingTime.Add(time);

                    if (diverItem.RescueStation == null)
                        diverItem.RescueStation = station;

                    return diverItem;
                },
                    splitOn: "station_id,diver_id",
                    param: parameters);

                return lookup.Values;
            }
        }

        public async Task<IEnumerable<DiverPoco>> GetListAsync(IFilterOptions options)
        {
            var parameters = new
            {
                p_station_id = options.RescueStationId,
                p_med_exam_start_date = options.MedicalExaminationStartDate,
                p_med_exam_end_date = options.MedicalExaminationEndDate,
                p_min_qualif = options.MinQualification,
                p_max_qualif = options.MaxQualification,
                p_name_query = options.NameQuery,
                p_min_hours = options.MinHours,
                p_max_hours = options.MaxHours,
                p_key = _settings.SecurityKey
            };

            var sqlBuilder = new StringBuilder("select ");
            sqlBuilder.Append("d.diver_id, ");
            sqlBuilder.Append("convert_from(decrypt(d.last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, ");
            sqlBuilder.Append("convert_from(decrypt(d.first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, ");
            sqlBuilder.Append("convert_from(decrypt(d.middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, ");
            sqlBuilder.Append("d.photo_url, ");
            sqlBuilder.Append("d.birth_date, ");
            sqlBuilder.Append("d.rescue_station_id, ");
            sqlBuilder.Append("d.medical_examination_date, ");
            sqlBuilder.Append("d.address, ");
            sqlBuilder.Append("d.qualification, ");
            sqlBuilder.Append("convert_from(decrypt(d.personal_book_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_number, ");
            sqlBuilder.Append("d.personal_book_issue_date, ");
            sqlBuilder.Append("convert_from(decrypt(d.personal_book_protocol_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_protocol_number, ");
            sqlBuilder.Append("d.created_at, ");
            sqlBuilder.Append("d.updated_at, ");
            sqlBuilder.Append("rs.*, ");
            sqlBuilder.Append("dh.diver_id, ");
            sqlBuilder.Append("dh.year, ");
            sqlBuilder.Append("dh.working_minutes ");
            sqlBuilder.Append("from ");
            sqlBuilder.Append("_staffinfo.divers d ");
            sqlBuilder.Append("left join _staffinfo.rescue_stations rs on station_id = rescue_station_id ");
            sqlBuilder.Append("left join _staffinfo.diving_hours dh on d.diver_id = dh.diver_id ");
            sqlBuilder.Append("where 1 = 1 ");
            sqlBuilder.Append(parameters.p_station_id == null ? "" : "and @p_station_id = d.rescue_station_id ");
            sqlBuilder.Append(parameters.p_med_exam_start_date == null   ? "" : "and @p_med_exam_start_date::date <= d.medical_examination_date ");
            sqlBuilder.Append(parameters.p_med_exam_end_date == null ? "" : "and @p_med_exam_end_date::date >= d.medical_examination_date ");
            sqlBuilder.Append(parameters.p_min_qualif == null ? "" : "and @p_min_qualif <= d.qualification ");
            sqlBuilder.Append(parameters.p_max_qualif == null ? "" : "and @p_max_qualif >= d.qualification ");
            sqlBuilder.Append(parameters.p_name_query == null ? "" : "and (convert_from(decrypt(d.last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') like concat('%', @p_name_query, '%') or convert_from(decrypt(d.first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') like concat('%', @p_name_query, '%'))");

            using (IDbConnection conn = Connection)
            {
                var lookup = new Dictionary<int, DiverPoco>();

                await conn.QueryAsync<DiverPoco, RescueStationPoco, DivingTimePoco, DiverPoco>(sqlBuilder.ToString(), (diver, station, time) =>
                {
                    DiverPoco diverItem;

                    if (!lookup.TryGetValue(diver.DiverId, out diverItem))
                        lookup.Add(diver.DiverId, diverItem = diver);
                    if (diverItem.WorkingTime == null)
                        diverItem.WorkingTime = new List<DivingTimePoco>();
                    if(time != null)
                        diverItem.WorkingTime.Add(time);

                    if (diverItem.RescueStation == null)
                        diverItem.RescueStation = station;

                    return diverItem;
                },
                    splitOn: "station_id,diver_id",
                    param: parameters);

                var diverPocos = lookup.Values.Where(diver => ((parameters.p_min_hours == 0) ? true : (parameters.p_min_hours <= (diver.WorkingTime.Sum(c => c.WorkingMinutes) / 60.0))) &&
                                          ((parameters.p_max_hours == 0) ? true : (parameters.p_max_hours >= (diver.WorkingTime.Sum(c => c.WorkingMinutes) / 60.0)))).ToList();
                
                return diverPocos;
            }
        }

        public async Task<DiverPoco> UpdateAsync(DiverPoco poco)
        {
            var parameters = new
            {
                p_diver_id = poco.DiverId,
                p_last_name = poco.LastName,
                p_first_name = poco.FirstName,
                p_middle_name = poco.MiddleName,
                p_photo_url = poco.PhotoUrl,
                p_birth_date = poco.BirthDate?.ToString("yyyy-MM-dd"),
                p_station_id = poco.RescueStationId,
                p_medical_exam_date = poco.MedicalExaminationDate,
                p_address = poco.Address,
                p_qualification = poco.Qualification,
                p_book_number = poco.PersonalBookNumber,
                p_book_issue_date = poco.PersonalBookIssueDate,
                p_book_protocol_number = poco.PersonalBookProtocolNumber,
                p_updated_at = DateTimeOffset.UtcNow,
                p_key = _settings.SecurityKey
            };

            var sqlBuilder = new StringBuilder("update _staffinfo.divers set ");
            sqlBuilder.Append("last_name = encrypt(@p_last_name::bytea, @p_key::bytea, 'aes'), ");
            sqlBuilder.Append("first_name = encrypt(@p_first_name::bytea, @p_key::bytea, 'aes'), ");
            sqlBuilder.Append("middle_name = encrypt(@p_middle_name::bytea, @p_key::bytea, 'aes'),");
            sqlBuilder.Append("photo_url = @p_photo_url, ");
            sqlBuilder.Append("birth_date = to_date(@p_birth_date, 'YYYY-MM-DD'), ");
            sqlBuilder.Append("rescue_station_id = @p_station_id,");
            sqlBuilder.Append("medical_examination_date = @p_medical_exam_date, ");
            sqlBuilder.Append("address = @p_address, ");
            sqlBuilder.Append("qualification = @p_qualification,");
            sqlBuilder.Append("personal_book_number = encrypt(@p_book_number::bytea, @p_key::bytea, 'aes'), ");
            sqlBuilder.Append("personal_book_issue_date = @p_book_issue_date, ");
            sqlBuilder.Append("personal_book_protocol_number = encrypt(@p_book_protocol_number::bytea, @p_key::bytea, 'aes'), ");
            sqlBuilder.Append("updated_at = @p_updated_at ");
            sqlBuilder.Append("where ");
            sqlBuilder.Append("diver_id = @p_diver_id; ");
            sqlBuilder.Append("select ");
            sqlBuilder.Append("diver_id, ");
            sqlBuilder.Append("convert_from(decrypt(last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, ");
            sqlBuilder.Append("convert_from(decrypt(first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, ");
            sqlBuilder.Append("convert_from(decrypt(middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, ");
            sqlBuilder.Append("photo_url, ");
            sqlBuilder.Append("birth_date, ");
            sqlBuilder.Append("rescue_station_id, ");
            sqlBuilder.Append("medical_examination_date, ");
            sqlBuilder.Append("address, ");
            sqlBuilder.Append("qualification, ");
            sqlBuilder.Append("convert_from(decrypt(personal_book_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_number, ");
            sqlBuilder.Append("personal_book_issue_date, ");
            sqlBuilder.Append("convert_from(decrypt(personal_book_protocol_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_protocol_number, ");
            sqlBuilder.Append("divers.created_at, ");
            sqlBuilder.Append("divers.updated_at, ");
            sqlBuilder.Append("station_id, ");
            sqlBuilder.Append("station_name, ");
            sqlBuilder.Append("rescue_stations.created_at, ");
            sqlBuilder.Append("rescue_stations.updated_at ");
            sqlBuilder.Append("from ");
            sqlBuilder.Append("_staffinfo.divers ");
            sqlBuilder.Append("left join _staffinfo.rescue_stations on station_id = rescue_station_id ");
            sqlBuilder.Append("where ");
            sqlBuilder.Append("diver_id = @p_diver_id;");

            using (IDbConnection conn = Connection)
            {
                var updatedDiverPoco = (await conn.QueryAsync<DiverPoco, RescueStationPoco, DiverPoco>(sqlBuilder.ToString(), (diver, station) =>
                        {
                            diver.RescueStation = station;

                            return diver;
                        },
                        splitOn: "station_id", param: parameters))
                    .FirstOrDefault();

                return updatedDiverPoco;
            }
        }
    }
}
