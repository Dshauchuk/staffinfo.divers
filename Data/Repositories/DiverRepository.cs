using Dapper;
using Staffinfo.Divers.Data.Poco;
using Staffinfo.Divers.Data.Repositories.Contracts;
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

        public DiverRepository(string connectionString) : base(connectionString)
        {
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
                p_book_protocol_number = poco.PersonalBookProtocolNumber
            };

            var sqlBuilder = new StringBuilder("with ins as (INSERT into divers(");
            sqlBuilder.Append("last_name, first_name, middle_name,");
            sqlBuilder.Append("photo_url, birth_date, rescue_station_id,");
            sqlBuilder.Append("medical_examination_date, address, qualification,");
            sqlBuilder.Append("personal_book_number, personal_book_issue_date, personal_book_protocol_number)");
            sqlBuilder.Append("VALUES(@p_last_name, @p_first_name, @p_middle_name,");
            sqlBuilder.Append("@p_photo_url, @p_birth_date, @p_station_id,");
            sqlBuilder.Append("@p_medical_exam_date, @p_address, @p_qualification,");
            sqlBuilder.Append("@p_book_number, @p_book_issue_date, @p_book_protocol_number) returning *) ");
            sqlBuilder.Append("select * from ins left join rescue_stations rs on ins.rescue_station_id = rs.station_id");

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

            string sql = "delete from divers where diver_id = @p_diver_id";

            using (IDbConnection conn = Connection)
            {
                await conn.ExecuteAsync(sql, parameters);
            }
        }

        public async Task<DiverPoco> GetAsync(int diverId)
        {
            var parameters = new
            {
                p_diver_id = diverId
            };

            string sql = "select d.*, rs.*, dh.diver_id, dh.year, dh.working_minutes from divers d left join rescue_stations rs on station_id = rescue_station_id left join diving_hours dh on d.diver_id = dh.diver_id where d.diver_id = @p_diver_id";

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
            string sql = "select * from divers left join rescue_stations on station_id = rescue_station_id left join diving_hours dh on sf.diver_id = dh.diver_id";

            using (IDbConnection conn = Connection)
            {
                var lookup = new Dictionary<int, DiverPoco>();

                var diverPoco = (await conn.QueryAsync<DiverPoco, RescueStationPoco, DivingTimePoco, DiverPoco>(sql, (diver, station, time) =>
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
                splitOn: "station_id,diver_id"));

                return diverPoco;
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
                p_max_hours = options.MaxHours
            };

            string sql = "select * from divers";

            using (IDbConnection conn = Connection)
            {
                var diverPocos = await conn.QueryAsync<DiverPoco>(sql);

                foreach(DiverPoco diver in diverPocos)
                {
                    diver.RescueStation = diver.RescueStationId == null ? null : await _rescueStationRepository.GetAsync((int)diver.RescueStationId);
                    diver.WorkingTime = (await _divingTimeRepository.GetListAsync(diver.DiverId)).ToList();
                }

                diverPocos = diverPocos.Where(diver => ((parameters.p_station_id == null) ? true : (parameters.p_station_id == diver.RescueStation.StationId)) &&
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
                p_updated_at = DateTimeOffset.UtcNow
            };

            var sqlBuilder = new StringBuilder("UPDATE divers set ");
            sqlBuilder.Append("last_name = @p_last_name, first_name = @p_first_name, middle_name = @p_middle_name,");
            sqlBuilder.Append("photo_url = @p_photo_url, birth_date = @p_birth_date, rescue_station_id = @p_station_id,");
            sqlBuilder.Append("medical_examination_date = @p_medical_exam_date, address = @p_address, qualification = @p_qualification,");
            sqlBuilder.Append("personal_book_number = @p_book_number, personal_book_issue_date = @p_book_issue_date, personal_book_protocol_number = @p_book_protocol_number, updated_at = @p_updated_at ");
            sqlBuilder.Append("where diver_id = @p_diver_id; ");
            sqlBuilder.Append("select * from divers left join rescue_stations on station_id = rescue_station_id where diver_id = @p_diver_id;");

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
