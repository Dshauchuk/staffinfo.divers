using Microsoft.AspNetCore.Mvc;
using Moq;
using staffinfo.divers.Controllers;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace staffinfo.divers.tests.Controllers
{
    public class DashboardControllerTests
    {
        [Fact]
        public async Task Index_NoInput_ShouldReturnViewResultForIndexPage()
        {
            // Arrange
            var diverServiceMock = new Mock<IDiverService>();
            var rescueStationServiceMock = new Mock<IRescueStationService>();

            var stationModel = new MinStationModel()
            {
                DiversCount = 2,
                StationId = 1,
                StationName = "Ветковская"
            };

            var stationsModel = new List<MinStationModel>()
            {
                stationModel
            };

            var stationDivingTimeModel = new StationDivingTimeModel()
            {
                Id = 1,
                Name = "Ветковская",
                TotalDivingTime = 32
            };

            var stationsDivingTimeModel = new List<StationDivingTimeModel>()
            {
                stationDivingTimeModel
            };

            var averageStationDivingTimeModel = new AverageStationDivingTimeModel()
            {
                Id = 1,
                Name = "Ветковская",
                AverageDivingTime = 8,
                DiveNumber = 4
            };

            var averageStationsDivingTimeModel = new List<AverageStationDivingTimeModel>()
            {
                averageStationDivingTimeModel
            };

            diverServiceMock.Setup(repo => repo.GetDiversPerStationAsync())
                .Returns(Task.FromResult(stationsModel as List<MinStationModel>));
            diverServiceMock.Setup(repo => repo.GetDivingTimePerStationAsync())
                .Returns(Task.FromResult(stationsDivingTimeModel as List<StationDivingTimeModel>));
            diverServiceMock.Setup(repo => repo.GetAverageDivingTimePerStationAsync())
                .Returns(Task.FromResult(averageStationsDivingTimeModel as List<AverageStationDivingTimeModel>));

            var dashboardController = new DashboardController(diverServiceMock.Object, rescueStationServiceMock.Object);

            // Act
            var result = (await dashboardController.Index()) as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task RedirectToDivers_GivenValidInput_ShouldReturnViewResultForIndexPage()
        {
            // Arrange
            var existingIndex = 0;

            var diverModel = new MinStationModel()
            {
                DiversCount = 2,
                StationId = 1,
                StationName = "Ветковская"
            };

            var divers = new List<MinStationModel>()
            {
                diverModel
            };

            var diverServiceMock = new Mock<IDiverService>();
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            diverServiceMock.Setup(repo => repo.GetDiversPerStationAsync())
                .Returns(Task.FromResult(divers as List<MinStationModel>));
            var dashboardController = new DashboardController(diverServiceMock.Object, rescueStationServiceMock.Object);

            // Act
            var redirectToActionResult = (await dashboardController.RedirectToDivers(existingIndex)) as RedirectToActionResult;

            // Assert
            Assert.NotNull(redirectToActionResult);
        }
    }
}
