using Microsoft.AspNetCore.Mvc;
using Moq;
using staffinfo.divers.Controllers;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Models.Abstract;
using Staffinfo.Divers.Services.Contracts;
using Staffinfo.Divers.Shared.Exceptions;
using System;
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

            var rescueStationModel = new RescueStation()
            {
                StationId = 1,
                StationName = "Ветковская",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var diverModel = new Diver()
            {
                Address = "г.Ветка, ул.Батракова 32",
                BirthDate = DateTime.Now,
                DiverId = 1,
                FirstName = "Иван",
                LastName = "Иванов",
                MedicalExaminationDate = DateTime.Now,
                MiddleName = "Иванов",
                PersonalBookIssueDate = DateTime.Now,
                PersonalBookNumber = "132412",
                PersonalBookProtocolNumber = "13233434",
                PhotoUrl = "",
                Qualification = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                RescueStation = rescueStationModel
            };

            var divers = new List<Diver>()
            {
                diverModel
            };

            var rescueStations = new List<RescueStation>()
            {
                rescueStationModel
            };

            var diverServiceMock = new Mock<IDiverService>();
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            rescueStationServiceMock.Setup(repo => repo.GetAsync())
                .Returns(Task.FromResult(rescueStations as IEnumerable<RescueStation>));
            diverServiceMock.Setup(repo => repo.GetAsync(null as IFilterOptions))
                .Returns(Task.FromResult(divers as IEnumerable<Diver>));
            rescueStationServiceMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(rescueStationModel));
            var dashboardController = new DashboardController(diverServiceMock.Object, rescueStationServiceMock.Object);

            // Act
            var redirectToActionResult = (await dashboardController.RedirectToDivers(existingIndex)) as RedirectToActionResult;

            // Assert
            Assert.NotNull(redirectToActionResult);
        }

        [Fact]
        public async Task RedirectToDivers_GivenInvalidInput_ShouldThrowNotFoundException()
        {
            // Arrange
            var existingIndex = 0;

            var rescueStationModel = new RescueStation()
            {
                StationId = 1,
                StationName = "Ветковская",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var diverModel = new Diver()
            {
                Address = "г.Ветка, ул.Батракова 32",
                BirthDate = DateTime.Now,
                DiverId = 1,
                FirstName = "Иван",
                LastName = "Иванов",
                MedicalExaminationDate = DateTime.Now,
                MiddleName = "Иванов",
                PersonalBookIssueDate = DateTime.Now,
                PersonalBookNumber = "132412",
                PersonalBookProtocolNumber = "13233434",
                PhotoUrl = "",
                Qualification = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                RescueStation = rescueStationModel
            };

            var divers = new List<Diver>()
            {
                diverModel
            };

            var rescueStations = new List<RescueStation>()
            {
                rescueStationModel
            };

            var diverServiceMock = new Mock<IDiverService>();
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            rescueStationServiceMock.Setup(repo => repo.GetAsync())
                .Returns(Task.FromResult(rescueStations as IEnumerable<RescueStation>));
            diverServiceMock.Setup(repo => repo.GetAsync(null as IFilterOptions))
                .Returns(Task.FromResult(divers as IEnumerable<Diver>));
            rescueStationServiceMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Throws(new NotFoundException());
            var dashboardController = new DashboardController(diverServiceMock.Object, rescueStationServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => dashboardController.RedirectToDivers(existingIndex));
        }
    }
}
