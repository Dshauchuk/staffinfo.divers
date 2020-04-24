using Staffinfo.Divers.Data.Poco;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Data.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<UserPoco> AddAsync(UserPoco poco);

        Task<UserPoco> GetAsync(int userId);

        Task<IEnumerable<UserPoco>> GetListAsync();

        Task DeleteAsync(int userId);

        Task<UserPoco> UpdateAsync(UserPoco poco);
    }
}
