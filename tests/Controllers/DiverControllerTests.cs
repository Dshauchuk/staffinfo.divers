using Microsoft.AspNetCore.Mvc;
using Moq;
using Staffinfo.Divers.Controllers;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Services.Contracts;
using Staffinfo.Divers.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace staffinfo.divers.tests.Controllers
{
    public class DiverControllerTests
    {
        [Fact]
        public async Task CreateAsync_InputModelIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var diverServiceMock = new Mock<IDiverService>();
            diverServiceMock.Setup(repo => repo.AddDiverAsync(It.IsAny<EditDiverModel>()))
                .Throws(new ArgumentNullException());
            var diverController = new DiverController(diverServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => diverController.CreateAsync(null));
        }
        
        [Fact]
        public async Task CreateAsync_GivenValidInput_ShouldReturnCreatedDiver()
        {
            // Arrange
            var modelEdit = new EditDiverModel()
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
                RescueStationId = 127,
                WorkingTime = new List<DivingTime>()
            };

            var model = new Diver()
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
                UpdatedAt = DateTime.Now
            };

            var diverServiceMock = new Mock<IDiverService>();
            diverServiceMock.Setup(repo => repo.AddDiverAsync(It.IsAny<EditDiverModel>()))
                .Returns(Task.FromResult(model));
            var diverController = new DiverController(diverServiceMock.Object);

            // Act
            var result = ((await diverController.CreateAsync(modelEdit)) as CreatedResult).Value as Diver;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.FirstName, result.FirstName);
            Assert.Equal(model.LastName, result.LastName);
            Assert.Equal(model.MiddleName, result.MiddleName);
            Assert.Equal(model.Address, result.Address);
            Assert.Equal(model.BirthDate, result.BirthDate);
            Assert.Equal(model.DiverId, result.DiverId);
            Assert.Equal(model.MedicalExaminationDate, result.MedicalExaminationDate);
            Assert.Equal(model.PersonalBookIssueDate, result.PersonalBookIssueDate);
            Assert.Equal(model.PersonalBookNumber, result.PersonalBookNumber);
            Assert.Equal(model.PersonalBookProtocolNumber, result.PersonalBookProtocolNumber);
            Assert.Equal(model.PhotoUrl, result.PhotoUrl);
            Assert.Equal(model.Qualification, result.Qualification);
            Assert.Equal(model.CreatedAt, result.CreatedAt);
            Assert.Equal(model.UpdatedAt, result.UpdatedAt);
        }

        [Fact]
        public async Task UpdateAsync_InputModelIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var notExistingId = 1111;

            var diverServiceMock = new Mock<IDiverService>();
            diverServiceMock.Setup(repo => repo.EditDiverAsync(It.IsAny<int>(), It.IsAny<EditDiverModel>()))
                .Throws(new ArgumentNullException());
            var diverController = new DiverController(diverServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => diverController.UpdateAsync(notExistingId, null));
        }

        [Fact]
        public async Task UpdateAsync_GivenInvalidInput_ShouldThrowNotFoundException()
        {
            // Arrange
            var notExistingId = 1111;

            var diverServiceMock = new Mock<IDiverService>();
            diverServiceMock.Setup(repo => repo.EditDiverAsync(It.IsAny<int>(), It.IsAny<EditDiverModel>()))
                .Throws(new NotFoundException());
            var diverController = new DiverController(diverServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => diverController.UpdateAsync(notExistingId, new EditDiverModel()));
        }

        [Fact]
        public async Task UpdateAsync_GivenValidInput_ShouldReturnUpdatedDiver()
        {
            // Arrange
            var modelEdit = new EditDiverModel()
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
                RescueStationId = 127,
                WorkingTime = new List<DivingTime>()
            };

            var model = new Diver()
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
                UpdatedAt = DateTime.Now
            };

            var diverServiceMock = new Mock<IDiverService>();
            diverServiceMock.Setup(repo => repo.EditDiverAsync(It.IsAny<int>(), It.IsAny<EditDiverModel>()))
                .Returns(Task.FromResult(model));
            var diverController = new DiverController(diverServiceMock.Object);

            // Act
            var result = ((await diverController.UpdateAsync(model.DiverId, modelEdit)) as OkObjectResult).Value as Diver;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.FirstName, result.FirstName);
            Assert.Equal(model.LastName, result.LastName);
            Assert.Equal(model.MiddleName, result.MiddleName);
            Assert.Equal(model.Address, result.Address);
            Assert.Equal(model.BirthDate, result.BirthDate);
            Assert.Equal(model.DiverId, result.DiverId);
            Assert.Equal(model.MedicalExaminationDate, result.MedicalExaminationDate);
            Assert.Equal(model.PersonalBookIssueDate, result.PersonalBookIssueDate);
            Assert.Equal(model.PersonalBookNumber, result.PersonalBookNumber);
            Assert.Equal(model.PersonalBookProtocolNumber, result.PersonalBookProtocolNumber);
            Assert.Equal(model.PhotoUrl, result.PhotoUrl);
            Assert.Equal(model.Qualification, result.Qualification);
            Assert.Equal(model.CreatedAt, result.CreatedAt);
            Assert.Equal(model.UpdatedAt, result.UpdatedAt);
        }

        [Fact]
        public async Task GetAsync_GivenInvalidInput_ShouldThrowNotFoundException()
        {
            // Arrange
            var notExistingId = 1111;

            var diverServiceMock = new Mock<IDiverService>();
            diverServiceMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Throws(new NotFoundException());
            var diverController = new DiverController(diverServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => diverController.GetAsync(notExistingId));
        }

        [Fact]
        public async Task GetAsync_GivenValidInput_ShouldReturnDiver()
        {
            // Arrange
            var model = new Diver()
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
                UpdatedAt = DateTime.Now
            };

            var diverServiceMock = new Mock<IDiverService>();
            diverServiceMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(model));
            var diverController = new DiverController(diverServiceMock.Object);

            // Act
            var result = ((await diverController.GetAsync(model.DiverId)) as OkObjectResult).Value as Diver;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.FirstName, result.FirstName);
            Assert.Equal(model.LastName, result.LastName);
            Assert.Equal(model.MiddleName, result.MiddleName);
            Assert.Equal(model.Address, result.Address);
            Assert.Equal(model.BirthDate, result.BirthDate);
            Assert.Equal(model.DiverId, result.DiverId);
            Assert.Equal(model.MedicalExaminationDate, result.MedicalExaminationDate);
            Assert.Equal(model.PersonalBookIssueDate, result.PersonalBookIssueDate);
            Assert.Equal(model.PersonalBookNumber, result.PersonalBookNumber);
            Assert.Equal(model.PersonalBookProtocolNumber, result.PersonalBookProtocolNumber);
            Assert.Equal(model.PhotoUrl, result.PhotoUrl);
            Assert.Equal(model.Qualification, result.Qualification);
            Assert.Equal(model.CreatedAt, result.CreatedAt);
            Assert.Equal(model.UpdatedAt, result.UpdatedAt);
        }

        [Fact]
        public async Task GetListAsync_GivenValidInput_ShouldReturnListDiver()
        {
            // Arrange
            var model1 = new Diver()
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
                UpdatedAt = DateTime.Now
            };
            var model2 = new Diver()
            {
                Address = "г.Ветка, ул.Батракова 32",
                BirthDate = DateTime.Now,
                DiverId = 2,
                FirstName = "Пётр",
                LastName = "Петров",
                MedicalExaminationDate = DateTime.Now,
                MiddleName = "Петрович",
                PersonalBookIssueDate = DateTime.Now,
                PersonalBookNumber = "11232",
                PersonalBookProtocolNumber = "123123",
                PhotoUrl = "",
                Qualification = 2,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            var modelsFilter = new List<Diver>()
            {
                model1
            };
            var options = new FilterOptions()
            {
                MinHours = 0,
                MaxHours = 2,
                MaxQualification = 3,
                MinQualification = 1,
                RescueStationId = 127,
                NameQuery = "Иван"
            };

            var diverServiceMock = new Mock<IDiverService>();
            diverServiceMock.Setup(repo => repo.GetAsync(It.IsAny<FilterOptions>()))
                .Returns(Task.FromResult((IEnumerable<Diver>)modelsFilter));
            var diverController = new DiverController(diverServiceMock.Object);

            // Act
            var result = ((await diverController.GetListAsync(options)) as OkObjectResult).Value as List<Diver>;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            foreach(Diver diver in result)
            {
                Assert.Equal(model1.FirstName, diver.FirstName);
                Assert.Equal(model1.LastName, diver.LastName);
                Assert.Equal(model1.MiddleName, diver.MiddleName);
                Assert.Equal(model1.Address, diver.Address);
                Assert.Equal(model1.BirthDate, diver.BirthDate);
                Assert.Equal(model1.DiverId, diver.DiverId);
                Assert.Equal(model1.MedicalExaminationDate, diver.MedicalExaminationDate);
                Assert.Equal(model1.PersonalBookIssueDate, diver.PersonalBookIssueDate);
                Assert.Equal(model1.PersonalBookNumber, diver.PersonalBookNumber);
                Assert.Equal(model1.PersonalBookProtocolNumber, diver.PersonalBookProtocolNumber);
                Assert.Equal(model1.PhotoUrl, diver.PhotoUrl);
                Assert.Equal(model1.Qualification, diver.Qualification);
                Assert.Equal(model1.CreatedAt, diver.CreatedAt);
                Assert.Equal(model1.UpdatedAt, diver.UpdatedAt);
            }
        }

        [Fact]
        public async Task DeleteAsync_GivenValidInput_ShouldSuccessfullyDeleteDiver()
        {
            // Arrange
            var existingId = 1;

            var diverServiceMock = new Mock<IDiverService>();
            diverServiceMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);
            var diverController = new DiverController(diverServiceMock.Object);

            // Act
            await diverController.DeleteAsync(existingId);

            // Assert
            diverServiceMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_InputModelIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var notExistingId = 1111;

            var diverServiceMock = new Mock<IDiverService>();
            diverServiceMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .Throws(new ArgumentNullException());
            var diverController = new DiverController(diverServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => diverController.DeleteAsync(notExistingId));
        }

    }
}
