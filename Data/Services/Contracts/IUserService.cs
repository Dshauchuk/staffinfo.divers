using Staffinfo.Divers.Models;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Services.Contracts
{
    public interface IUserService
    {
        Task<User> ModifyUserAsync(int userId, EditUserModel model);

        Task DeleteUserAsync(int userId);
    }
}