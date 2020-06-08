﻿using Microsoft.AspNetCore.Mvc;
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

            var existingUrl = "api/divers";

            var diverServiceMock = new Mock<IDiverService>();
            diverServiceMock.Setup(repo => repo.AddDiverAsync(It.IsAny<EditDiverModel>()))
                .Returns(Task.FromResult(model));
            var diverController = new DiverController(diverServiceMock.Object);

            // Act
            var createdResult = (await diverController.CreateAsync(modelEdit)) as CreatedResult;

            // Assert
            Assert.NotNull(createdResult);
            var result = createdResult.Value as Diver;
            Assert.NotNull(result);
            Assert.Equal(existingUrl, createdResult.Location);
            Assert.Equal(model.FirstName, result.FirstName);
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
            var okObjectResult = (await diverController.UpdateAsync(model.DiverId, modelEdit)) as OkObjectResult;

            // Assert
            Assert.NotNull(okObjectResult);
            var result = okObjectResult.Value as Diver;
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
            var okObjectResult = (await diverController.GetAsync(model.DiverId)) as OkObjectResult;

            // Assert
            Assert.NotNull(okObjectResult);
            var result = okObjectResult.Value as Diver;
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

            var expectedCountOfItems = 1;

            var diverServiceMock = new Mock<IDiverService>();
            diverServiceMock.Setup(repo => repo.GetAsync(It.IsAny<FilterOptions>()))
                .Returns(Task.FromResult((IEnumerable<Diver>)modelsFilter));
            var diverController = new DiverController(diverServiceMock.Object);

            // Act
            var okObjectResult = (await diverController.GetListAsync(options)) as OkObjectResult;

            // Assert
            Assert.NotNull(okObjectResult);
            var result = okObjectResult.Value as List<Diver>;
            Assert.NotNull(result);
            Assert.Equal(expectedCountOfItems, result.Count);
            Assert.Equal(model1.FirstName, result[0].FirstName);
            Assert.Equal(model1.LastName, result[0].LastName);
            Assert.Equal(model1.MiddleName, result[0].MiddleName);
            Assert.Equal(model1.Address, result[0].Address);
            Assert.Equal(model1.BirthDate, result[0].BirthDate);
            Assert.Equal(model1.DiverId, result[0].DiverId);
            Assert.Equal(model1.MedicalExaminationDate, result[0].MedicalExaminationDate);
            Assert.Equal(model1.PersonalBookIssueDate, result[0].PersonalBookIssueDate);
            Assert.Equal(model1.PersonalBookNumber, result[0].PersonalBookNumber);
            Assert.Equal(model1.PersonalBookProtocolNumber, result[0].PersonalBookProtocolNumber);
            Assert.Equal(model1.PhotoUrl, result[0].PhotoUrl);
            Assert.Equal(model1.Qualification, result[0].Qualification);
            Assert.Equal(model1.CreatedAt, result[0].CreatedAt);
            Assert.Equal(model1.UpdatedAt, result[0].UpdatedAt);
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