using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Staffinfo.Divers.Controllers;
using Xunit;

namespace staffinfo.divers.tests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_GivenValidInput_ShouldSuccessfullyOpenIndexView()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<HomeController>>();
            var homeController = new HomeController(loggerMock.Object);

            // Act
            var result = homeController.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Privacy_GivenValidInput_ShouldSuccessfullyOpenIndexView()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<HomeController>>();
            var homeController = new HomeController(loggerMock.Object);

            // Act
            var result = homeController.Privacy() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        /*[Fact]
        public void Error_GivenValidInput_ShouldSuccessfullyOpenIndexView()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<HomeController>>();
            var homeController = new HomeController(loggerMock.Object);

            // Act
            var result = homeController.Error() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }*/
    }
}
