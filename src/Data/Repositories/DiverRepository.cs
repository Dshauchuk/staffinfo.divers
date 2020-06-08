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
        RescueStationRepository _rescueStationRepository;
        DivingTimeRepository _divingTimeRepository;
        private Settings _settings;


        public DiverRepository(string connectionString, IOptions<Settings> settings) : base(connectionString)
        {
            _settings = settings.Value;
            _rescueStationRepository = new RescueStationRepository(connectionString);
            _divingTimeRepository = new DivingTimeRepository(connectionString);
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

            var sqlBuilder = new StringBuilder("with ins as (INSERT into _staffinfo.divers(");
            sqlBuilder.Append("last_name, first_name, middle_name,");
            sqlBuilder.Append("photo_url, birth_date, rescue_station_id,");
            sqlBuilder.Append("medical_examination_date, address, qualification,");
            sqlBuilder.Append("personal_book_number, personal_book_issue_date, personal_book_protocol_number)");
            sqlBuilder.Append("VALUES(encrypt(@p_last_name::bytea, @p_key::bytea, 'aes'), encrypt(@p_first_name::bytea, @p_key::bytea, 'aes'), encrypt(@p_middle_name::bytea, @p_key::bytea, 'aes'),");
            sqlBuilder.Append("@p_photo_url, @p_birth_date, @p_station_id,");
            sqlBuilder.Append("@p_medical_exam_date, @p_address, @p_qualification,");
            sqlBuilder.Append("encrypt(@p_book_number::bytea, @p_key::bytea, 'aes'), @p_book_issue_date, encrypt(@p_book_protocol_number::bytea, @p_key::bytea, 'aes')) returning *) ");
            sqlBuilder.Append("select diver_id, convert_from(decrypt(last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, ");
            sqlBuilder.Append("convert_from(decrypt(first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, ");
            sqlBuilder.Append("convert_from(decrypt(middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, ");
            sqlBuilder.Append("photo_url, birth_date, rescue_station_id, medical_examination_date, address, qualification, convert_from(decrypt(personal_book_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_number, ");
            sqlBuilder.Append("personal_book_issue_date, convert_from(decrypt(personal_book_protocol_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_protocol_number, ins.created_at, ins.updated_at, station_id, station_name, ");
            sqlBuilder.Append("rs.created_at, rs.updated_at from ins left join _staffinfo.rescue_stations rs on rs.station_id = ins.rescue_station_id");


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

            string sql = "delete from _staffinfo.divers where diver_id = @p_diver_id";

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

            string sql = "select d.diver_id, convert_from(decrypt(d.last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, " +
                "convert_from(decrypt(d.first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, " +
                "convert_from(decrypt(d.middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, " +
                "d.photo_url, d.birth_date, d.rescue_station_id, d.medical_examination_date, d.address, " +
                "d.qualification, convert_from(decrypt(d.personal_book_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_number, " +
                "d.personal_book_issue_date, convert_from(decrypt(d.personal_book_protocol_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_protocol_number, " +
                "d.created_at, d.updated_at, rs.*, dh.diver_id, dh.year, dh.working_minutes from _staffinfo.divers d" +
                " left join _staffinfo.rescue_stations rs on station_id = rescue_station_id left join _staffinfo.diving_hours dh on d.diver_id = dh.diver_id where d.diver_id = @p_diver_id";

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

            string sql = "select d.diver_id, convert_from(decrypt(d.last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, " +
                "convert_from(decrypt(d.first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, " +
                "convert_from(decrypt(d.middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, " +
                "d.photo_url, d.birth_date, d.rescue_station_id, d.medical_examination_date, d.address, " +
                "d.qualification, convert_from(decrypt(d.personal_book_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_number, " +
                "d.personal_book_issue_date, convert_from(decrypt(d.personal_book_protocol_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_protocol_number, " +
                "d.created_at, d.updated_at from _staffinfo.divers d";

            using (IDbConnection conn = Connection)
            {
                var diverPocos = await conn.QueryAsync<DiverPoco>(sql, parameters);

                foreach (DiverPoco diver in diverPocos)
                {
                    diver.RescueStation = diver.RescueStationId == null ? null : await _rescueStationRepository.GetAsync((int)diver.RescueStationId);
                    diver.WorkingTime = (await _divingTimeRepository.GetListAsync(diver.DiverId)).ToList();
                }

                return diverPocos;
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

            string sql = "select d.diver_id, convert_from(decrypt(d.last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, " +
                "convert_from(decrypt(d.first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, " +
                "convert_from(decrypt(d.middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, " +
                "d.photo_url, d.birth_date, d.rescue_station_id, d.medical_examination_date, d.address, " +
                "d.qualification, convert_from(decrypt(d.personal_book_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_number, " +
                "d.personal_book_issue_date, convert_from(decrypt(d.personal_book_protocol_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_protocol_number, " +
                "d.created_at, d.updated_at from _staffinfo.divers d";

            using (IDbConnection conn = Connection)
            {
                var diverPocos = await conn.QueryAsync<DiverPoco>(sql, parameters);

                foreach(DiverPoco diver in diverPocos)
                {
                    diver.RescueStation = diver.RescueStationId == null ? null : await _rescueStationRepository.GetAsync((int)diver.RescueStationId);
                    diver.WorkingTime = (await _divingTimeRepository.GetListAsync(diver.DiverId)).ToList();
                }

                diverPocos = diverPocos.Where(diver => ((parameters.p_station_id == null) ? true : (parameters.p_station_id == diver.RescueStationId)) &&
                                            parameters.p_min_qualif <= diver.Qualification &&
                                            parameters.p_max_qualif >= diver.Qualification &&
                                          ((parameters.p_med_exam_start_date == null) ? true : (parameters.p_med_exam_start_date <= diver.MedicalExaminationDate)) &&
                                          ((parameters.p_med_exam_end_date == null) ? true : (parameters.p_med_exam_end_date >= diver.MedicalExaminationDate)) &&
                                          ((parameters.p_name_query == null) ? true : (diver.FirstName.ToLower().Contains(parameters.p_name_query.ToLower()))) &&
                                          ((parameters.p_min_hours == 0) ? true : (parameters.p_min_hours <= (diver.WorkingTime.Sum(c => c.WorkingMinutes) / 60.0))) &&
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

            var sqlBuilder = new StringBuilder("UPDATE _staffinfo.divers set ");
            sqlBuilder.Append("last_name = encrypt(@p_last_name::bytea, @p_key::bytea, 'aes'), first_name = encrypt(@p_first_name::bytea, @p_key::bytea, 'aes'), middle_name = encrypt(@p_middle_name::bytea, @p_key::bytea, 'aes'),");
            sqlBuilder.Append("photo_url = @p_photo_url, birth_date = @p_birth_date, rescue_station_id = @p_station_id,");
            sqlBuilder.Append("medical_examination_date = @p_medical_exam_date, address = @p_address, qualification = @p_qualification,");
            sqlBuilder.Append("personal_book_number = encrypt(@p_book_number::bytea, @p_key::bytea, 'aes'), personal_book_issue_date = @p_book_issue_date, personal_book_protocol_number = encrypt(@p_book_protocol_number::bytea, @p_key::bytea, 'aes'), updated_at = @p_updated_at ");
            sqlBuilder.Append("where diver_id = @p_diver_id; ");
            sqlBuilder.Append("select diver_id, convert_from(decrypt(last_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') last_name, ");
            sqlBuilder.Append("convert_from(decrypt(first_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') first_name, ");
            sqlBuilder.Append("convert_from(decrypt(middle_name::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') middle_name, ");
            sqlBuilder.Append("photo_url, birth_date, rescue_station_id, medical_examination_date, address, qualification, convert_from(decrypt(personal_book_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_number, ");
            sqlBuilder.Append("personal_book_issue_date, convert_from(decrypt(personal_book_protocol_number::bytea, @p_key::bytea, 'aes'), 'SQL_ASCII') personal_book_protocol_number, divers.created_at, divers.updated_at, station_id, station_name, ");
            sqlBuilder.Append("rescue_stations.created_at, rescue_stations.updated_at from _staffinfo.divers left join _staffinfo.rescue_stations on station_id = rescue_station_id where diver_id = @p_diver_id;");

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
