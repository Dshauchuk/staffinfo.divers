using Staffinfo.Divers.Models;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Services.Contracts
{
    public interface IAccountService
    {
        Task<UserIdentity> RegisterAsync(RegisterModel model);

        Task<UserIdentity> LoginAsync(LoginModel model);

        Task<UserIdentity> RefreshAsync(int userId, string refreshToken);
    }
}
