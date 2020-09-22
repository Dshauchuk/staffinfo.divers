using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Staffinfo.Divers.Data.Poco;
using Staffinfo.Divers.Data.Repositories.Contracts;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Services.Contracts;
using Staffinfo.Divers.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly UserManager _userManager;
        public readonly string jwtSecret;
        public readonly int tokenLifeTimeInMinutes;
        public readonly string tokenIssuer;
        public readonly string tokenAudience;

        public static readonly List<string> Roles = new List<string> { "guest", "user", "admin" };

        public AccountService(IUserRepository userRepository, IMapper mapper, IConfiguration config, UserManager userManager)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _config = config;
            _userManager = userManager;

            jwtSecret = _config["JwtSecurityKey"];
            tokenLifeTimeInMinutes = int.Parse(_config["JwtExpiryInMinutes"]);
            tokenIssuer = _config["JwtIssuer"];
            tokenAudience = _config["JwtAudience"];
        }

        public async Task<UserIdentity> LoginAsync(LoginModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var userPoco = (await _userRepository.GetListAsync()).FirstOrDefault(u => u.Login.Equals(model.Login) && u.PwdHash.Equals(GenerateHashPass(model.Password)));
           

            if (userPoco == null)
                return null;

            var refreshToken = Guid.NewGuid().ToString();
            userPoco.TokenRefreshTimestamp = DateTime.UtcNow;
            userPoco.RefreshToken = refreshToken;
            var updated = await _userRepository.UpdateAsync(userPoco);

            var user = _mapper.Map<User>(updated);
            var timestamp = DateTime.UtcNow.AddMinutes(tokenLifeTimeInMinutes);
            var identity = new UserIdentity(user, BuildToken(user, timestamp), refreshToken, timestamp);

            // set identity to local storage
            _userManager.SetUserIdentity(identity);

            return identity;
        }

        public async Task<UserIdentity> RefreshAsync(int userId, string refreshToken)
        {
            var userPoco = await _userRepository.GetAsync(userId);

            if (userPoco == null)
                throw new NotFoundException("Пользователь не найден");

            if (!userPoco.RefreshToken.Equals(refreshToken))
                return null;

            // generate a new refresh token
            refreshToken = Guid.NewGuid().ToString();
            userPoco.RefreshToken = refreshToken;

            // update the identity
            await _userRepository.UpdateAsync(userPoco);

            var user = _mapper.Map<User>(userPoco);
            var timestamp = DateTime.UtcNow.AddMinutes(tokenLifeTimeInMinutes);
            var token = BuildToken(user, timestamp);

            var identity = new UserIdentity(user, token, refreshToken, timestamp);

            // set identity to local storage
            _userManager.SetUserIdentity(identity);

            return identity;
        }

        public async Task<UserIdentity> RegisterAsync(RegisterModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (!model.Password.Equals(model.ConfirmPassword))
                throw new ArgumentException("Пароли не совпадают");

            var timestamp = DateTime.UtcNow.AddMinutes(tokenLifeTimeInMinutes);
            var hash = GenerateHashPass(model.Password);
            var refreshToken = Guid.NewGuid().ToString();

            var newUser = new UserPoco
            {
                Login = model.Login,
                PwdHash = hash,
                RefreshToken = refreshToken,
                RegistrationTimestamp = timestamp,
                Role = Roles.Find(r => r.Equals("user")),
                TokenRefreshTimestamp = timestamp
            };

            var added = await _userRepository.AddAsync(newUser);
            var addedUser = _mapper.Map<User>(added);

            var identity = new UserIdentity(addedUser, BuildToken(addedUser, timestamp), refreshToken, timestamp.AddMinutes(tokenLifeTimeInMinutes));

            return identity;
        }

        private static string GenerateHashPass(string password)
        {
            var byte_hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(password));
            var string_hash = (BitConverter.ToString(byte_hash)).Replace("-", string.Empty);
            return string_hash;
        }

        private string BuildToken(User user, DateTimeOffset tokenExp)
        {

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, user.UserId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                tokenIssuer,
                tokenAudience,
                claims,
                expires: tokenExp.LocalDateTime,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}