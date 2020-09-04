using AutoMapper;
using Moq;
using Staffinfo.Divers.Data.Poco;
using Staffinfo.Divers.Data.Repositories.Contracts;
using Staffinfo.Divers.Infrastructure.Mapping;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Services;
using Staffinfo.Divers.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace staffinfo.divers.tests.Service
{
    public class DiverServiceTests
    {
        IMapper _mapper;

        public DiverServiceTests()
        {
            _mapper = new Mapper(new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper().ConfigurationProvider);
        }

        [Fact]
        public async Task AddDiverAsync_InputModelIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var diverRepositoryMock = new Mock<IDiverRepository>();
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, rescueStationRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => diverService.AddDiverAsync(null));
        }

        [Fact]
        public async Task AddDiverAsync_GivenValidInput_ShouldReturnCreatedDiver()
        {
            // Arrange
            var model = new EditDiverModel()
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

            var modelPoco = new DiverPoco()
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
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var diverRepositoryMock = new Mock<IDiverRepository>();
            diverRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<DiverPoco>()))
                .Returns(Task.FromResult(modelPoco));
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, rescueStationRepositoryMock.Object, _mapper);

            // Act
            var result = await diverService.AddDiverAsync(model);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Diver>(result);
            Assert.Equal(result.Address, model.Address);
            Assert.Equal(result.BirthDate.Date, model.BirthDate.Value.Date);
            Assert.Equal(result.DiverId, model.DiverId);
            Assert.Equal(result.FirstName, model.FirstName);
            Assert.Equal(result.LastName, model.LastName);
            Assert.Equal(result.MiddleName, model.MiddleName);
            Assert.Equal(result.MedicalExaminationDate.Value.Date, model.MedicalExaminationDate.Value.Date);
            Assert.Equal(result.PersonalBookIssueDate.Date, model.PersonalBookIssueDate.Value.Date);
            Assert.Equal(result.PersonalBookNumber, model.PersonalBookNumber);
            Assert.Equal(result.PersonalBookProtocolNumber, model.PersonalBookProtocolNumber);
            Assert.Equal(result.PhotoUrl, model.PhotoUrl);
            Assert.Equal(result.Qualification, model.Qualification);
        }

        [Fact]
        public async Task DeleteAsync_GivenValidInput_ShouldSuccessfullyDeleteDiver()
        {
            // Arrange
            var modelPoco = new DiverPoco()
            {
                Address = "г.Ветка, ул.Батракова 32",
                BirthDate = DateTime.Now,
                DiverId = 62,
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
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var diverRepositoryMock = new Mock<IDiverRepository>();
            diverRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(modelPoco));
            diverRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, rescueStationRepositoryMock.Object, _mapper);

            // Act
            await diverService.DeleteAsync(modelPoco.DiverId);

            // Assert
            diverRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_GivenInvalidInput_ShouldThrowNotFoundException()
        {
            // Arrange
            var notExistingId = 1111;

            var diverRepositoryMock = new Mock<IDiverRepository>();
            diverRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(null as DiverPoco));
            diverRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, rescueStationRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => diverService.DeleteAsync(notExistingId));
        }

        [Fact]
        public async Task EditDiverAsync_InputModelIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var diverRepositoryMock = new Mock<IDiverRepository>();
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, rescueStationRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => diverService.EditDiverAsync(1, null));
        }

        [Fact]
        public async Task EditDiverAsync_GivenInvalidInput_ShouldThrowNotFoundException()
        {
            // Arrange
            var notExistingId = 1111;

            var diverRepositoryMock = new Mock<IDiverRepository>();
            diverRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(null as DiverPoco));
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, rescueStationRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => diverService.EditDiverAsync(notExistingId, new EditDiverModel()));
        }

        [Fact]
        public async Task EditDiverAsync_GivenValidInput_ShouldReturnUpdatedDiver()
        {
            // Arrange
            var model = new EditDiverModel()
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
                {
                    new DivingTime() { DiverId = 1, WorkingMinutes = 122, Year = 2000},
                    new DivingTime() { DiverId = 1, WorkingMinutes = 46, Year = 2001}
                }
            };

            var modelPoco = new DiverPoco()
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
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var workingTime = new List<DivingTimePoco>()
            {
                new DivingTimePoco() { DiverId = 1, WorkingMinutes = 122, Year = 2000},
                new DivingTimePoco() { DiverId = 1, WorkingMinutes = 46, Year = 2001}
            };

            var diverRepositoryMock = new Mock<IDiverRepository>();
            diverRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(modelPoco));
            diverRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<DiverPoco>()))
                .Returns(Task.FromResult(modelPoco));
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            divingTimeRepositoryMock.Setup(repo => repo.GetListAsync(It.IsAny<int>()))
                .Returns(Task.FromResult((IEnumerable<DivingTimePoco>)workingTime));
            divingTimeRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<IEnumerable<DivingTimePoco>>()))
                .Returns(Task.FromResult((IEnumerable<DivingTimePoco>)workingTime));
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, rescueStationRepositoryMock.Object, _mapper);

            // Act
            var result = await diverService.EditDiverAsync(model.DiverId, model);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Diver>(result);
            Assert.Equal(result.Address, model.Address);
            Assert.Equal(result.BirthDate.Date, model.BirthDate.Value.Date);
            Assert.Equal(result.DiverId, model.DiverId);
            Assert.Equal(result.FirstName, model.FirstName);
            Assert.Equal(result.LastName, model.LastName);
            Assert.Equal(result.MiddleName, model.MiddleName);
            Assert.Equal(result.MedicalExaminationDate.Value.Date, model.MedicalExaminationDate.Value.Date);
            Assert.Equal(result.PersonalBookIssueDate.Date, model.PersonalBookIssueDate.Value.Date);
            Assert.Equal(result.PersonalBookNumber, model.PersonalBookNumber);
            Assert.Equal(result.PersonalBookProtocolNumber, model.PersonalBookProtocolNumber);
            Assert.Equal(result.PhotoUrl, model.PhotoUrl);
            Assert.Equal(result.Qualification, model.Qualification);
        }

        [Fact]
        public async Task GetAsync_GivenValidInput_ShouldReturnDiver()
        {
            // Arrange
            var modelPoco = new DiverPoco()
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
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var diverRepositoryMock = new Mock<IDiverRepository>();
            diverRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(modelPoco));
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, rescueStationRepositoryMock.Object, _mapper);

            // Act
            var result = await diverService.GetAsync(modelPoco.DiverId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Diver>(result);
            Assert.Equal(result.Address, modelPoco.Address);
            Assert.Equal(result.BirthDate.Date, modelPoco.BirthDate.Value.Date);
            Assert.Equal(result.DiverId, modelPoco.DiverId);
            Assert.Equal(result.FirstName, modelPoco.FirstName);
            Assert.Equal(result.LastName, modelPoco.LastName);
            Assert.Equal(result.MiddleName, modelPoco.MiddleName);
            Assert.Equal(result.MedicalExaminationDate.Value.Date, modelPoco.MedicalExaminationDate.Value.Date);
            Assert.Equal(result.PersonalBookIssueDate.Date, modelPoco.PersonalBookIssueDate.Value.Date);
            Assert.Equal(result.PersonalBookNumber, modelPoco.PersonalBookNumber);
            Assert.Equal(result.PersonalBookProtocolNumber, modelPoco.PersonalBookProtocolNumber);
            Assert.Equal(result.PhotoUrl, modelPoco.PhotoUrl);
            Assert.Equal(result.Qualification, modelPoco.Qualification);
        }

        [Fact]
        public async Task GetAsync_GivenInvalidInput_ShouldThrowNotFoundException()
        {
            // Arrange
            var notExistingId = 1111;

            var diverRepositoryMock = new Mock<IDiverRepository>();
            diverRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(null as DiverPoco));
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, rescueStationRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => diverService.GetAsync(notExistingId));
        }

        [Fact]
        public async Task GetAsync_GivenValidInput_ShouldReturnListDiver()
        {
            // Arrange
            var modelPoco1 = new DiverPoco()
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
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            var modelPoco2 = new DiverPoco()
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
                RescueStationId = 127,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            var modelPocos = new List<DiverPoco>()
            {
                modelPoco1,
                modelPoco2
            };
            var modelFilterPocos = new List<DiverPoco>()
            {
                modelPoco1
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

            var diverRepositoryMock = new Mock<IDiverRepository>();
            diverRepositoryMock.Setup(repo => repo.GetListAsync())
                .Returns(Task.FromResult((IEnumerable<DiverPoco>)modelPocos));
            diverRepositoryMock.Setup(repo => repo.GetListAsync(It.IsAny<FilterOptions>()))
                .Returns(Task.FromResult((IEnumerable<DiverPoco>)modelFilterPocos));
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, rescueStationRepositoryMock.Object, _mapper);

            // Act
            var result = await diverService.GetAsync(options);
            var resultWithoutOptions = await diverService.GetAsync(null);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(resultWithoutOptions);
            Assert.IsAssignableFrom<IEnumerable<Diver>>(result);
            Assert.IsAssignableFrom<IEnumerable<Diver>>(resultWithoutOptions);
        }

        [Fact]
        public async Task AddDivingTimeAsync_GivenValidInput_ShouldSuccessfullyCreateDivingTime()
        {
            // Arrange
            var modelPoco = new DiverPoco()
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
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var workingTime = new List<DivingTimePoco>()
            {
                new DivingTimePoco() { DiverId = 1, WorkingMinutes = 122, Year = 2000},
                new DivingTimePoco() { DiverId = 1, WorkingMinutes = 46, Year = 2001}
            };

            var divingTime = new DivingTime()
            {
                DiverId = 1,
                WorkingMinutes = 22,
                Year = 2002
            };

            var workingTimeAfterAdd = new List<DivingTimePoco>()
            {
                new DivingTimePoco() { DiverId = 1, WorkingMinutes = 122, Year = 2000},
                new DivingTimePoco() { DiverId = 1, WorkingMinutes = 46, Year = 2001},
                new DivingTimePoco() { DiverId = 1, WorkingMinutes = 22, Year = 2002}
            };

            var diverRepositoryMock = new Mock<IDiverRepository>();
            diverRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<DiverPoco>()))
                .Returns(Task.FromResult(modelPoco));
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            divingTimeRepositoryMock.Setup(repo => repo.GetListAsync(It.IsAny<int>()))
                .Returns(Task.FromResult((IEnumerable<DivingTimePoco>)workingTime));
            divingTimeRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<IEnumerable<DivingTimePoco>>()))
                .Returns(Task.FromResult((IEnumerable<DivingTimePoco>)workingTimeAfterAdd));
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, rescueStationRepositoryMock.Object, _mapper);

            // Act
            await diverService.AddDivingTimeAsync(divingTime);

            // Assert
            divingTimeRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<IEnumerable<DivingTimePoco>>()), Times.Once);
        }

        [Fact]
        public async Task AddPhotoAsync_GivenInvalidInput_ShouldThrowNotFoundException()
        {
            // Arrange
            var notExistingId = 1111;
            var notExistingphotoBase64 = "qwerty123";

            var diverRepositoryMock = new Mock<IDiverRepository>();
            diverRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(null as DiverPoco));
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, rescueStationRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => diverService.AddPhotoAsync(notExistingphotoBase64, notExistingId));
        }

        [Fact]
        public async Task AddPhotoAsync_GivenValidInput_ShouldSuccessfullyUpdateDiver()
        {
            // Arrange
            var modelPoco = new DiverPoco()
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
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var photoBase64 = "qwerty123";

            var diverRepositoryMock = new Mock<IDiverRepository>();
            diverRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(modelPoco));
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, rescueStationRepositoryMock.Object, _mapper);

            // Act
            await diverService.AddPhotoAsync(photoBase64, modelPoco.DiverId);

            // Assert
            diverRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<DiverPoco>()), Times.Once);
        }

        [Fact]
        public async Task DeletePhotoAsync_GivenInvalidInput_ShouldThrowNotFoundException()
        {
            // Arrange
            var notExistingId = 1111;

            var diverRepositoryMock = new Mock<IDiverRepository>();
            diverRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(null as DiverPoco));
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, rescueStationRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => diverService.DeletePhotoAsync(notExistingId));
        }

        [Fact]
        public async Task DeletePhotoAsync_GivenValidInput_ShouldSuccessfullyDeletePhotoAsync()
        {
            var modelPoco = new DiverPoco()
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
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var diverRepositoryMock = new Mock<IDiverRepository>();
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            diverRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(modelPoco as DiverPoco));
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, rescueStationRepositoryMock.Object, _mapper);

            // Act
            await diverService.DeletePhotoAsync(modelPoco.DiverId);

            // Assert
            diverRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<DiverPoco>()), Times.Once);
        }

        [Fact]
        public async Task DeleteDivingTimeAsync_GivenValidInput_ShouldSuccessfullyDeleteDivingTime()
        {
            // Arrange
            var workingTime = new List<DivingTimePoco>()
            {
                new DivingTimePoco() { DiverId = 1, WorkingMinutes = 122, Year = 2000},
                new DivingTimePoco() { DiverId = 1, WorkingMinutes = 46, Year = 2001}
            };

            var divingTime = new DivingTime()
            {
                DiverId = 1,
                WorkingMinutes = 22,
                Year = 2002
            };

            var workingTimeAfterAdd = new List<DivingTimePoco>()
            {
                new DivingTimePoco() { DiverId = 1, WorkingMinutes = 122, Year = 2000},
                new DivingTimePoco() { DiverId = 1, WorkingMinutes = 46, Year = 2001},
                new DivingTimePoco() { DiverId = 1, WorkingMinutes = 22, Year = 2002}
            };

            var diverRepositoryMock = new Mock<IDiverRepository>();
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            divingTimeRepositoryMock.Setup(repo => repo.GetListAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(workingTime as IEnumerable<DivingTimePoco>));
            divingTimeRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<IEnumerable<DivingTimePoco>>()))
                .Returns(Task.FromResult(workingTimeAfterAdd as IEnumerable<DivingTimePoco>));
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, rescueStationRepositoryMock.Object, _mapper);

            // Act
            await diverService.DeleteDivingTimeAsync(divingTime.DiverId, divingTime.Year);

            // Assert
            divingTimeRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<IEnumerable<DivingTimePoco>>()), Times.Once);
        }

        [Fact]
        public async Task GetDiversPerStationAsync_NoInput_ShouldReturnListDiversPerStationModel()
        {
            // Arrange
            var modelStationPoco = new RescueStationPoco()
            {
                CreatedAt = DateTime.Now,
                StationId = 1,
                StationName = "Ветковская",
                UpdatedAt = DateTime.Now
            };

            var modelDiverPoco = new DiverPoco()
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
                RescueStationId = 1,
                RescueStation = modelStationPoco,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var modelDiverPocos = new List<DiverPoco>()
            {
                modelDiverPoco
            };

            var modelStationsPocos = new List<RescueStationPoco>()
            {
                modelStationPoco
            };

            var diversPerStationModel = new MinStationModel()
            {
                Id = modelStationPoco.StationId,
                Name = modelStationPoco.StationName,
                DiversCount = modelDiverPocos.Where(c => c.RescueStation.StationId == modelStationPoco.StationId).Count()
            };

            var diverRepositoryMock = new Mock<IDiverRepository>();
            diverRepositoryMock.Setup(repo => repo.GetListAsync())
                .Returns(Task.FromResult((IEnumerable<DiverPoco>)modelDiverPocos));
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            rescueStationRepositoryMock.Setup(repo => repo.GetListAsync())
                .Returns(Task.FromResult((IEnumerable<RescueStationPoco>)modelStationsPocos));
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, rescueStationRepositoryMock.Object, _mapper);

            // Act
            var result = await diverService.GetDiversPerStationAsync();

            // Act & Assert
            Assert.NotNull(result);
            Assert.IsType<List<MinStationModel>>(result);
            Assert.Equal(result[0].Id, diversPerStationModel.Id);
            Assert.Equal(result[0].Name, diversPerStationModel.Name);
            Assert.Equal(result[0].DiversCount, diversPerStationModel.DiversCount);
        }

        [Fact]
        public async Task GetDivingTimePerStationAsync_NoInput_ShouldReturnListDivingTimePerStationModel()
        {
            // Arrange
            var modelStationPoco = new RescueStationPoco()
            {
                CreatedAt = DateTime.Now,
                StationId = 1,
                StationName = "Ветковская",
                UpdatedAt = DateTime.Now
            };

            var modelDiverPoco = new DiverPoco()
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
                RescueStationId = 1,
                RescueStation = modelStationPoco,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                WorkingTime = new List<DivingTimePoco>()
                {
                    new DivingTimePoco() { DiverId = 1, WorkingMinutes = 122, Year = 2000},
                    new DivingTimePoco() { DiverId = 1, WorkingMinutes = 46, Year = 2001}
                }
            };

            var modelDiverPocos = new List<DiverPoco>()
            {
                modelDiverPoco
            };

            var modelStationsPocos = new List<RescueStationPoco>()
            {
                modelStationPoco
            };

            var divingTimePerStationModel = new StationDivingTimeModel()
            {
                Name = modelStationPoco.StationName,
                TotalDivingTime = modelDiverPoco.WorkingTime.Sum(c => c.WorkingMinutes)
            };

            var diverRepositoryMock = new Mock<IDiverRepository>();
            diverRepositoryMock.Setup(repo => repo.GetListAsync())
                .Returns(Task.FromResult((IEnumerable<DiverPoco>)modelDiverPocos));
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            rescueStationRepositoryMock.Setup(repo => repo.GetListAsync())
                .Returns(Task.FromResult((IEnumerable<RescueStationPoco>)modelStationsPocos));
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, rescueStationRepositoryMock.Object, _mapper);

            // Act
            var result = await diverService.GetDivingTimePerStationAsync();

            // Act & Assert
            Assert.NotNull(result);
            Assert.IsType<List<StationDivingTimeModel>>(result);
            Assert.Equal(result[0].Name, divingTimePerStationModel.Name);
            Assert.Equal(result[0].TotalDivingTime, divingTimePerStationModel.TotalDivingTime);
        }

        [Fact]
        public async Task GetAverageDivingTimePerStationAsync_NoInput_ShouldReturnListAverageDivingTimePerStationModel()
        {
            // Arrange
            var modelStationPoco = new RescueStationPoco()
            {
                CreatedAt = DateTime.Now,
                StationId = 1,
                StationName = "Ветковская",
                UpdatedAt = DateTime.Now
            };

            var modelDiverPoco = new DiverPoco()
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
                RescueStationId = 1,
                RescueStation = modelStationPoco,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                WorkingTime = new List<DivingTimePoco>()
                {
                    new DivingTimePoco() { DiverId = 1, WorkingMinutes = 122, Year = 2000},
                    new DivingTimePoco() { DiverId = 1, WorkingMinutes = 46, Year = 2001}
                }
            };

            var modelDiverPocos = new List<DiverPoco>()
            {
                modelDiverPoco
            };

            var modelStationsPocos = new List<RescueStationPoco>()
            {
                modelStationPoco
            };

            var averageDivingTimePerStationModel = new AverageStationDivingTimeModel()
            {
                Name = modelStationPoco.StationName,
                DiveNumber = modelDiverPoco.WorkingTime.Count,
                AverageDivingTime = Math.Round(modelDiverPoco.WorkingTime.Sum(c => c.WorkingMinutes) / (double)modelDiverPoco.WorkingTime.Count, 1)
            };

            var diverRepositoryMock = new Mock<IDiverRepository>();
            diverRepositoryMock.Setup(repo => repo.GetListAsync())
                .Returns(Task.FromResult((IEnumerable<DiverPoco>)modelDiverPocos));
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            rescueStationRepositoryMock.Setup(repo => repo.GetListAsync())
                .Returns(Task.FromResult((IEnumerable<RescueStationPoco>)modelStationsPocos));
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, rescueStationRepositoryMock.Object, _mapper);

            // Act
            var result = await diverService.GetAverageDivingTimePerStationAsync();

            // Act & Assert
            Assert.NotNull(result);
            Assert.IsType<List<AverageStationDivingTimeModel>>(result);
            Assert.Equal(result[0].Name, averageDivingTimePerStationModel.Name);
            Assert.Equal(result[0].DiveNumber, averageDivingTimePerStationModel.DiveNumber);
            Assert.Equal(result[0].AverageDivingTime, averageDivingTimePerStationModel.AverageDivingTime);
        }
    }
}
