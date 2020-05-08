using Microsoft.AspNetCore.Mvc;
using staffinfo.divers.Controllers;
using Xunit;

namespace staffinfo.divers.tests.Controllers
{
    public class DashboardControllerTests
    {
        [Fact]
        public void Index_NoInput_ShouldReturnViewResultForIndexPage()
        {
            // Arrange
            var dashboardController = new DashboardController();

            // Act
            var result = dashboardController.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }
    }
}
