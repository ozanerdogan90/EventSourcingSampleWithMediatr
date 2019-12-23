using AutoMapper;
using Clay.WebService.DataAccess.Entities;
using Clay.WebService.DataAccess.Repositories;
using Clay.WebService.Services;
using Clay.WebService.Services.Models.Errors;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Clay.WebService.Tests.Services
{
    public class DoorServiceUnitTests
    {
        private readonly IMapper _mapper;
        public DoorServiceUnitTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            _mapper = mappingConfig.CreateMapper();
        }


        [Fact]
        public void DoorService_NullServiceInjection_ThrowsError()
        {
            var repositoryMock = new Mock<IDoorRepository>();
            var mapperMock = new Mock<IMapper>();
            Assert.Throws<ArgumentNullException>(() => new DoorService(null, null, null));
            Assert.Throws<ArgumentNullException>(() => new DoorService(repositoryMock.Object, null, null));
            Assert.Throws<ArgumentNullException>(() => new DoorService(null, mapperMock.Object, null));
        }

        [Fact]
        public async Task Create_Success_ReturnsDTO()
        {
            var expected = Guid.NewGuid();
            var repositoryMock = new Mock<IDoorRepository>();
            var auditMock = new Mock<IAuditRepository>();
            repositoryMock.Setup(x => x.Create(It.IsAny<Door>())).ReturnsAsync(expected);

            var sut = new DoorService(repositoryMock.Object, _mapper, auditMock.Object);
            var actual = await sut.Create(new Models.DoorDTO());

            actual.Should().Be(expected);
        }

        [Fact]
        public async Task GetAll_Success_ReturnsDTOs()
        {
            var expected = Guid.NewGuid();
            var repositoryMock = new Mock<IDoorRepository>();
            var auditMock = new Mock<IAuditRepository>();
            repositoryMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Door> { new Door() { Id = expected } });

            var sut = new DoorService(repositoryMock.Object, _mapper, auditMock.Object);
            var actual = await sut.GetAll();

            actual.Should().NotBeNull();
            actual.Should().NotBeEmpty();
            actual.First().Should().NotBeNull();
            actual.First().Id.Should().Be(expected);
        }

        [Fact]
        public async Task GetById_Success_ReturnsDTO()
        {
            var expected = Guid.NewGuid();
            var repositoryMock = new Mock<IDoorRepository>();
            var auditMock = new Mock<IAuditRepository>();
            repositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new Door() { Id = expected });

            var sut = new DoorService(repositoryMock.Object, _mapper, auditMock.Object);
            var actual = await sut.GetById(Guid.Empty);

            actual.Should().NotBeNull();
            actual.Id.Should().Be(expected);
        }

        [Fact]
        public async Task Update_NonExistingDoor_ThrowsDoorDoesntExistError()
        {
            var expected = Guid.NewGuid();
            var repositoryMock = new Mock<IDoorRepository>();
            var auditMock = new Mock<IAuditRepository>();
            repositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(default(Door));

            var sut = new DoorService(repositoryMock.Object, _mapper, auditMock.Object);

            await Assert.ThrowsAsync<DoorDoesntExistError>(async () => await sut.Update(Guid.Empty, null));
        }

        [Fact]
        public async Task Update_Failure_ThrowsCommandExecutionFailedError()
        {
            var expected = Guid.NewGuid();
            var repositoryMock = new Mock<IDoorRepository>();
            var auditMock = new Mock<IAuditRepository>();
            repositoryMock.Setup(x => x.DoesExist(It.IsAny<Guid>())).ReturnsAsync(true);
            repositoryMock.Setup(x => x.Update(It.IsAny<Door>())).ReturnsAsync(false);

            var sut = new DoorService(repositoryMock.Object, _mapper, auditMock.Object);

            await Assert.ThrowsAsync<CommandExecutionFailedError>(async () => await sut.Update(Guid.Empty, new Models.DoorDTO()));
        }

        [Fact]
        public async Task Update_Success_ReturnsNothing()
        {
            var expected = Guid.NewGuid();
            var repositoryMock = new Mock<IDoorRepository>();
            var auditMock = new Mock<IAuditRepository>();
            repositoryMock.Setup(x => x.DoesExist(It.IsAny<Guid>())).ReturnsAsync(true);
            repositoryMock.Setup(x => x.Update(It.IsAny<Door>())).ReturnsAsync(true);

            var sut = new DoorService(repositoryMock.Object, _mapper, auditMock.Object);

            await sut.Update(Guid.Empty, new Models.DoorDTO());
        }

        [Fact]
        public async Task UpdateState_NonExistingDoor_ThrowsDoorDoesntExistError()
        {
            var expected = Guid.NewGuid();
            var repositoryMock = new Mock<IDoorRepository>();
            var auditMock = new Mock<IAuditRepository>();
            repositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(default(Door));

            var sut = new DoorService(repositoryMock.Object, _mapper, auditMock.Object);

            await Assert.ThrowsAsync<DoorDoesntExistError>(async () => await sut.UpdateState(Guid.Empty, Models.DoorStates.Unlocked));
        }

        [Fact]
        public async Task UpdateState_Failure_ThrowsCommandExecutionFailedError()
        {
            var expected = Guid.NewGuid();
            var repositoryMock = new Mock<IDoorRepository>();
            var auditMock = new Mock<IAuditRepository>();
            repositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new Door());
            repositoryMock.Setup(x => x.Update(It.IsAny<Door>())).ReturnsAsync(false);

            var sut = new DoorService(repositoryMock.Object, _mapper, auditMock.Object);

            await Assert.ThrowsAsync<CommandExecutionFailedError>(async () => await sut.UpdateState(Guid.Empty, Models.DoorStates.Unlocked));
        }

        [Fact]
        public async Task UpdateState_Success_ReturnsNothing()
        {
            var expected = Guid.NewGuid();
            var repositoryMock = new Mock<IDoorRepository>();
            var auditMock = new Mock<IAuditRepository>();
            repositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new Door());
            repositoryMock.Setup(x => x.Update(It.IsAny<Door>())).ReturnsAsync(true);

            var sut = new DoorService(repositoryMock.Object, _mapper, auditMock.Object);

            await sut.UpdateState(Guid.Empty, Models.DoorStates.Unlocked);
        }

        [Fact]
        public async Task GetAuditList_Success_ReturnsList()
        {
            var expected = Guid.NewGuid();
            var repositoryMock = new Mock<IDoorRepository>();
            var auditMock = new Mock<IAuditRepository>();
            auditMock.Setup(y => y.GetById(It.IsAny<Guid>())).ReturnsAsync(new List<Audit>() { new Audit() });

            var sut = new DoorService(repositoryMock.Object, _mapper, auditMock.Object);

            var actual = await sut.GetAuditsById(Guid.Empty);
            actual.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAuditList_Failure_ReturnsNull()
        {
            var expected = Guid.NewGuid();
            var repositoryMock = new Mock<IDoorRepository>();
            var auditMock = new Mock<IAuditRepository>();
            auditMock.Setup(y => y.GetById(It.IsAny<Guid>())).ReturnsAsync(default(List<Audit>));

            var sut = new DoorService(repositoryMock.Object, _mapper, auditMock.Object);

            var actual = await sut.GetAuditsById(Guid.Empty);
            actual.Should().BeNull();
        }
    }
}
