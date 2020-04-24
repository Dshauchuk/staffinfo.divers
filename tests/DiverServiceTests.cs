using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Staffinfo.Divers.Data.Repositories.Contracts;
using Staffinfo.Divers.Services;
using Staffinfo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Staffinfo.Divers.Models;
using Staffinfo.Divers.Data.Poco;
using Staffinfo.Divers;
using Staffinfo.Divers.Infrastructure.Mapping;
using Moq;
using Staffinfo.Divers.Shared.Exceptions;
using Staffinfo.Divers.Models.Abstract;

namespace staffinfo.divers.tests
{
    public class DiverServiceTests
    {
        IMapper _mapper;

        public DiverServiceTests()
        {
            _mapper = new Mapper(new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper().ConfigurationProvider);
        }
        /* public async Task<Diver> AddDiverAsync(EditDiverModel model)
         {
             if (model == null)
                 throw new ArgumentNullException(nameof(model));

             var poco = _mapper.Map<DiverPoco>(model);

             var added = await _diverRepository.AddAsync(poco);
             var diver = _mapper.Map<Diver>(added);

             return diver;
         }*/

        [Fact]
        public async Task AddDiverAsync_InputModelIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var diverRepositoryMock = new Mock<IDiverRepository>();
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => diverService.AddDiverAsync(null));
        }

        [Fact]
        public async Task AddDiverAsync_CorrectAdd_ShouldDiver()
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
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, _mapper);

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

        //[Fact]
        //public async Task AddDiverAsync_InputInvalidData_ShouldThrowArgumentNullException()
        //{
        //    // Arrange
        //    var model = new EditDiverModel()
        //    {
        //        Address = "г.Ветка, ул.Батракова 32",
        //        BirthDate = DateTime.Now,
        //        DiverId = 1,
        //        FirstName = "Иван",
        //        LastName = "Иванов",
        //        MedicalExaminationDate = DateTime.Now,
        //        MiddleName = "Иванов",
        //        PersonalBookIssueDate = DateTime.Now,
        //        PersonalBookNumber = "132412",
        //        PersonalBookProtocolNumber = "132334334444",
        //        PhotoUrl = "",
        //        Qualification = 1,
        //        RescueStationId = 127,
        //        WorkingTime = new List<DivingTime>()
        //    };

        //    var modelPoco = new DiverPoco()
        //    {
        //        Address = "г.Ветка, ул.Батракова 32",
        //        BirthDate = DateTime.Now,
        //        DiverId = 1,
        //        FirstName = "Иван",
        //        LastName = "Иванов",
        //        MedicalExaminationDate = DateTime.Now,
        //        MiddleName = "Иванов",
        //        PersonalBookIssueDate = DateTime.Now,
        //        PersonalBookNumber = "132412",
        //        PersonalBookProtocolNumber = "13233434",
        //        PhotoUrl = "",
        //        Qualification = 1,
        //        RescueStationId = 127,
        //        CreatedAt = DateTime.Now,
        //        UpdatedAt = DateTime.Now
        //    };

        //    var diverRepositoryMock = new Mock<IDiverRepository>();
        //    diverRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<DiverPoco>()))
        //        .Returns(Task.FromResult(modelPoco));
        //    var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
        //    var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, _mapper);

        //    // Act
        //    var result = await diverService.AddDiverAsync(model);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.IsType<Diver>(result);
        //    Assert.Equal(result.Address, model.Address);
        //    Assert.Equal(result.BirthDate.Date, model.BirthDate.Value.Date);
        //    Assert.Equal(result.DiverId, model.DiverId);
        //    Assert.Equal(result.FirstName, model.FirstName);
        //    Assert.Equal(result.LastName, model.LastName);
        //    Assert.Equal(result.MiddleName, model.MiddleName);
        //    Assert.Equal(result.MedicalExaminationDate.Value.Date, model.MedicalExaminationDate.Value.Date);
        //    Assert.Equal(result.PersonalBookIssueDate.Date, model.PersonalBookIssueDate.Value.Date);
        //    Assert.Equal(result.PersonalBookNumber, model.PersonalBookNumber);
        //    Assert.Equal(result.PersonalBookProtocolNumber, model.PersonalBookProtocolNumber);
        //    Assert.Equal(result.PhotoUrl, model.PhotoUrl);
        //    Assert.Equal(result.Qualification, model.Qualification);
        //}

        [Fact]
        public async Task DeleteAsync_CorrectDelete_Success()
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
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, _mapper);

            // Act
            await diverService.DeleteAsync(modelPoco.DiverId);

            // Assert
            diverRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_InputModelIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var badDiverId = 11111;

            var diverRepositoryMock = new Mock<IDiverRepository>();
            diverRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(null as DiverPoco));
            diverRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => diverService.DeleteAsync(badDiverId));
        }

        [Fact]
        public async Task EditDiverAsync_InputModelIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var diverRepositoryMock = new Mock<IDiverRepository>();
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => diverService.EditDiverAsync(1, null));
        }

        [Fact]
        public async Task EditDiverAsync_InvalidParameter_ShouldThrowNotFoundException()
        {
            // Arrange
            var diverRepositoryMock = new Mock<IDiverRepository>();
            diverRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(null as DiverPoco));
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => diverService.EditDiverAsync(111111, new EditDiverModel()));
        }

        [Fact]
        public async Task EditDiverAsync_CorrectEdit_ShouldDiver()
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
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, _mapper);

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
        public async Task GetAsync_CorrectGet_ShouldReturnDiver()
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
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, _mapper);

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
        public async Task GetAsync_InvalidParameter_ShouldThrowNotFoundException()
        {
            // Arrange
            var diverRepositoryMock = new Mock<IDiverRepository>();
            diverRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(null as DiverPoco));
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => diverService.GetAsync(111111));
        }

        [Fact]
        public async Task GetAsync_CorrectGet_ShouldReturnListDiver()
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
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, _mapper);

            // Act
            var result = await diverService.GetAsync(options);
            var resultWithoutOptions = await diverService.GetAsync(null);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(resultWithoutOptions);
            Assert.IsAssignableFrom<IEnumerable<Diver>>(result);
            Assert.IsAssignableFrom<IEnumerable<Diver>>(resultWithoutOptions);
            //Assert.Equal(result, modelFilterPocos);
        }

        [Fact]
        public async Task AddDivingTime_CorrectAdd_ShouldReturnDivingTime()
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
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, _mapper);

            // Act
            await diverService.AddDivingTime(divingTime);

            // Assert
            divingTimeRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<DivingTimePoco>()), Times.Once);
        }

        [Fact]
        public async Task AddPhoto_InputModelIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var diverRepositoryMock = new Mock<IDiverRepository>();
            diverRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(null as DiverPoco));
            var divingTimeRepositoryMock = new Mock<IDivingTimeRepository>();
            var diverService = new DiverService(diverRepositoryMock.Object, divingTimeRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => diverService.AddPhoto("", 1111));
        }
    }
}
