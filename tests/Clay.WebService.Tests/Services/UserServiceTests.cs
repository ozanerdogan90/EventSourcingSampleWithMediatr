using AutoMapper;
using Clay.WebService.DataAccess.Entities;
using Clay.WebService.DataAccess.Repositories;
using Clay.WebService.Models;
using Clay.WebService.Services;
using Clay.WebService.Services.Helpers;
using Clay.WebService.Services.Models.Errors;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Clay.WebService.Tests.Services
{
    public class UserServiceTests
    {
        private readonly IMapper _mapper;
        public UserServiceTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            _mapper = mappingConfig.CreateMapper();
        }
        [Fact]
        public void UserService_NullInjections_ThrowsError()
        {
            var encMock = new Mock<IEncryptionService>();
            var repositoryMock = new Mock<IUserRepository>();
            Assert.Throws<ArgumentNullException>(() => new UserService(null, null, null));
            Assert.Throws<ArgumentNullException>(() => new UserService(null, repositoryMock.Object, _mapper));
            Assert.Throws<ArgumentNullException>(() => new UserService(encMock.Object, null, _mapper));
            Assert.Throws<ArgumentNullException>(() => new UserService(encMock.Object, repositoryMock.Object, null));
        }


        [Fact]
        public async Task Register_Success_ReturnsId()
        {
            var expected = Guid.NewGuid();
            var encMock = new Mock<IEncryptionService>();
            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(x => x.Add(It.IsAny<User>())).ReturnsAsync(expected);
            encMock.Setup(x => x.HashPassword(It.IsAny<string>())).Returns(string.Empty);

            var sut = new UserService(encMock.Object, repositoryMock.Object, _mapper);
            var actual = await sut.Register(new UserDTO());

            actual.Should().Be(actual);
        }

        [Fact]
        public async Task Register_Failure_ThrowsError()
        {
            var encMock = new Mock<IEncryptionService>();
            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(x => x.Add(It.IsAny<User>())).ReturnsAsync(default(Guid?));
            encMock.Setup(x => x.HashPassword(It.IsAny<string>())).Returns(string.Empty);

            var sut = new UserService(encMock.Object, repositoryMock.Object, _mapper);

            await Assert.ThrowsAsync<CommandExecutionFailedError>(async () => await sut.Register(new UserDTO()));

        }

        [Fact]
        public async Task Get_Failure_ReturnsNull()
        {
            var encMock = new Mock<IEncryptionService>();
            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>())).ReturnsAsync(default(User));
            encMock.Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            var sut = new UserService(encMock.Object, repositoryMock.Object, _mapper);
            var actual = await sut.Get(string.Empty);

            actual.Should().BeNull();
        }

        [Fact]
        public async Task Get_Success_ReturnsUser()
        {
            var encMock = new Mock<IEncryptionService>();
            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>())).ReturnsAsync(new User());
            encMock.Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var sut = new UserService(encMock.Object, repositoryMock.Object, _mapper);
            var actual = await sut.Get(string.Empty);

            actual.Should().NotBeNull();
        }

        [Fact]
        public async Task Update_Success_ReturnsNothing()
        {
            var encMock = new Mock<IEncryptionService>();
            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(x => x.Update(It.IsAny<User>())).ReturnsAsync(true);
            repositoryMock.Setup(x => x.DoesExist(It.IsAny<Guid>())).ReturnsAsync(true);

            var sut = new UserService(encMock.Object, repositoryMock.Object, _mapper);
            await sut.Update(Guid.Empty, new UserDTO());
        }

        [Fact]
        public async Task Update_Failure_ThrowsError()
        {
            var encMock = new Mock<IEncryptionService>();
            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(x => x.Add(It.IsAny<User>())).ReturnsAsync(default(Guid?));

            var sut = new UserService(encMock.Object, repositoryMock.Object, _mapper);

            await Assert.ThrowsAsync<UserDoesntExistError>(async () => await sut.Update(Guid.NewGuid(), new UserDTO()));
        }
    }
}
