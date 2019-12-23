using Clay.WebService.Controllers;
using Clay.WebService.Models;
using Clay.WebService.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Clay.WebService.Tests.Controllers
{
    public class UserControllerUnitTests
    {
        [Fact]
        public void UserController_NullServiceInjection_ThrowsError()
        {
            Assert.Throws<ArgumentNullException>(() => new UserController(null));
        }

        [Fact]
        public async Task Create_Success_ReturnsCreated()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(x => x.Register(It.IsAny<UserDTO>())).ReturnsAsync(Guid.NewGuid());

            var sut = new UserController(mock.Object);
            var actual = (await sut.Create(null)) as CreatedResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.Created);
        }
        [Fact]
        public async Task Update_Success_ReturnsCreated()
        {
            var mock = new Mock<IUserService>();

            var sut = new UserController(mock.Object);
            var actual = (await sut.Update(Guid.Empty, null)) as NoContentResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

    }
}
