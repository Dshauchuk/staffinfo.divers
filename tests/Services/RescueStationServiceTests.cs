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
using System.Threading.Tasks;
using Xunit;

namespace staffinfo.divers.tests.Service
{
    public class RescueStationServiceTests
    {
        IMapper _mapper;

        public RescueStationServiceTests()
        {
            _mapper = new Mapper(new MapperConfiguration(mc => mc.AddProfile(new MappingProfile())).CreateMapper().ConfigurationProvider);
        }

        [Fact]
        public async Task AddRescueStationAsync_InputModelIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            var rescueStationService = new RescueStationService(rescueStationRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => rescueStationService.AddStationAsync(null));
        }

        [Fact]
        public async Task AddRescueStationAsync_GivenValidInput_ShouldReturnCreatedRescueStation()
        {
            // Arrange
            var model = new EditRescueStationModel()
            {
                StationId = 1,
                StationName = "Ветковская"
            };

            var modelPoco = new RescueStationPoco()
            {
                CreatedAt = DateTime.Now,
                StationId = 1,
                StationName = "Ветковская",
                UpdatedAt = DateTime.Now
            };

            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            rescueStationRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<RescueStationPoco>()))
                .Returns(Task.FromResult(modelPoco));
            var rescueStationService = new RescueStationService(rescueStationRepositoryMock.Object, _mapper);

            // Act
            var result = await rescueStationService.AddStationAsync(model);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<RescueStation>(result);
            Assert.Equal(result.StationName, model.StationName);
            Assert.Equal(result.StationId, model.StationId);
            Assert.NotNull(result.CreatedAt);
        }

        [Fact]
        public async Task DeleteAsync_GivenValidInput_ShouldSuccessfullyDeleteRescueStation()
        {
            // Arrange
            var modelPoco = new RescueStationPoco()
            {
                StationId = 1,
                StationName = "Ветковская",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            rescueStationRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(modelPoco));
            rescueStationRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
            var rescueStationService = new RescueStationService(rescueStationRepositoryMock.Object, _mapper);

            // Act
            await rescueStationService.DeleteAsync(modelPoco.StationId);

            // Assert
            rescueStationRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_GivenInvalidInput_ShouldThrowNotFoundException()
        {
            // Arrange
            var notExistingId = 1111;

            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            rescueStationRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(null as RescueStationPoco));
            rescueStationRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
            var rescueStationService = new RescueStationService(rescueStationRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => rescueStationService.DeleteAsync(notExistingId));
        }

        [Fact]
        public async Task EditRescueStationAsync_InputModelIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            var rescueStationService = new RescueStationService(rescueStationRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => rescueStationService.EditStationAsync(null));
        }

        [Fact]
        public async Task EditRescueStationAsync_GivenInvalidInput_ShouldThrowNotFoundException()
        {
            // Arrange
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            rescueStationRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(null as RescueStationPoco));
            var rescueStationService = new RescueStationService(rescueStationRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => rescueStationService.EditStationAsync(new EditRescueStationModel()));
        }

        [Fact]
        public async Task EditRescueStationAsync_GivenValidInput_ShouldReturnUpdatedRescueStation()
        {
            // Arrange
            var model = new EditRescueStationModel()
            {
                StationId = 1,
                StationName = "Ветковская"
            };

            var modelPoco = new RescueStationPoco()
            {
                StationId = 1, 
                StationName = "Ветковская",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            rescueStationRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(modelPoco));
            rescueStationRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<RescueStationPoco>()))
                .Returns(Task.FromResult(modelPoco));
            var rescueStationService = new RescueStationService(rescueStationRepositoryMock.Object, _mapper);

            // Act
            var result = await rescueStationService.EditStationAsync(model);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<RescueStation>(result);
            Assert.Equal(result.StationId, model.StationId);
            Assert.Equal(result.StationName, model.StationName);
            Assert.NotNull(result.UpdatedAt);
        }

        [Fact]
        public async Task GetAsync_GivenValidInput_ShouldReturnRescueStation()
        {
            // Arrange
            var modelPoco = new RescueStationPoco()
            {
                StationId = 1,
                StationName = "Ветковская",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            rescueStationRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(modelPoco));
            var rescueStationService = new RescueStationService(rescueStationRepositoryMock.Object, _mapper);

            // Act
            var result = await rescueStationService.GetAsync(modelPoco.StationId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<RescueStation>(result);
            Assert.Equal(result.StationId, modelPoco.StationId);
            Assert.Equal(result.StationName, modelPoco.StationName);
            Assert.Equal(result.CreatedAt.Date, modelPoco.CreatedAt.Date);
            Assert.Equal(result.UpdatedAt.Value.Date, modelPoco.UpdatedAt.Value.Date);
        }

        [Fact]
        public async Task GetAsync_GivenInvalidInput_ShouldThrowNotFoundException()
        {
            // Arrange
            var notExistingId = 1111;

            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            rescueStationRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(null as RescueStationPoco));
            var rescueStationService = new RescueStationService(rescueStationRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => rescueStationService.GetAsync(notExistingId));
        }

        [Fact]
        public async Task GetAsync_GivenValidInput_ShouldReturnListRescueStation()
        {
            // Arrange
            var modelPoco1 = new RescueStationPoco()
            {
                StationId = 1,
                StationName = "Ветковская",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var modelPoco2 = new RescueStationPoco()
            {
                StationId = 2,
                StationName = "Гомельская",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var modelPocos = new List<RescueStationPoco>()
            {
                modelPoco1,
                modelPoco2
            };
            
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            rescueStationRepositoryMock.Setup(repo => repo.GetListAsync())
                .Returns(Task.FromResult((IEnumerable<RescueStationPoco>)modelPocos));
            var rescueStationService = new RescueStationService(rescueStationRepositoryMock.Object, _mapper);

            // Act
            var result = await rescueStationService.GetAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<RescueStation>>(result);
        }

        [Fact]
        public async Task GetAsync_NoFoundElements_ShouldThrowArgumentNullException()
        {
            // Arrange
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            rescueStationRepositoryMock.Setup(repo => repo.GetListAsync())
                .Returns(Task.FromResult(null as IEnumerable<RescueStationPoco>));
            var rescueStationService = new RescueStationService(rescueStationRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => rescueStationService.GetAsync());
        }

        [Fact]
        public async Task GetAsyncPredicate_GivenValidInput_ShouldReturnListRescueStation()
        {
            // Arrange
            var modelPoco1 = new RescueStationPoco()
            {
                StationId = 1,
                StationName = "Ветковская",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            var modelPoco2 = new RescueStationPoco()
            {
                StationId = 2,
                StationName = "Гомельская",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            var modelPocos = new List<RescueStationPoco>()
            {
                modelPoco1,
                modelPoco2
            };
            Func<RescueStation, bool> predicate = x => x.StationName != "Ветковская";
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            rescueStationRepositoryMock.Setup(repo => repo.GetListAsync())
                .Returns(Task.FromResult(modelPocos as IEnumerable<RescueStationPoco>));
            var rescueStationService = new RescueStationService(rescueStationRepositoryMock.Object, _mapper);

            // Act
            var result = await rescueStationService.GetAsync(predicate);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<RescueStation>>(result);
        }

        [Fact]
        public async Task GetAsyncPredicate_NotFoundElements_ShouldThrowArgumentNullException()
        {
            // Arrange
            Func<RescueStation, bool> predicate = x => x.StationName != "Ветковская";
            var rescueStationRepositoryMock = new Mock<IRescueStationRepository>();
            rescueStationRepositoryMock.Setup(repo => repo.GetListAsync())
                .Returns(Task.FromResult(null as IEnumerable<RescueStationPoco>));
            var rescueStationService = new RescueStationService(rescueStationRepositoryMock.Object, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => rescueStationService.GetAsync(predicate));
        }
    }
}
