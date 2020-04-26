using AutoMapper;
using Moq;
using Staffinfo.Divers.Data.Poco;
using Staffinfo.Divers.Data.Repositories.Contracts;
using Staffinfo.Divers.Infrastructure.Mapping;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Services;
using Staffinfo.Divers.Shared.Exceptions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace staffinfo.divers.tests.Service
{
    public class UserServiceTests
    {
        IMapper _mapper;

        public UserServiceTests()
        {
            _mapper = new Mapper(new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper().ConfigurationProvider);
        }

        [Fact]
        public async Task DeleteAsync_GivenValidInput_ShouldSuccessfullyDeleteUser()
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

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(modelPoco));
            userRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
            var userService = new UserService(userRepositoryMock.Object, _mapper);

            // Act
            await userService.DeleteUserAsync(modelPoco.UserId);

            // Assert
            userRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_GivenInvalidInput_ShouldThrowNotFoundException()
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

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(null as UserPoco));
            var userService = new UserService(userRepositoryMock.Object, _mapper);
            
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => userService.DeleteUserAsync(modelPoco.UserId));
        }

        [Fact]
        public async Task ModifyUserAsync_InputModelIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userService = new UserService(userRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => userService.ModifyUserAsync(1, null));
        }

        [Fact]
        public async Task ModifyUserAsync_GivenInvalidInput_ShouldThrowNotFoundException()
        {
            // Arrange
             var model = new EditUserModel()
            {
                FirstName = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванов",
                NeedToChangePwd = false,
                Role = "admin"
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(null as UserPoco));
            var userService = new UserService(userRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => userService.ModifyUserAsync(111111, model));
        }

        [Fact]
        public async Task ModifyUserAsync_InputRoleIsNull_ShouldThrowArgumentException()
        {
            // Arrange
            var model = new EditUserModel()
            {
                FirstName = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванов",
                NeedToChangePwd = false
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            var userService = new UserService(userRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => userService.ModifyUserAsync(111111, model));
        }

        [Fact]
        public async Task ModifyUserAsync_GivenValidInput_ShouldReturnUpdatedUser()
        {
            // Arrange
            var model = new EditUserModel()
            {
                FirstName = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванов",
                NeedToChangePwd = false,
                Role = "admin"
            };

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

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(modelPoco));
            userRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<UserPoco>()))
                .Returns(Task.FromResult(modelPoco));
            var userService = new UserService(userRepositoryMock.Object, _mapper);

            // Act
            var result = await userService.ModifyUserAsync(modelPoco.UserId, model);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<User>(result);
            Assert.Equal(result.FirstName, model.FirstName);
            Assert.Equal(result.LastName, model.LastName);
            Assert.Equal(result.MiddleName, model.MiddleName);
            Assert.Equal(result.NeedToChangePwd, model.NeedToChangePwd);
            Assert.Equal(result.Role, model.Role);
        }
    }
}
