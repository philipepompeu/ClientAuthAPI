using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using ClientAuthAPI.Application.Services;
using ClientAuthAPI.Application.Interfaces;
using ClientAuthAPI.Application.ViewModels;
using ClientAuthAPI.Domain.Interfaces;
using ClientAuthAPI.Domain.Models;
using Bogus;

namespace ClientAuthAPI.Application.Tests.Services
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IAuthTokenService> _authTokenServiceMock;
        private readonly UserService _userService;
        private readonly Faker _faker = new Faker();

        public UserServiceTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _authTokenServiceMock = new Mock<IAuthTokenService>();
            _userService = new UserService(_userRepositoryMock.Object, _authTokenServiceMock.Object);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldCreateUserWithHashedPassword()
        {
            // Arrange
            var userName = _faker.Internet.UserName();
            var pwd = _faker.Internet.Password();

            var userViewModel = new UserViewModel { Username = userName, Password = pwd };
            var clientId = "client-1";
            var hashedPassword = "hashedPassword";
            var createdUser = new User
            {
                Username = userViewModel.Username,
                PasswordHash = hashedPassword,
                ClientId = clientId,
                CreatedAt = DateTime.UtcNow
            };

            _authTokenServiceMock.Setup(x => x.HashPassword(userViewModel.Password)).Returns(hashedPassword);
            _userRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync(createdUser);

            // Act
            var result = await _userService.CreateUserAsync(userViewModel, clientId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userViewModel.Username, result.Username);
            Assert.Equal(hashedPassword, result.PasswordHash);
            Assert.Equal(clientId, result.ClientId);
            _authTokenServiceMock.Verify(x => x.HashPassword(userViewModel.Password), Times.Once);
            _userRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task FindUserByNameAndClientId_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var username = _faker.Internet.UserName();
            var clientId = Guid.NewGuid().ToString("N");

            var user = new User { Username = username, ClientId = clientId };

            _userRepositoryMock.Setup(x => x.FindByUsernameAndClientIdAsync(username, clientId)).ReturnsAsync(user);

            // Act
            var result = await _userService.FindUserByNameAndClientId(username, clientId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(username, result.Username);
            Assert.Equal(clientId, result.ClientId);
        }

        [Fact]
        public async Task FindUserByNameAndClientId_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var username = _faker.Internet.UserName();
            var clientId = "client-1";

            _userRepositoryMock.Setup(x => x.FindByUsernameAndClientIdAsync(username, clientId)).ReturnsAsync((User?)null);

            // Act
            var result = await _userService.FindUserByNameAndClientId(username, clientId);

            // Assert
            Assert.Null(result);
        }
    }
}