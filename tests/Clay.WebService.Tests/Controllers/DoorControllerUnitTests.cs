using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Clay.WebService.Controllers;
using Clay.WebService.Models;
using Clay.WebService.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Clay.WebService.Tests.Controllers
{
    public class DoorControllerUnitTest
    {
        [Fact]
        public void DoorController_NullServiceInjection_ThrowsError()
        {
            Assert.Throws<ArgumentNullException>(() => new DoorController(null));
        }

        [Fact]
        public async Task GetAll_Success_ReturnsAllDoors()
        {
            var expected = new List<DoorDTO>()
            {
                new DoorDTO(),
                new DoorDTO()
        };
            var mock = new Mock<IDoorService>();
            mock.Setup(x => x.GetAll()).ReturnsAsync(expected);

            var sut = new DoorController(mock.Object);
            var actual = (await sut.GetAll()) as ObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().NotBeNull();
            (actual.Value as ICollection<DoorDTO>).Should().HaveSameCount(expected);
        }

        [Fact]
        public async Task GetAll_EmptyList_ReturnsNotFound()
        {
            var mock = new Mock<IDoorService>();
            mock.Setup(x => x.GetAll()).ReturnsAsync(new List<DoorDTO>());

            var sut = new DoorController(mock.Object);
            var actual = (await sut.GetAll()) as NotFoundResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetById_Success_ReturnOk()
        {
            var expected = new DoorDTO() { Name = "newName", Id = Guid.NewGuid() };
            var mock = new Mock<IDoorService>();
            mock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(expected);

            var sut = new DoorController(mock.Object);
            var actual = (await sut.GetById(Guid.Empty)) as ObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().NotBeNull();
            actual.Value.As<DoorDTO>().Name.Should().Be(expected.Name);
        }

        [Fact]
        public async Task GetById_NullValue_ReturnsNotFound()
        {
            var mock = new Mock<IDoorService>();
            mock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(default(DoorDTO));

            var sut = new DoorController(mock.Object);
            var actual = (await sut.GetById(Guid.Empty)) as NotFoundResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_Success_ReturnsCreated()
        {
            var mock = new Mock<IDoorService>();
            mock.Setup(x => x.Create(It.IsAny<DoorDTO>())).ReturnsAsync(Guid.NewGuid());

            var sut = new DoorController(mock.Object);
            var actual = (await sut.Create(null)) as CreatedResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.Created);
        }

        [Fact]
        public async Task UpdateState_Success_ReturnsNoContent()
        {
            var mock = new Mock<IDoorService>();

            var sut = new DoorController(mock.Object);
            var actual = (await sut.UpdateState(Guid.Empty, DoorStates.Locked)) as NoContentResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Update_Success_ReturnsNoContent()
        {
            var mock = new Mock<IDoorService>();

            var sut = new DoorController(mock.Object);
            var actual = (await sut.UpdateDoor(Guid.Empty, null)) as NoContentResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }


        [Fact]
        public async Task GetAuditList_Success_ReturnsList()
        {
            var mock = new Mock<IDoorService>();
            mock.Setup(y => y.GetAuditsById(It.IsAny<Guid>())).ReturnsAsync(new List<AuditDTO>());

            var sut = new DoorController(mock.Object);
            var actual = (await sut.GetAuditList(Guid.Empty)) as ObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetAuditList_Failure_ReturnsNotFound()
        {
            var mock = new Mock<IDoorService>();
            mock.Setup(y => y.GetAuditsById(It.IsAny<Guid>())).ReturnsAsync(default(List<AuditDTO>));

            var sut = new DoorController(mock.Object);
            var actual = (await sut.GetAuditList(Guid.Empty)) as NotFoundResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
