using Staffinfo.Divers.Data.Poco;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Data.Repositories.Contracts
{
    public interface IRescueStationRepository
    {
        Task<RescueStationPoco> AddAsync(RescueStationPoco poco);

        Task<RescueStationPoco> GetAsync(int stationId);

        Task<IEnumerable<RescueStationPoco>> GetListAsync();

        Task DeleteAsync(int stationId);

        Task<RescueStationPoco> UpdateAsync(RescueStationPoco poco);
    }
}
