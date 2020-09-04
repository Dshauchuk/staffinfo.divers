using AutoMapper;
using Staffinfo.Divers.Data.Poco;
using Staffinfo.Divers.Data.Repositories.Contracts;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Models.Abstract;
using Staffinfo.Divers.Services.Contracts;
using Staffinfo.Divers.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Services
{
    public class DiverService : IDiverService
    {
        private readonly IDiverRepository _diverRepository;
        private readonly IDivingTimeRepository _divingTimeRepository;
        private readonly IRescueStationRepository _rescueStationRepository;
        private readonly IMapper _mapper;

        public DiverService(IDiverRepository diverRepository, IDivingTimeRepository divingTimeRepository, IRescueStationRepository rescueStationRepository, IMapper mapper)
        {
            _diverRepository = diverRepository;
            _divingTimeRepository = divingTimeRepository;
            _rescueStationRepository = rescueStationRepository;
            _mapper = mapper;
        }

        public async Task<Diver> AddDiverAsync(EditDiverModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var poco = _mapper.Map<DiverPoco>(model);

            var added = await _diverRepository.AddAsync(poco);
            var diver = _mapper.Map<Diver>(added);

            return diver;
        }


        public async Task DeleteAsync(int diverId)
        {
            var existing = await GetAsync(diverId);
            if (existing == null)
                throw new NotFoundException("Водолаз не найден.");

            await _diverRepository.DeleteAsync(diverId);
        }


        public async Task<Diver> EditDiverAsync(int diverId, EditDiverModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var existing = await _diverRepository.GetAsync(diverId);
            if (existing == null)
                throw new NotFoundException("Водолаз не найден.");

            existing.LastName = model.LastName;
            existing.FirstName = model.FirstName;
            existing.MiddleName = model.MiddleName;
            existing.PhotoUrl = model.PhotoUrl;
            existing.BirthDate = model.BirthDate;
            existing.RescueStationId = model.RescueStationId;
            existing.MedicalExaminationDate = model.MedicalExaminationDate;
            existing.Address = model.Address;
            existing.Qualification = model.Qualification;
            existing.PersonalBookNumber = model.PersonalBookNumber;
            existing.PersonalBookIssueDate = model.PersonalBookIssueDate;
            existing.PersonalBookProtocolNumber = model.PersonalBookProtocolNumber;

            var updated = await _diverRepository.UpdateAsync(existing);
            var hours = await SetWorkingHoursAsync(updated.DiverId, model.WorkingTime);
            var diver = _mapper.Map<Diver>(updated);
            diver.WorkingTime = hours;

            return diver;
        }

        public async Task<Diver> GetAsync(int diverId)
        {
            var poco = await _diverRepository.GetAsync(diverId);
            if (poco == null)
                throw new NotFoundException("Водолаз не найден.");

            var diver = _mapper.Map<Diver>(poco);
            return diver;
        }

        public async Task<List<DivingTime>> GetListWorkingTimeByDiverAsync(int diverId)
        {
            var diver = await GetAsync(diverId);
            return diver.WorkingTime;
        }

        public async Task<IEnumerable<Diver>> GetAsync(IFilterOptions options)
        {
            IEnumerable<DiverPoco> pocos;

            if (options == null)
                pocos = await _diverRepository.GetListAsync();
            else
                pocos = await _diverRepository.GetListAsync(options);

            var divers = pocos.Select(p => _mapper.Map<Diver>(p));

            return divers;
        }

        public async Task AddDivingTimeAsync(DivingTime time)
        {
            var allDiverHours = await _divingTimeRepository.GetListAsync(time.DiverId);
            var times = new List<DivingTime>();

            if (allDiverHours != null && allDiverHours.Any())
                times.AddRange(allDiverHours.Select(t => _mapper.Map<DivingTime>(t)));

            times.Add(time);

            await SetWorkingHoursAsync(time.DiverId, times);
        }

        public async Task ChangeDivingTimeAsync(DivingTime time)
        {
            await _divingTimeRepository.UpdateAsync(_mapper.Map<DivingTimePoco>(time));
            var allDiverHours = await _divingTimeRepository.GetListAsync(time.DiverId);
            var times = new List<DivingTime>();

            if (allDiverHours != null && allDiverHours.Any())
                times.AddRange(allDiverHours.Select(t => _mapper.Map<DivingTime>(t)));

            await SetWorkingHoursAsync(time.DiverId, times);
        }

        public async Task AddPhotoAsync(string photoBase64, int diverId)
        {
            DiverPoco diver = await _diverRepository.GetAsync(diverId);
            if (diver == null)
                throw new NotFoundException("Водолаз не найден.");
            diver.PhotoUrl = photoBase64;

            await _diverRepository.UpdateAsync(diver);
        }

        public async Task DeletePhotoAsync(int diverId)
        {
            DiverPoco diver = await _diverRepository.GetAsync(diverId);
            if (diver == null)
                throw new NotFoundException("Водолаз не найден.");
            diver.PhotoUrl = null;

            await _diverRepository.UpdateAsync(diver);
        }

        public async Task DeleteDivingTimeAsync(int diverId, int year)
        {
            var allDiverHours = (await _divingTimeRepository.GetListAsync(diverId)).ToList();

            if (allDiverHours != null && allDiverHours.Any())
            {
                allDiverHours.RemoveAll(t => t.Year == year);
                await SetWorkingHoursAsync(diverId, allDiverHours.Select(t => _mapper.Map<DivingTime>(t)));
            }
        }

        public async Task<List<MinStationModel>> GetDiversPerStationAsync()
        {
            var stations = (await _rescueStationRepository.GetListAsync()).ToArray();
            var divers = (await _diverRepository.GetListAsync()).ToArray();

            List<MinStationModel> diversPerStation = new List<MinStationModel>();

            foreach (RescueStationPoco rescueStation in stations)
            {
                diversPerStation.Add(new MinStationModel()
                {
                    Id = rescueStation.StationId,
                    Name = rescueStation.StationName,
                    DiversCount = divers.Where(c => c.RescueStation.StationId == rescueStation.StationId).Count()
                });
            }

            diversPerStation = diversPerStation.OrderByDescending(c => c.DiversCount).Take(10).ToList();

            return diversPerStation;
        }

        public async Task<List<StationDivingTimeModel>> GetDivingTimePerStationAsync()
        {
            var stations = (await _rescueStationRepository.GetListAsync()).ToArray();
            var divers = (await _diverRepository.GetListAsync()).ToArray();

            List<StationDivingTimeModel> divingTimePerStation = new List<StationDivingTimeModel>();

            foreach (RescueStationPoco rescueStation in stations)
            {
                divingTimePerStation.Add(new StationDivingTimeModel()
                {
                    Id = rescueStation.StationId,
                    Name = rescueStation.StationName,
                    TotalDivingTime = 0
                });
            }

            foreach (DiverPoco diver in divers)
            {
                divingTimePerStation.First(c => c.Id == diver.RescueStationId).TotalDivingTime += diver.WorkingTime.Sum(c => c.WorkingMinutes);
            }

            return divingTimePerStation;
        }

        public async Task<List<AverageStationDivingTimeModel>> GetAverageDivingTimePerStationAsync()
        {
            var stations = (await _rescueStationRepository.GetListAsync()).ToArray();
            var divers = (await _diverRepository.GetListAsync()).ToArray();

            List<AverageStationDivingTimeModel> averageDivingTimePerStation = new List<AverageStationDivingTimeModel>();

            foreach (RescueStationPoco rescueStation in stations)
            {
                averageDivingTimePerStation.Add(new AverageStationDivingTimeModel()
                {
                    Id = rescueStation.StationId,
                    Name = rescueStation.StationName,
                    AverageDivingTime = 0,
                    DiveNumber = 0
                });
            }

            foreach (DiverPoco diver in divers)
            {
                var chartModel = averageDivingTimePerStation.First(c => c.Id == diver.RescueStationId);
                chartModel.AverageDivingTime += diver.WorkingTime.Sum(c => c.WorkingMinutes);
                chartModel.DiveNumber += diver.WorkingTime.Count;
            }

            foreach (AverageStationDivingTimeModel model in averageDivingTimePerStation)
            {
                model.AverageDivingTime = model.AverageDivingTime == 0 ? 0 : Math.Round(model.AverageDivingTime / model.DiveNumber, 1);
            }

            return averageDivingTimePerStation;
        }

        private async Task<List<DivingTime>> SetWorkingHoursAsync(int diverId, IEnumerable<DivingTime> time)
        {
            List<DivingTime> currentTime = new List<DivingTime>();

            var allDiverHours = await _divingTimeRepository.GetListAsync(diverId);

            // remove all if exist
            if (allDiverHours.Any())
                await _divingTimeRepository.DeleteAsync(diverId, allDiverHours.Select(h => h.Year));

            if (time != null && time.Any())
            {
                var pocos = time.Select(t => _mapper.Map<DivingTimePoco>(t));
                var addedPocos = await _divingTimeRepository.AddAsync(pocos);
                currentTime.AddRange(addedPocos.Select(p => _mapper.Map<DivingTime>(p)));
            }

            return currentTime;
        }
    }
}