using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using staffinfo.divers.Controllers;
using Staffinfo.Divers.Data.Repositories.Contracts;
using Staffinfo.Divers.Infrastructure.Mapping;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Services.Contracts;
using Staffinfo.Divers.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace staffinfo.divers.tests.Controllers
{
    public class DiversControllerTests
    {
        IMapper _mapper;

        public DiversControllerTests()
        {
            _mapper = new Mapper(new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper().ConfigurationProvider);
        }

        [Fact]
        public void Index_GivenValidInput_ShouldSuccessfullyOpenIndexView()
        {
            // Arrange
            var diverServiceMock = new Mock<IDiverService>();
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            var divingTimeServiceMock = new Mock<IDivingTimeRepository>();
            var diversController = new DiversController(divingTimeServiceMock.Object, rescueStationServiceMock.Object, diverServiceMock.Object, _mapper);

            // Act
            var result = diversController.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_GivenInvalidInput_ShouldThrowNotFoundException()
        {
            // Arrange
            var notExistingId = 1111;

            var diverServiceMock = new Mock<IDiverService>();
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            var divingTimeServiceMock = new Mock<IDivingTimeRepository>();
            diverServiceMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Throws(new NotFoundException());
            var diversController = new DiversController(divingTimeServiceMock.Object, rescueStationServiceMock.Object, diverServiceMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => diversController.Edit(notExistingId));
        }

        [Fact]
        public async Task Edit_GivenValidInput_ShouldReturnDiver()
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
                UpdatedAt = DateTime.Now,
                RescueStation = new RescueStation()
                {
                    StationId = 1,
                    StationName = "Ветковская",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            };

            var diverServiceMock = new Mock<IDiverService>();
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            var divingTimeServiceMock = new Mock<IDivingTimeRepository>();
            diverServiceMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(model));
            var diversController = new DiversController(divingTimeServiceMock.Object, rescueStationServiceMock.Object, diverServiceMock.Object, _mapper);

            // Act
            var result = ((await diversController.Edit(model.DiverId)) as ViewResult).Model as EditDiverModel;

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
        }

        [Fact]
        public async Task Details_GivenInvalidInput_ShouldThrowNotFoundException()
        {
            // Arrange
            var notExistingId = 1111;

            var diverServiceMock = new Mock<IDiverService>();
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            var divingTimeServiceMock = new Mock<IDivingTimeRepository>();
            diverServiceMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Throws(new NotFoundException());
            var diversController = new DiversController(divingTimeServiceMock.Object, rescueStationServiceMock.Object, diverServiceMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => diversController.Details(notExistingId));
        }

        [Fact]
        public async Task Details_GivenValidInput_ShouldReturnDiver()
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
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            var divingTimeServiceMock = new Mock<IDivingTimeRepository>();
            diverServiceMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(model));
            var diversController = new DiversController(divingTimeServiceMock.Object, rescueStationServiceMock.Object, diverServiceMock.Object, _mapper);

            // Act
            var result = ((await diversController.Details(model.DiverId)) as ViewResult).Model as Diver;

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
        public void New_GivenValidInput_ShouldSuccessfullyOpenEditView()
        {
            // Arrange
            var diverServiceMock = new Mock<IDiverService>();
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            var divingTimeServiceMock = new Mock<IDivingTimeRepository>();
            var diversController = new DiversController(divingTimeServiceMock.Object, rescueStationServiceMock.Object, diverServiceMock.Object, _mapper);

            // Act
            var result = diversController.New() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Update_InputModelIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var existingId = 1111;

            var diverServiceMock = new Mock<IDiverService>();
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            var divingTimeServiceMock = new Mock<IDivingTimeRepository>();
            var diversController = new DiversController(divingTimeServiceMock.Object, rescueStationServiceMock.Object, diverServiceMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => diversController.Update(existingId, null));
        }

        [Fact]
        public async Task Update_GivenInvalidInput_ShouldThrowNotFoundException()
        {
            // Arrange
            var modelEdit = new EditDiverModel()
            {
                Address = "г.Ветка, ул.Батракова 32",
                BirthDate = DateTime.Now,
                DiverId = 11111,
                FirstName = "Иван",
                LastName = "Иванов",
                MedicalExaminationDate = DateTime.Now,
                MiddleName = "Иванов",
                PersonalBookIssueDate = DateTime.Now,
                PersonalBookNumber = "132412",
                PersonalBookProtocolNumber = "13233434",
                PhotoUrl = "",
                Qualification = 1
            };

            var notExistingId = 1111;

            var diverServiceMock = new Mock<IDiverService>();
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            var divingTimeServiceMock = new Mock<IDivingTimeRepository>();
            diverServiceMock.Setup(repo => repo.EditDiverAsync(It.IsAny<int>(), It.IsAny<EditDiverModel>()))
                .Throws(new NotFoundException());
            var diversController = new DiversController(divingTimeServiceMock.Object, rescueStationServiceMock.Object, diverServiceMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => diversController.Update(notExistingId, modelEdit));
        }

        [Fact]
        public async Task Update_GivenValidInput_ShouldReturnUpdatedDiver()
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
                Qualification = 1
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
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            var divingTimeServiceMock = new Mock<IDivingTimeRepository>();
            diverServiceMock.Setup(repo => repo.EditDiverAsync(It.IsAny<int>(), It.IsAny<EditDiverModel>()))
                .Returns(Task.FromResult(model));
            var diversController = new DiversController(divingTimeServiceMock.Object, rescueStationServiceMock.Object, diverServiceMock.Object, _mapper);

            // Act
            var result = ((await diversController.Update(modelEdit.DiverId, modelEdit)) as ViewResult).Model as Diver;

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
        public async Task AddDivingTime_GivenValidInput_ShouldReturnCreatedDivingTime()
        {
            // Arrange
            var model = new DivingTime()
            {
                DiverId = 1,
                WorkingMinutes = 12,
                Year = 2000
            };

            var diverServiceMock = new Mock<IDiverService>();
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            var divingTimeServiceMock = new Mock<IDivingTimeRepository>();
            diverServiceMock.Setup(repo => repo.AddDivingTime(It.IsAny<DivingTime>()))
                .Returns(Task.CompletedTask);
            var diversController = new DiversController(divingTimeServiceMock.Object, rescueStationServiceMock.Object, diverServiceMock.Object, _mapper);

            // Act
            var result = ((await diversController.AddDivingTime(model)) as CreatedAtActionResult).Value as DivingTime;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.DiverId, result.DiverId);
            Assert.Equal(model.WorkingMinutes, result.WorkingMinutes);
            Assert.Equal(model.Year, result.Year);
        }

        [Fact]
        public async Task UploadPhoto_GivenValidInput_ShouldReturnDiver()
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

            var formFileMock = new Mock<IFormFile>();
            var ms = new MemoryStream();
            formFileMock.Setup(m => m.OpenReadStream()).Returns(ms);

            var diverServiceMock = new Mock<IDiverService>();
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            var divingTimeServiceMock = new Mock<IDivingTimeRepository>();
            diverServiceMock.Setup(repo => repo.AddPhoto(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);
            diverServiceMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(model));
            var diversController = new DiversController(divingTimeServiceMock.Object, rescueStationServiceMock.Object, diverServiceMock.Object, _mapper);

            // Act
            var result = ((await diversController.UploadPhoto(formFileMock.Object, model.DiverId)) as ViewResult).Model as Diver;

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
        public async Task UploadPhoto_GivenInvalidInput_ShouldThrowNotFoundException()
        {
            // Arrange
            var notExistingId = 1111;

            var formFileMock = new Mock<IFormFile>();
            var ms = new MemoryStream();
            formFileMock.Setup(m => m.OpenReadStream()).Returns(ms);

            var diverServiceMock = new Mock<IDiverService>();
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            var divingTimeServiceMock = new Mock<IDivingTimeRepository>();
            diverServiceMock.Setup(repo => repo.AddPhoto(It.IsAny<string>(), It.IsAny<int>()))
                .Throws(new NotFoundException());
            var diversController = new DiversController(divingTimeServiceMock.Object, rescueStationServiceMock.Object, diverServiceMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => diversController.UploadPhoto(formFileMock.Object, notExistingId));
        }

        [Fact]
        public async Task DeleteDivingTime_GivenValidInput_ShouldSuccessfullyDeleteDivingTime()
        {
            var model = new DivingTime()
            {
                DiverId = 1,
                WorkingMinutes = 12,
                Year = 2000
            };

            var diverServiceMock = new Mock<IDiverService>();
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            var divingTimeServiceMock = new Mock<IDivingTimeRepository>();
            diverServiceMock.Setup(repo => repo.DeleteDivingTime(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);
            var diversController = new DiversController(divingTimeServiceMock.Object, rescueStationServiceMock.Object, diverServiceMock.Object, _mapper);

            // Act
            await diversController.DeleteDivingTime(model.DiverId, model.Year, model.WorkingMinutes);

            // Assert
            diverServiceMock.Verify(repo => repo.DeleteDivingTime(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteDiver_GivenValidInput_ShouldSuccessfullyDeleteDiver()
        {
            var existingId = 1;

            var diverServiceMock = new Mock<IDiverService>();
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            var divingTimeServiceMock = new Mock<IDivingTimeRepository>();
            diverServiceMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);
            var diversController = new DiversController(divingTimeServiceMock.Object, rescueStationServiceMock.Object, diverServiceMock.Object, _mapper);

            // Act
            await diversController.DeleteDiver(existingId);

            // Assert
            diverServiceMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteDiver_GivenInvalidInput_ShouldThrowNotFoundException()
        {
            var notExistingId = 11111;

            var diverServiceMock = new Mock<IDiverService>();
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            var divingTimeServiceMock = new Mock<IDivingTimeRepository>();
            diverServiceMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .Throws(new NotFoundException());
            var diversController = new DiversController(divingTimeServiceMock.Object, rescueStationServiceMock.Object, diverServiceMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => diversController.DeleteDiver(notExistingId));
        }

        [Fact]
        public async Task Add_GivenValidInput_ShouldReturnCreatedDiver()
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
                Qualification = 1
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
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            var divingTimeServiceMock = new Mock<IDivingTimeRepository>();
            diverServiceMock.Setup(repo => repo.AddDiverAsync(It.IsAny<EditDiverModel>()))
                .Returns(Task.FromResult(model));
            var diversController = new DiversController(divingTimeServiceMock.Object, rescueStationServiceMock.Object, diverServiceMock.Object, _mapper);

            // Act
            var result = ((await diversController.Add(modelEdit)) as ViewResult).Model as Diver;

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
        public async Task Add_InputModelIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var diverServiceMock = new Mock<IDiverService>();
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            var divingTimeServiceMock = new Mock<IDivingTimeRepository>();
            diverServiceMock.Setup(repo => repo.AddDiverAsync(It.IsAny<EditDiverModel>()))
                .Throws(new ArgumentNullException());
            var diversController = new DiversController(divingTimeServiceMock.Object, rescueStationServiceMock.Object, diverServiceMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => diversController.Add(null));
        }

        [Fact]
        public async Task GetListJson_GivenValidInput_ShouldReturnListDiver()
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
            var rescueStationServiceMock = new Mock<IRescueStationService>();
            var divingTimeServiceMock = new Mock<IDivingTimeRepository>();
            diverServiceMock.Setup(repo => repo.GetAsync(It.IsAny<FilterOptions>()))
                .Returns(Task.FromResult((IEnumerable<Diver>)modelsFilter));
            var diversController = new DiversController(divingTimeServiceMock.Object, rescueStationServiceMock.Object, diverServiceMock.Object, _mapper);

            // Act
            var result = ((await diversController.GetListJson(options)) as JsonResult).Value as List<Diver>;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            foreach (Diver diver in result)
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
    }
}
