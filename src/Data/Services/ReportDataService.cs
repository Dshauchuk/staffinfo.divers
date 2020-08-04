using AutoMapper;
using Staffinfo.Divers.Data.Poco;
using Staffinfo.Divers.Data.Repositories.Contracts;
using Staffinfo.Divers.Data.Services.Contracts;
using Staffinfo.Divers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Data.Services
{
    public class ReportDataService : IReportDataService
    {
        private readonly IDiverRepository _diverRepository;
        private readonly IMapper _mapper;

        public ReportDataService(IDiverRepository diverRepository, IMapper mapper)
        {
            _diverRepository = diverRepository;
            _mapper = mapper;
        }

        public async Task<List<DiversReportData>> GetReportDatas()
        {
            List<DiversReportData> diversReportDatas = new List<DiversReportData>();
            IEnumerable<DiverPoco> pocos = await _diverRepository.GetListAsync();

            var divers = pocos.Select(p => _mapper.Map<Diver>(p));

            foreach (var diver in divers)
            {
                diversReportDatas.Add(new DiversReportData()
                {
                    Name = diver.LastName + " " + diver.FirstName + " " + diver.MiddleName,
                    StationName = diver.RescueStation.StationName,
                    BirthDate = diver.BirthDate,
                    MedicalExaminationDate = diver.MedicalExaminationDate,
                    TotalHours = diver.WorkingTime.Sum(t => t.WorkingMinutes),
                    HoursThisYear = diver.WorkingTime.Where(t => t.Year == DateTime.Now.Year).Sum(t => t.WorkingMinutes)
                });
            }

            diversReportDatas = diversReportDatas.OrderBy(c => c.StationName).ToList();

            return diversReportDatas;
        }
    }
}
