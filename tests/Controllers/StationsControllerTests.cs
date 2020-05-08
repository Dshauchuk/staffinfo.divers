using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using staffinfo.divers.Controllers;
using Staffinfo.Divers.Infrastructure.Mapping;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Services.Contracts;
using Staffinfo.Divers.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace staffinfo.divers.tests.Controllers
{
    public class StationsControllerTests
    {
        IMapper _mapper;

        public StationsControllerTests()
        {
            _mapper = new Mapper(new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper().ConfigurationProvider);
        }

        [Fact]
        public void Index_NoInput_ShouldReturnViewResultForIndexPage()
        {
            // Arrange
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            var stationsController = new StationsController(rescueStationServiceMock.Object, _mapper);

            // Act
            var result = stationsController.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void New_NoInput_ShouldReturnViewResultForEditPage()
        {
            // Arrange
            var existingViewName = "Edit";

            var rescueStationServiceMock = new Mock<IRescueStationService>();
            var stationsController = new StationsController(rescueStationServiceMock.Object, _mapper);

            // Act
            var result = stationsController.New() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingViewName, result.ViewName);
        }

        [Fact]
        public async Task Edit_GivenInvalidInput_ShouldThrowNotFoundException()
        {
            // Arrange
            var notExistingId = 1111;

            var rescueStationServiceMock = new Mock<IRescueStationService>();
            rescueStationServiceMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Throws(new NotFoundException());
            var stationsController = new StationsController(rescueStationServiceMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => stationsController.Edit(notExistingId));
        }

        [Fact]
        public async Task Edit_GivenValidInput_ShouldReturnEditRescueStation()
        {
            // Arrange
            var model = new RescueStation()
            {
                StationId = 1,
                StationName = "Ветковская",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var existingViewName = "Edit";

            var rescueStationServiceMock = new Mock<IRescueStationService>();
            rescueStationServiceMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(model));
            var stationsController = new StationsController(rescueStationServiceMock.Object, _mapper);

            // Act
            var viewResult = (await stationsController.Edit(model.StationId)) as ViewResult;

            // Assert
            Assert.NotNull(viewResult);
            var result = viewResult.Model as EditRescueStationModel;
            Assert.NotNull(result);
            Assert.Equal(existingViewName, viewResult.ViewName);
            Assert.Equal(model.StationId, result.StationId);
            Assert.Equal(model.StationName, result.StationName);
        }

        [Fact]
        public async Task Add_InputModelIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            rescueStationServiceMock.Setup(repo => repo.AddStationAsync(It.IsAny<EditRescueStationModel>()))
                .Throws(new ArgumentNullException());
            var stationsController = new StationsController(rescueStationServiceMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => stationsController.Add(null));
        }

        [Fact]
        public async Task Add_GivenValidInput_ShouldSuccessfullyAddRescueStation()
        {
            // Arrange
            var modelEdit = new EditRescueStationModel()
            {
                StationId = 1,
                StationName = "Ветковская"
            };

            var model = new RescueStation()
            {
                StationId = 1,
                StationName = "Ветковская",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var existingActionName = "Index";
            var existingControllerName = "Stations";

            var rescueStationServiceMock = new Mock<IRescueStationService>();
            rescueStationServiceMock.Setup(repo => repo.AddStationAsync(It.IsAny<EditRescueStationModel>()))
                .Returns(Task.FromResult(model));
            var stationsController = new StationsController(rescueStationServiceMock.Object, _mapper);

            // Act
            var result = (await stationsController.Add(modelEdit)) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingActionName, result.ActionName);
            Assert.Equal(existingControllerName, result.ControllerName);
            rescueStationServiceMock.Verify(repo => repo.AddStationAsync(It.IsAny<EditRescueStationModel>()), Times.Once);
        }

        [Fact]
        public async Task Update_InputModelIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            rescueStationServiceMock.Setup(repo => repo.EditStationAsync(It.IsAny<EditRescueStationModel>()))
                .Throws(new ArgumentNullException());
            var stationsController = new StationsController(rescueStationServiceMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => stationsController.Update(null));
        }

        [Fact]
        public async Task Update_GivenInvalidInput_ShouldThrowNotFoundException()
        {
            // Arrange
            var modelEdit = new EditRescueStationModel()
            {
                StationId = 11111,
                StationName = "Ветковская"
            };

            var rescueStationServiceMock = new Mock<IRescueStationService>();
            rescueStationServiceMock.Setup(repo => repo.EditStationAsync(It.IsAny<EditRescueStationModel>()))
                .Throws(new NotFoundException());
            var stationsController = new StationsController(rescueStationServiceMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => stationsController.Update(modelEdit));
        }

        [Fact]
        public async Task Update_GivenValidInput_ShouldSuccessfullyUpdateRescueStation()
        {
            // Arrange
            var modelEdit = new EditRescueStationModel()
            {
                StationId = 1,
                StationName = "Ветковская"
            };

            var model = new RescueStation()
            {
                StationId = 1,
                StationName = "Ветковская",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var existingActionName = "Index";
            var existingControllerName = "Stations";

            var rescueStationServiceMock = new Mock<IRescueStationService>();
            rescueStationServiceMock.Setup(repo => repo.EditStationAsync(It.IsAny<EditRescueStationModel>()))
                .Returns(Task.FromResult(model));
            var stationsController = new StationsController(rescueStationServiceMock.Object, _mapper);

            // Act
            var result = (await stationsController.Update(modelEdit)) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingActionName, result.ActionName);
            Assert.Equal(existingControllerName, result.ControllerName);
            rescueStationServiceMock.Verify(repo => repo.EditStationAsync(It.IsAny<EditRescueStationModel>()), Times.Once);
        }

        [Fact]
        public async Task Delete_GivenInvalidInput_ShouldThrowNotFoundException()
        {
            // Arrange
            var notExistingId = 1111;

            var rescueStationServiceMock = new Mock<IRescueStationService>();
            rescueStationServiceMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .Throws(new NotFoundException());
            var stationsController = new StationsController(rescueStationServiceMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => stationsController.Delete(notExistingId));
        }

        [Fact]
        public async Task Delete_GivenValidInput_ShouldSuccessfullyDeleteRescueStation()
        {
            // Arrange
            var existingId = 1;

            var rescueStationServiceMock = new Mock<IRescueStationService>();
            rescueStationServiceMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
            var stationsController = new StationsController(rescueStationServiceMock.Object, _mapper);

            // Act
            await stationsController.Delete(existingId);

            // Assert
            rescueStationServiceMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetListJson_NoFoundElements_ShouldThrowArgumentNullException()
        {
            // Arrange
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            rescueStationServiceMock.Setup(repo => repo.GetAsync())
                .Throws(new ArgumentNullException());
            var stationsController = new StationsController(rescueStationServiceMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => stationsController.GetListJson());
        }

        [Fact]
        public async Task GetListJson_GivenValidInput_ShouldReturnListJsonRescueStations()
        {
            // Arrange
            var model1 = new RescueStation()
            {
                StationId = 1,
                StationName = "Ветковская",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var model2 = new RescueStation()
            {
                StationId = 2,
                StationName = "Гомельская",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var models = new List<RescueStation>()
            {
                model1,
                model2
            };

            var expectedCountOfItems = 2;

            var rescueStationServiceMock = new Mock<IRescueStationService>();
            rescueStationServiceMock.Setup(repo => repo.GetAsync())
                .Returns(Task.FromResult(models as IEnumerable<RescueStation>));
            var stationsController = new StationsController(rescueStationServiceMock.Object, _mapper);

            // Act
            var jsonResult = await stationsController.GetListJson() as JsonResult;

            // Assert
            Assert.NotNull(jsonResult);
            var result = jsonResult.Value as List<RescueStation>;
            Assert.NotNull(result);
            Assert.Equal(expectedCountOfItems, result.Count);
            Assert.Equal(model1.StationId, result[0].StationId);
            Assert.Equal(model1.StationName, result[0].StationName);
            Assert.Equal(model1.CreatedAt, result[0].CreatedAt);
            Assert.Equal(model1.UpdatedAt, result[0].UpdatedAt);
            Assert.Equal(model2.StationId, result[1].StationId);
            Assert.Equal(model2.StationName, result[1].StationName);
            Assert.Equal(model2.CreatedAt, result[1].CreatedAt);
            Assert.Equal(model2.UpdatedAt, result[1].UpdatedAt);
        }
    }
}
