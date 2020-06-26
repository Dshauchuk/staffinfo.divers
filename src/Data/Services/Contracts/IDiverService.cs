using Staffinfo.Divers.Models;
using Staffinfo.Divers.Models.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Services.Contracts
{
    public interface IDiverService
    {
        Task<Diver> AddDiverAsync(EditDiverModel model);

        Task<Diver> EditDiverAsync(int diverId, EditDiverModel model);

        Task<Diver> GetAsync(int diverId);

        Task<IEnumerable<Diver>> GetAsync(IFilterOptions options);

        Task DeleteAsync(int diverId);

        Task AddPhotoAsync(string photoBase64, int diverId);

        Task AddDivingTimeAsync(DivingTime time);

        Task DeleteDivingTimeAsync(int diverId, int year);

        Task<List<DiversPerStationModel>> GetDiversPerStationAsync();

        Task<List<DivingTimePerStationModel>> GetDivingTimePerStationAsync();

        Task<List<AverageDivingTimePerStationModel>> GetAverageDivingTimePerStationAsync();
    }
}
