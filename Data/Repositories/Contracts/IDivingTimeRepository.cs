using Staffinfo.Divers.Data.Poco;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Data.Repositories.Contracts
{
    public interface IDivingTimeRepository
    {
        Task<DivingTimePoco> AddAsync(DivingTimePoco poco);

        Task<IEnumerable<DivingTimePoco>> AddAsync(IEnumerable<DivingTimePoco> pocos);

        Task<DivingTimePoco> GetAsync(int diverId, int year);

        Task<IEnumerable<DivingTimePoco>> GetListAsync(int diverId);

        Task DeleteAsync(int diverId, int year);

        Task DeleteAsync(int diverId, IEnumerable<int> years);

        Task<DivingTimePoco> UpdateAsync(DivingTimePoco poco);
    }
}
