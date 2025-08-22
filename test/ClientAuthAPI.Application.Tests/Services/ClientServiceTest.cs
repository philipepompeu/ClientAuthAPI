using System.Threading.Tasks;
using Xunit;
using Moq;
using ClientAuthAPI.Application.Services;
using ClientAuthAPI.Application.ViewModels;
using ClientAuthAPI.Domain.Interfaces;
using ClientAuthAPI.Domain.Models;

namespace ClientAuthAPI.Application.Tests.Services
{
    public class ClientServiceTest
    {
        private readonly ClientService _clientService;
        private readonly Mock<IClientRepository> _mockRepo;

        public ClientServiceTest()
        {
            _mockRepo = new Mock<IClientRepository>();
            _clientService = new ClientService(_mockRepo.Object);
        }

        [Fact]
        public async Task ProvisionClientAsync_ShouldCreateClientWithExpectedProperties()
        {
            // Arrange
            Client createdClient = new Client
            {
                Name = "UniqueClient",
                ClientId = Guid.NewGuid().ToString("N"),
                ClientSecret = "XPTO"
            };
            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Client>()))                
                .ReturnsAsync(createdClient);

            var viewModel = new ClientViewModel { Name = "TestClient" };

            // Act
            var result = await _clientService.ProvisionClientAsync(viewModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("TestClient", result.Name);
            Assert.False(string.IsNullOrWhiteSpace(result.ClientId));
            Assert.False(string.IsNullOrWhiteSpace(result.ClientSecret));            
        }

        [Fact]
        public async Task ProvisionClientAsync_ShouldCallRepositoryCreateAsyncOnce()
        {
            var client = new Client
            {
                Name = "UniqueClient",
                ClientId = Guid.NewGuid().ToString("N"),
                ClientSecret = "XPTO"
            };
            // Ar
            // Arrange
            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Client>()))
                .ReturnsAsync(client)
                .Verifiable();

            var viewModel = new ClientViewModel { Name = "AnotherClient" };

            // Act
            await _clientService.ProvisionClientAsync(viewModel);

            // Assert
            _mockRepo.Verify(r => r.CreateAsync(It.IsAny<Client>()), Times.Once);
        }

        [Fact]
        public async Task ProvisionClientAsync_ShouldGenerateUniqueClientIdAndSecret()
        {

            var client = new Client
            {
                Name = "UniqueClient",
                ClientId = Guid.NewGuid().ToString("N"),
                ClientSecret = "XPTO"
            };
            // Arrange
            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Client>()))
                .ReturnsAsync(client);

            var viewModel = new ClientViewModel { Name = "UniqueClient" };

            // Act
            var client1 = await _clientService.ProvisionClientAsync(viewModel);
            var client2 = await _clientService.ProvisionClientAsync(viewModel);

            // Assert
            Assert.NotEqual(client1.ClientId, client2.ClientId);
            Assert.NotEqual(client1.ClientSecret, client2.ClientSecret);
        }
    }
}