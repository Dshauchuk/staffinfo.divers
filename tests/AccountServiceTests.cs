using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Staffinfo.Divers.Data.Repositories.Contracts;
using Staffinfo.Divers.Infrastructure.Mapping;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace staffinfo.divers.tests
{
    public class AccountServiceTests
    {
        IMapper _mapper;
        IConfiguration _config;
        UserManager _userManager;

        public AccountServiceTests()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"JwtSecurityKey", "Value1"},
                {"JwtExpiryInMinutes", "NestedValue1"},
                {"JwtIssuer", "NestedValue1"},
                {"JwtAudience", "NestedValue2"}
            };

            _userManager = new UserManager(null);
            _config = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration).Build();
            _mapper = new Mapper(new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper().ConfigurationProvider);
        }
        
        [Fact]
        public async Task LoginAsync_CorrectLogin_ShouldReturnUserIdentity()
        {
            // Arrange
            var model = new LoginModel()
            {
                Login = "Ivan",
                Password = "qwerty123"
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            var userManagerMock = new Mock<UserManager>();
            /*userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(modelPoco));
            userRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);*/
            var userService = new AccountService(userRepositoryMock.Object, _mapper, _config, userManagerMock.Object);

            // Act
            var result = await userService.LoginAsync(model);

            // Assert
            Assert.NotNull(result);
        }
    }
}
