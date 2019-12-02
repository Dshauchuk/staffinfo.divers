using Staffinfo.Divers.Data.Poco;
using Staffinfo.Divers.Models.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Data.Repositories.Contracts
{
    public interface IDiverRepository
    {
        Task<DiverPoco> AddAsync(DiverPoco poco);

        Task<DiverPoco> GetAsync(int diverId);

        Task<IEnumerable<DiverPoco>> GetListAsync();

        Task<IEnumerable<DiverPoco>> GetListAsync(IFilterOptions options);

        Task DeleteAsync(int diverId);

        Task<DiverPoco> UpdateAsync(DiverPoco poco);
    }
}