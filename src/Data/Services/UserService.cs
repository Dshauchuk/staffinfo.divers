using AutoMapper;
using Staffinfo.Divers.Data.Repositories.Contracts;
using Staffinfo.Divers.Services.Contracts;
using Staffinfo.Divers.Shared.Exceptions;
using Staffinfo.Divers.Models;
using System;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task DeleteUserAsync(int userId)
        {
            var existing = await _userRepository.GetAsync(userId);
            if (existing == null)
                throw new NotFoundException("Пользователь не найден.");
            await _userRepository.DeleteAsync(userId);
        }
        
        public async Task<User> ModifyUserAsync(int userId, EditUserModel model)
        {
            if (model == null)
                throw new ArgumentNullException();

            if (string.IsNullOrEmpty(model.Role) || !AccountService.Roles.Contains(model.Role.ToLower()))
                throw new ArgumentException("Роль не указана или указана неверно.");

            var existing = await _userRepository.GetAsync(userId);
            if (existing == null)
                throw new NotFoundException("Пользователь не найден.");
                       
            existing.LastName = model.LastName;
            existing.FirstName = model.FirstName;
            existing.MiddleName = model.MiddleName;
            existing.NeedToChangePwd = model.NeedToChangePwd;
            existing.Role = model.Role.ToLower();

            var updated = await _userRepository.UpdateAsync(existing);
            var user = _mapper.Map<User>(updated);

            return user;
        }
    }
}