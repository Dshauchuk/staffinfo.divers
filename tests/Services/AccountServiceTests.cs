using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Staffinfo.Divers.Data.Poco;
using Staffinfo.Divers.Data.Repositories.Contracts;
using Staffinfo.Divers.Infrastructure.Mapping;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Services;
using Staffinfo.Divers.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace staffinfo.divers.tests.Service
{
    public class AccountServiceTests
    {
        IMapper _mapper;
        IConfiguration _config;

        public AccountServiceTests()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"JwtSecurityKey", "AESadsfaa122dsda"},
                {"JwtExpiryInMinutes", "8"},
                {"JwtIssuer", "https://localhost"},
                {"JwtAudience", "https://localhost"}
            };

            _config = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration).Build();
            _mapper = new Mapper(new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper().ConfigurationProvider);
        }
        
        [Fact]
        public async Task LoginAsync_GivenValidInput_ShouldReturnUserIdentity()
        {
            // Arrange
            var modelPoco = new UserPoco()
            {
                FirstName = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванов",
                Login = "admin",
                NeedToChangePwd = false,
                PwdHash = "sfsdfsa",
                RefreshToken = "sdfdsfds",
                RegistrationTimestamp = DateTime.Now,
                Role = "admin",
                TokenRefreshTimestamp = DateTime.Now,
                UserId = 1
            };

            var modelPocos = new List<UserPoco>()
            {
                modelPoco
            };

            var model = new LoginModel()
            {
                Login = "admin",
                Password = "qwerty123"
            };

            var userIdentity = new UserIdentity()
            {
                AccessToken = "sffs",
                RefreshToken = "sdfds",
                TokenExpire = DateTime.Now,
                User = _mapper.Map<User>(modelPoco)
            };

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var userManagerMock = new Mock<UserManager>(httpContextAccessorMock.Object);
            userRepositoryMock.Setup(repo => repo.GetListAsync())
                .Returns(Task.FromResult(modelPocos as IEnumerable<UserPoco>));
            userRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<UserPoco>()))
                .Returns(Task.FromResult(modelPoco as UserPoco));
            var userService = new AccountService(userRepositoryMock.Object, _mapper, _config, userManagerMock.Object);

            // Act
            var result = await userService.LoginAsync(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.User.FirstName, modelPoco.FirstName);
            Assert.Equal(result.User.LastName, modelPoco.LastName);
            Assert.Equal(result.User.MiddleName, modelPoco.MiddleName);
            Assert.Equal(result.User.Login, modelPoco.Login);
            Assert.Equal(result.User.RefreshToken, modelPoco.RefreshToken);
            Assert.Equal(result.User.Role, modelPoco.Role);
            Assert.Equal(result.User.UserId, modelPoco.UserId);
            Assert.Equal(result.User.NeedToChangePwd, modelPoco.NeedToChangePwd);
        }

        [Fact]
        public async Task LoginAsync_InputModelIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var userManagerMock = new Mock<UserManager>(httpContextAccessorMock.Object);
            var userService = new AccountService(userRepositoryMock.Object, _mapper, _config, userManagerMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => userService.LoginAsync(null));
        }

        [Fact]
        public async Task LoginAsync_GivenInvalidInput_ShouldReturnNull()
        {
            // Arrange
            var modelPoco = new UserPoco()
            {
                FirstName = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванов",
                Login = "admin",
                NeedToChangePwd = false,
                PwdHash = "sfsdfsa",
                RefreshToken = "sdfdsfds",
                RegistrationTimestamp = DateTime.Now,
                Role = "admin",
                TokenRefreshTimestamp = DateTime.Now,
                UserId = 1
            };

            var modelPocos = new List<UserPoco>()
            {
                modelPoco
            };

            var model = new LoginModel()
            {
                Login = "Ivan",
                Password = "qwerty123"
            };

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetListAsync())
                .Returns(Task.FromResult(modelPocos as IEnumerable<UserPoco>));
            var userManagerMock = new Mock<UserManager>(httpContextAccessorMock.Object);
            var userService = new AccountService(userRepositoryMock.Object, _mapper, _config, userManagerMock.Object);

            // Act
            var result = await userService.LoginAsync(model);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RefreshAsync_GivenValidInput_ShouldReturnUserIdentity()
        {
            // Arrange
            var modelPoco = new UserPoco()
            {
                FirstName = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванов",
                Login = "admin",
                NeedToChangePwd = false,
                PwdHash = "sfsdfsa",
                RefreshToken = "sdfdsfds",
                RegistrationTimestamp = DateTime.Now,
                Role = "admin",
                TokenRefreshTimestamp = DateTime.Now,
                UserId = 1
            };

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var userManagerMock = new Mock<UserManager>(httpContextAccessorMock.Object);
            userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(modelPoco as UserPoco));
            userRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<UserPoco>()))
                .Returns(Task.FromResult(modelPoco as UserPoco));
            var userService = new AccountService(userRepositoryMock.Object, _mapper, _config, userManagerMock.Object);

            // Act
            var result = await userService.RefreshAsync(modelPoco.UserId, modelPoco.RefreshToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.User.FirstName, modelPoco.FirstName);
            Assert.Equal(result.User.LastName, modelPoco.LastName);
            Assert.Equal(result.User.MiddleName, modelPoco.MiddleName);
            Assert.Equal(result.User.Login, modelPoco.Login);
            Assert.Equal(result.User.RefreshToken, modelPoco.RefreshToken);
            Assert.Equal(result.User.Role, modelPoco.Role);
            Assert.Equal(result.User.UserId, modelPoco.UserId);
            Assert.Equal(result.User.NeedToChangePwd, modelPoco.NeedToChangePwd);
        }

        [Fact]
        public async Task RefreshAsync_GivenInvalidInput_ShouldThrowNotFoundException()
        {
            // Arrange
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(null as UserPoco));
            var userManagerMock = new Mock<UserManager>(httpContextAccessorMock.Object);
            var userService = new AccountService(userRepositoryMock.Object, _mapper, _config, userManagerMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => userService.RefreshAsync(1111, ""));
        }

        [Fact]
        public async Task RefreshAsync_GivenInvalidInput_ShouldReturnNull()
        {
            // Arrange
            var modelPoco = new UserPoco()
            {
                FirstName = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванов",
                Login = "admin",
                NeedToChangePwd = false,
                PwdHash = "sfsdfsa",
                RefreshToken = "sdfdsfds",
                RegistrationTimestamp = DateTime.Now,
                Role = "admin",
                TokenRefreshTimestamp = DateTime.Now,
                UserId = 1
            };

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(modelPoco as UserPoco));
            var userManagerMock = new Mock<UserManager>(httpContextAccessorMock.Object);
            var userService = new AccountService(userRepositoryMock.Object, _mapper, _config, userManagerMock.Object);

            // Act
            var result = await userService.RefreshAsync(modelPoco.UserId, "badRefreshToken");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RegisterAsync_GivenValidInput_ShouldReturnUserIdentity()
        {
            // Arrange
            var modelPoco = new UserPoco()
            {
                FirstName = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванов",
                Login = "Ivan",
                NeedToChangePwd = false,
                PwdHash = "sfsdfsa",
                RefreshToken = "sdfdsfds",
                RegistrationTimestamp = DateTime.Now,
                Role = "admin",
                TokenRefreshTimestamp = DateTime.Now,
                UserId = 1
            };

            var model = new RegisterModel()
            {
                Login = "Ivan",
                Password = "qwerty",
                ConfirmPassword = "qwerty"
            };

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var userManagerMock = new Mock<UserManager>(httpContextAccessorMock.Object);
            userRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<UserPoco>()))
                .Returns(Task.FromResult(modelPoco as UserPoco));
            var userService = new AccountService(userRepositoryMock.Object, _mapper, _config, userManagerMock.Object);

            // Act
            var result = await userService.RegisterAsync(model);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.User.FirstName, modelPoco.FirstName);
            Assert.Equal(result.User.LastName, modelPoco.LastName);
            Assert.Equal(result.User.MiddleName, modelPoco.MiddleName);
            Assert.Equal(result.User.Login, modelPoco.Login);
            Assert.Equal(result.User.RefreshToken, modelPoco.RefreshToken);
            Assert.Equal(result.User.Role, modelPoco.Role);
            Assert.Equal(result.User.UserId, modelPoco.UserId);
            Assert.Equal(result.User.NeedToChangePwd, modelPoco.NeedToChangePwd);
        }

        [Fact]
        public async Task RegisterAsync_GivenInvalidInput_ShouldThrowArgumentNullException()
        {
            // Arrange
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var userManagerMock = new Mock<UserManager>(httpContextAccessorMock.Object);
            var userService = new AccountService(userRepositoryMock.Object, _mapper, _config, userManagerMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => userService.RegisterAsync(null));
        }

        [Fact]
        public async Task RegisterAsync_GivenInvalidInput_ShouldThrowArgumentException()
        {
            // Arrange
            var model = new RegisterModel()
            {
                Login = "Ivan",
                Password = "qwerty",
                ConfirmPassword = "qwerty123"
            };

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var userManagerMock = new Mock<UserManager>(httpContextAccessorMock.Object);
            var userService = new AccountService(userRepositoryMock.Object, _mapper, _config, userManagerMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => userService.RegisterAsync(model));
        }
    }
}
