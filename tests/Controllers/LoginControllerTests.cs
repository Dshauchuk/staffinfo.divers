using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using staffinfo.divers.Controllers;
using Staffinfo.Divers.Data.Poco;
using Staffinfo.Divers.Infrastructure.Mapping;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Services;
using Staffinfo.Divers.Services.Contracts;
using System;
using System.Threading.Tasks;
using Xunit;

namespace staffinfo.divers.tests.Controllers
{
    public class LoginControllerTests
    {
        IMapper _mapper;

        public LoginControllerTests()
        {
            _mapper = new Mapper(new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper().ConfigurationProvider);
        }

        [Fact]
        public void Index_NoInput_ShouldReturnViewResultForIndexPage()
        {
            // Arrange
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userManagerMock = new Mock<UserManager>(httpContextAccessorMock.Object);
            var accountServiceMock = new Mock<IAccountService>();
            var loginController = new LoginController(accountServiceMock.Object, userManagerMock.Object);

            // Act
            var result = loginController.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task SignIn_GivenValidInput_ShouldSuccessfullyOpenDashboardViewAsync()
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

            var existingViewName = "/Dashboard";

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userManagerMock = new Mock<UserManager>(httpContextAccessorMock.Object);
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(repo => repo.LoginAsync(It.IsAny<LoginModel>()))
                .Returns(Task.FromResult(userIdentity));
            var loginController = new LoginController(accountServiceMock.Object, userManagerMock.Object);

            // Act
            var result = (await loginController.SignIn(model)) as RedirectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingViewName, result.Url);
        }

        [Fact]
        public async Task SignIn_GivenInvalidInput_ShouldOpenIndexViewAsync()
        {
            // Arrange
            var model = new LoginModel()
            {
                Login = "admin",
                Password = "qwerty123"
            };

            var existingViewName = "Index";

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userManagerMock = new Mock<UserManager>(httpContextAccessorMock.Object);
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(repo => repo.LoginAsync(It.IsAny<LoginModel>()))
                .Returns(Task.FromResult(null as UserIdentity));
            var loginController = new LoginController(accountServiceMock.Object, userManagerMock.Object);

            // Act
            var result = (await loginController.SignIn(model)) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingViewName, result.ViewName);
        }

        [Fact]
        public async Task SignOut_GivenValidInput_ShouldSuccessfullySignOut()
        {
            // Arrange
            var existingActionName = "Index";
            var existingControllerName = "Login";

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var sessionMock = new Mock<ISession>();
            var userManagerMock = new Mock<UserManager>(httpContextAccessorMock.Object);
            var accountServiceMock = new Mock<IAccountService>();
            httpContextAccessorMock.Setup(repo => repo.HttpContext.Session).Returns(sessionMock.Object);
            var loginController = new LoginController(accountServiceMock.Object, userManagerMock.Object);

            // Act
            var result = (await loginController.SignOut()) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingActionName, result.ActionName);
            Assert.Equal(existingControllerName, result.ControllerName);
        }
    }
}
