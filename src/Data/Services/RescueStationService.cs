using AutoMapper;
using Staffinfo.Divers.Data.Poco;
using Staffinfo.Divers.Data.Repositories.Contracts;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Services.Contracts;
using Staffinfo.Divers.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Services
{
    public class RescueStationService : IRescueStationService
    {
        private readonly IRescueStationRepository _rescueStationRepository;
        private readonly IMapper _mapper;

        public RescueStationService(IRescueStationRepository rescueStationRepository, IMapper mapper)
        {
            _rescueStationRepository = rescueStationRepository;
            _mapper = mapper;
        }

        public async Task<RescueStation> AddStationAsync(EditRescueStationModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var poco = _mapper.Map<RescueStationPoco>(model);

            var added = await _rescueStationRepository.AddAsync(poco);
            var station = _mapper.Map<RescueStation>(added);

            return station;
        }

        public async Task DeleteAsync(int stationId)
        {
            var existing = await GetAsync(stationId);
            if (existing == null)
                throw new NotFoundException("Станция не найдена.");

            await _rescueStationRepository.DeleteAsync(stationId);
        }

        public async Task<RescueStation> EditStationAsync(EditRescueStationModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var existing = await _rescueStationRepository.GetAsync(model.StationId);
            if (existing == null)
                throw new NotFoundException("Станция не найдена.");

            existing.StationName = model.StationName;
            existing.UpdatedAt = DateTimeOffset.UtcNow;

            var updated = await _rescueStationRepository.UpdateAsync(existing);
            var station = _mapper.Map<RescueStation>(updated);

            return station;
        }

        public async Task<RescueStation> GetAsync(int stationId)
        {
            var poco = await _rescueStationRepository.GetAsync(stationId);
            if(poco == null)
                throw new NotFoundException("Станция не найдена.");

            var station = _mapper.Map<RescueStation>(poco);

            return station;
        }

        public async Task<IEnumerable<RescueStation>> GetAsync()
        {
            var pocos = await _rescueStationRepository.GetListAsync();
            var stations = pocos.Select(p => _mapper.Map<RescueStation>(p));

            return stations;
        }

        public async Task<IEnumerable<RescueStation>> GetAsync(Func<RescueStation, bool> predicate)
        {
            var pocos = await _rescueStationRepository.GetListAsync();
            var stations = pocos.Select(p => _mapper.Map<RescueStation>(p)).Where(predicate);

            return stations;
        }
    }
}