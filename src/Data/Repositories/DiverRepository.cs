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
                p_birth_date = poco.BirthDate,
                p_station_id = poco.RescueStationId,
                p_medical_exam_date = poco.MedicalExaminationDate,
                p_address = poco.Address,
                p_qualification = poco.Qualification,
                p_book_number = poco.PersonalBookNumber,
                p_book_issue_date = poco.PersonalBookIssueDate,
                p_book_protocol_number = poco.PersonalBookProtocolNumber,
                p_key = _settings.SecurityKey
            };

            string sql = "with ins as (INSERT into " +
            "_staffinfo.divers(" +
                "last_name, " +
                "first_name, " +
                "middle_name," +
                "photo_url, " +
                "birth_date, " +
                "rescue_station_id," +
                "medical_examination_date, " +
                "address, " +
                "qualification," +
                "personal_book_number, " +
                "personal_book_issue_date, " +
                "personal_book_protocol_number)" +
            "VALUES(" +
                "encrypt(@p_last_name::bytea, @p_key::bytea, 'aes'), " +
                "encrypt(@p_first_name::bytea, @p_key::bytea, 'aes'), " +
                "encrypt(@p_middle_name::bytea, @p_key::bytea, 'aes')," +
                "@p_photo_url, " +
                "@p_birth_date, " +
                "@p_station_id," +
                "@p_medical_exam_date, " +
                "@p_address, " +
                "@p_qualification," +
                "encrypt(@p_book_number::bytea, @p_key::bytea, 'aes'), " +
                "@p_book_issue_date, " +
                "encrypt(@p_book_protocol_number::bytea, @p_key::bytea, 'aes')) returning *) " +
            "select " +
                "diver_id, " +
                "convert_from(decrypt(last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, " +
                "convert_from(decrypt(first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, " +
                "convert_from(decrypt(middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, " +
                "photo_url, " +
                "birth_date, " +
                "rescue_station_id, " +
                "medical_examination_date, " +
                "address, " +
                "qualification, " +
                "convert_from(decrypt(personal_book_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_number, " +
                "personal_book_issue_date, " +
                "convert_from(decrypt(personal_book_protocol_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_protocol_number, " +
                "ins.created_at, " +
                "ins.updated_at, " +
                "station_id, " +
                "station_name, " +
                "rs.created_at, " +
                "rs.updated_at " +
            "from " +
                "ins " +
                "left join _staffinfo.rescue_stations rs on rs.station_id = ins.rescue_station_id";

            using (IDbConnection conn = Connection)
            {
                var addedDiverPoco = 
                    (await conn.QueryAsync<DiverPoco, RescueStationPoco, DiverPoco>(sql, (diver, station) =>
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

            string sql = "select " +
                "d.diver_id, " +
                "convert_from(decrypt(d.last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, " +
                "convert_from(decrypt(d.first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, " +
                "convert_from(decrypt(d.middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, " +
                "d.photo_url, " +
                "d.birth_date, " +
                "d.rescue_station_id, " +
                "d.medical_examination_date, " +
                "d.address, " +
                "d.qualification, " +
                "convert_from(decrypt(d.personal_book_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_number, " +
                "d.personal_book_issue_date, " +
                "convert_from(decrypt(d.personal_book_protocol_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_protocol_number, " +
                "d.created_at, " +
                "d.updated_at, " +
                "rs.*, " +
                "dh.diver_id, " +
                "dh.year, " +
                "dh.working_minutes " +
            "from " +
                "_staffinfo.divers d " +
                "left join _staffinfo.rescue_stations rs on station_id = rescue_station_id " +
                "left join _staffinfo.diving_hours dh on d.diver_id = dh.diver_id " +
            "where " +
                "d.diver_id = @p_diver_id";

            using (IDbConnection conn = Connection)
            {
                var lookup = new Dictionary<int, DiverPoco>();

                var diverPoco = 
                    (await conn.QueryAsync<DiverPoco, RescueStationPoco, DivingTimePoco, DiverPoco>(sql, (diver, station, time) =>
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

            string sql = "select " +
                "d.diver_id, " +
                "convert_from(decrypt(d.last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, " +
                "convert_from(decrypt(d.first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, " +
                "convert_from(decrypt(d.middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, " +
                "d.photo_url, " +
                "d.birth_date, " +
                "d.rescue_station_id, " +
                "d.medical_examination_date, " +
                "d.address, " +
                "d.qualification, " +
                "convert_from(decrypt(d.personal_book_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_number, " +
                "d.personal_book_issue_date, " +
                "convert_from(decrypt(d.personal_book_protocol_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_protocol_number, " +
                "d.created_at, " +
                "d.updated_at, " +
                "rs.*, " +
                "dh.diver_id, " +
                "dh.year, " +
                "dh.working_minutes " +
            "from " +
                "_staffinfo.divers d " +
                "left join _staffinfo.rescue_stations rs on station_id = rescue_station_id " +
                "left join _staffinfo.diving_hours dh on d.diver_id = dh.diver_id ";

            using (IDbConnection conn = Connection)
            {
                var lookup = new Dictionary<int, DiverPoco>();

                await conn.QueryAsync<DiverPoco, RescueStationPoco, DivingTimePoco, DiverPoco>(sql, (diver, station, time) =>
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

            string sql = "select " +
                "d.diver_id, " +
                "convert_from(decrypt(d.last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, " +
                "convert_from(decrypt(d.first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, " +
                "convert_from(decrypt(d.middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, " +
                "d.photo_url, " +
                "d.birth_date, " +
                "d.rescue_station_id, " +
                "d.medical_examination_date, " +
                "d.address, " +
                "d.qualification, " +
                "convert_from(decrypt(d.personal_book_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_number, " +
                "d.personal_book_issue_date, " +
                "convert_from(decrypt(d.personal_book_protocol_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_protocol_number, " +
                "d.created_at, " +
                "d.updated_at, " +
                "rs.*, " +
                "dh.diver_id, " +
                "dh.year, " +
                "dh.working_minutes " +
            "from " +
                "_staffinfo.divers d " +
                "left join _staffinfo.rescue_stations rs on station_id = rescue_station_id " +
                "left join _staffinfo.diving_hours dh on d.diver_id = dh.diver_id " +
            "where 1 = 1 " +
                (parameters.p_station_id == null ? "" : "AND @p_station_id = d.rescue_station_id ") +
                (parameters.p_med_exam_end_date == null   ? "" : "AND @p_med_exam_start_date <= d.medical_examination_date ") +
                (parameters.p_med_exam_start_date == null ? "" : "AND @p_med_exam_end_date >= d.medical_examination_date ") +
                (parameters.p_min_qualif == null ? "" : "AND @p_min_qualif <= d.qualification ") +
                (parameters.p_max_qualif == null ? "" : "AND @p_max_qualif >= d.qualification ") +
                (parameters.p_name_query == null ? "" : "AND (convert_from(decrypt(d.last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') LIKE CONCAT('%', @p_name_query, '%') OR convert_from(decrypt(d.first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') LIKE CONCAT('%', @p_name_query, '%'))");

            using (IDbConnection conn = Connection)
            {
                var lookup = new Dictionary<int, DiverPoco>();

                await conn.QueryAsync<DiverPoco, RescueStationPoco, DivingTimePoco, DiverPoco>(sql, (diver, station, time) =>
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
                p_birth_date = poco.BirthDate,
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

            string sql = "UPDATE _staffinfo.divers set " +
                "last_name = encrypt(@p_last_name::bytea, @p_key::bytea, 'aes'), " +
                "first_name = encrypt(@p_first_name::bytea, @p_key::bytea, 'aes'), " +
                "middle_name = encrypt(@p_middle_name::bytea, @p_key::bytea, 'aes')," +
                "photo_url = @p_photo_url, " +
                "birth_date = @p_birth_date, " +
                "rescue_station_id = @p_station_id," +
                "medical_examination_date = @p_medical_exam_date, " +
                "address = @p_address, " +
                "qualification = @p_qualification," +
                "personal_book_number = encrypt(@p_book_number::bytea, @p_key::bytea, 'aes'), " +
                "personal_book_issue_date = @p_book_issue_date, " +
                "personal_book_protocol_number = encrypt(@p_book_protocol_number::bytea, @p_key::bytea, 'aes'), " +
                "updated_at = @p_updated_at " +
            "where " +
                "diver_id = @p_diver_id; " +
            "select " +
                "diver_id, " +
                "convert_from(decrypt(last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, " +
                "convert_from(decrypt(first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, " +
                "convert_from(decrypt(middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, " +
                "photo_url, " +
                "birth_date, " +
                "rescue_station_id, " +
                "medical_examination_date, " +
                "address, " +
                "qualification, " +
                "convert_from(decrypt(personal_book_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_number, " +
                "personal_book_issue_date, " +
                "convert_from(decrypt(personal_book_protocol_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_protocol_number, " +
                "divers.created_at, " +
                "divers.updated_at, " +
                "station_id, " +
                "station_name, " +
                "rescue_stations.created_at, " +
                "rescue_stations.updated_at " +
            "from " +
                "_staffinfo.divers " +
                "left join _staffinfo.rescue_stations on station_id = rescue_station_id " +
            "where " +
                "diver_id = @p_diver_id;";

            using (IDbConnection conn = Connection)
            {
                var updatedDiverPoco = (await conn.QueryAsync<DiverPoco, RescueStationPoco, DiverPoco>(sql, (diver, station) =>
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
