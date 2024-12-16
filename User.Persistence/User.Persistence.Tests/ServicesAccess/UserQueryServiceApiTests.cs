using FluentAssertions;
using Moq;
using RichardSzalay.MockHttp;
using Serilog;
using System.Net;
using User.Persistence.Domain.ResultServices;
using User.Persistence.Infrastructure.ServicesAccess;

namespace User.Persistence.Tests.ServicesAccess;
public class UserQueryServiceApiTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<ILogger> _loggerMock;
    private readonly UserQueryServiceApi _service;
    private const string BaseAddress = "http://localhost";

    public UserQueryServiceApiTests()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _loggerMock = new Mock<ILogger>();
        _service = new UserQueryServiceApi(_httpClientFactoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ThereIsUserWithEmailAsync_ShouldReturnSuccessResult_WhenApiCallSucceeds()
    {
        // Arrange
        var email = "test@example.com";

        var apiResponse = new Result<ThereIsUserResult>(new ThereIsUserResult() { ThereIsUser = true }, true, string.Empty);

        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(apiResponse);

        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(jsonResponse)
        };

        var mockHttp = new MockHttpMessageHandler();

        var endpoint = $"/api/v1/user/there-is-user/{email}";

        mockHttp
            .When($"{BaseAddress}{endpoint}")
            .Respond("application/json", jsonResponse);

        var httpClient = mockHttp.ToHttpClient();

        httpClient.BaseAddress = new Uri(BaseAddress);

        _httpClientFactoryMock
            .Setup(factory => factory.CreateClient("UserQueryApi"))
            .Returns(httpClient);

        // Act
        var result = await _service.ThereIsUserWithEmailAsync(email);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.ThereIsUser.Should().BeTrue();
        _loggerMock.Verify(logger => logger.Information(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task ThereIsUserWithEmailAsync_ShouldReturnFailureResult_WhenApiCallFails()
    {
        // Arrange
        var email = "test@example.com";

        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest,
            Content = new StringContent("Bad Request")
        };

        var mockHttp = new MockHttpMessageHandler();

        var endpoint = $"/api/v1/user/there-is-user/{email}";

        mockHttp
            .When($"{BaseAddress}{endpoint}")
            .Respond(httpResponseMessage);

        var httpClient = mockHttp.ToHttpClient();

        httpClient.BaseAddress = new Uri(BaseAddress);

        _httpClientFactoryMock
            .Setup(factory => factory.CreateClient("UserQueryApi"))
            .Returns(httpClient);

        // Act
        var result = await _service.ThereIsUserWithEmailAsync(email);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("StatusCode: BadRequest");
        _loggerMock.Verify(logger => logger.Error(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task RecoverByEmailAsync_ShouldReturnSuccessResult_WhenApiCallSucceeds()
    {
        // Arrange
        var email = "test@example.com";

        var apiResponse = new Result<UserResult>(new UserResult()
        {
            Id = Guid.NewGuid(),
            RegistrationDate = DateTime.UtcNow,
            Name = "John Cena",
            Email = email,
            Password = "any-encrypted-password"
        }, true, string.Empty);

        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(apiResponse);

        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(jsonResponse)
        };

        var mockHttp = new MockHttpMessageHandler();

        var endpoint = $"/api/v1/user/{email}";

        mockHttp
            .When($"{BaseAddress}{endpoint}")
            .Respond("application/json", jsonResponse);

        var httpClient = mockHttp.ToHttpClient();

        httpClient.BaseAddress = new Uri(BaseAddress);

        _httpClientFactoryMock
            .Setup(factory => factory.CreateClient("UserQueryApi"))
            .Returns(httpClient);

        // Act
        var result = await _service.RecoverByEmailAsync(email);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Email.Should().Be(email);
        _loggerMock.Verify(logger => logger.Information(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task RecoverByEmailAsync_ShouldReturnFailureResult_WhenApiCallFails()
    {
        // Arrange
        var email = "test@example.com";

        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound,
            Content = new StringContent("Not Found")
        };

        var mockHttp = new MockHttpMessageHandler();

        var endpoint = $"/api/v1/user/{email}";

        mockHttp
            .When($"{BaseAddress}{endpoint}")
            .Respond(httpResponseMessage);

        var httpClient = mockHttp.ToHttpClient();

        httpClient.BaseAddress = new Uri(BaseAddress);

        _httpClientFactoryMock
            .Setup(factory => factory.CreateClient("UserQueryApi"))
            .Returns(httpClient);

        // Act
        var result = await _service.RecoverByEmailAsync(email);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("StatusCode: NotFound");
        _loggerMock.Verify(logger => logger.Error(It.IsAny<string>()), Times.Once);
    }
}