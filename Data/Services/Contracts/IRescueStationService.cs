using Staffinfo.Divers.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Services.Contracts
{
    public interface IRescueStationService
    {
        Task<RescueStation> AddStationAsync(EditRescueStationModel model);

        Task<RescueStation> EditStationAsync(EditRescueStationModel model);

        Task<RescueStation> GetAsync(int stationId);

        Task<IEnumerable<RescueStation>> GetAsync();

        Task<IEnumerable<RescueStation>> GetAsync(Func<RescueStation, bool> predicate);

        Task DeleteAsync(int stationId);
    }
}
