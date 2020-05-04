using Microsoft.AspNetCore.Mvc;
using Moq;
using Staffinfo.Divers.Controllers;
using Staffinfo.Divers.Data.Poco;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Services.Contracts;
using Staffinfo.Divers.Shared.Exceptions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace staffinfo.divers.tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public async Task ModifyUserAsync_InputModelIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var notExistingId = 1111;

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(repo => repo.ModifyUserAsync(It.IsAny<int>(), It.IsAny<EditUserModel>()))
                .Throws(new ArgumentNullException());
            var userController = new UserController(userServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => userController.ModifyUser(notExistingId, null));
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

            var notExistingId = 1111;

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(repo => repo.ModifyUserAsync(It.IsAny<int>(), It.IsAny<EditUserModel>()))
                .Throws(new NotFoundException());
            var userController = new UserController(userServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => userController.ModifyUser(notExistingId, model));
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

            var notExistingId = 1111;

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(repo => repo.ModifyUserAsync(It.IsAny<int>(), It.IsAny<EditUserModel>()))
                .Throws(new ArgumentException());
            var userController = new UserController(userServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => userController.ModifyUser(notExistingId, model));
        }

        [Fact]
        public async Task ModifyUserAsync_GivenValidInput_ShouldReturnUpdatedUser()
        {
            // Arrange
            var modelEdit = new EditUserModel()
            {
                FirstName = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванов",
                NeedToChangePwd = false,
                Role = "admin"
            };

            var model = new User()
            {
                FirstName = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванов",
                Login = "Ivan",
                NeedToChangePwd = false,
                RefreshToken = "sdfdsfds",
                RegistrationTimestamp = DateTime.Now,
                Role = "admin",
                UserId = 1
            };

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(repo => repo.ModifyUserAsync(It.IsAny<int>(), It.IsAny<EditUserModel>()))
                .Returns(Task.FromResult(model));
            var userController = new UserController(userServiceMock.Object);

            // Act
            var result = ((await userController.ModifyUser(model.UserId, modelEdit)) as OkObjectResult).Value as User;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.Login, result.Login);
            Assert.Equal(model.FirstName, result.FirstName);
            Assert.Equal(model.LastName, result.LastName);
            Assert.Equal(model.MiddleName, result.MiddleName);
            Assert.Equal(model.UserId, result.UserId);
            Assert.Equal(model.Role, result.Role);
            Assert.Equal(model.RefreshToken, result.RefreshToken);
            Assert.Equal(model.NeedToChangePwd, result.NeedToChangePwd);
            Assert.Equal(model.RegistrationTimestamp, result.RegistrationTimestamp);
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

            var userServiceMock = new Mock<IUserService>();
            var userController = new UserController(userServiceMock.Object);

            // Act
            await userController.DeleteAsync(modelPoco.UserId);

            // Assert
            userServiceMock.Verify(repo => repo.DeleteUserAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_GivenInvalidInput_ShouldThrowNotFoundException()
        {
            // Arrange
            var notExistingId = 1111;

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(repo => repo.DeleteUserAsync(It.IsAny<int>()))
                .Throws(new NotFoundException());
            var userController = new UserController(userServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => userController.DeleteAsync(notExistingId));
        }
    }
}
