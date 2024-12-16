using FluentAssertions;
using System.Net.Http.Json;
using User.Persistence.Communication.Response;
using User.Persistence.Integration.Tests.Fakes.Request;

namespace User.Persistence.Integration.Tests.Api.Controllers.v1.Register;
public class UserControllerTests() : BaseTestClient("/api/v1/user")
{
    [Fact]
    public async Task UserController_ReturnsError_WhenUserAlreadyExists()
    {
        // Arrange
        var user = Factory.RecoverUser();
        var password = Factory.RecoverPassword();

        var request = new RequestRegisterUserJsonBuilder()
            .WithEmail("natan@email.com")
            .WithPassword(password)
            .Build();

        // Act
        var response = await Client.PostAsJsonAsync(ControllerUri, request);

        // Assert
        var userRegistredJson = await DeserializeResponse<Responses.Result<MessageResult>>(response);

        userRegistredJson.Should().NotBeNull();
        userRegistredJson.Data.Should().BeNull();
        userRegistredJson.Errors.Should().NotBeNullOrEmpty();
        userRegistredJson.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task UserController_ReturnsError_WhenUserEmailIsBlank()
    {
        // Arrange
        var request = new RequestRegisterUserJsonBuilder()
            .WithName(string.Empty)
            .Build();

        // Act
        var response = await Client.PostAsJsonAsync(ControllerUri, request);

        // Assert
        var userRegistredJson = await DeserializeResponse<Responses.Result<MessageResult>>(response);

        userRegistredJson.Should().NotBeNull();
        userRegistredJson.Data.Should().BeNull();
        userRegistredJson.Errors.Should().NotBeNullOrEmpty();
        userRegistredJson.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task UserController_ReturnsError_WhenPasswordIsBlank()
    {
        // Arrange
        var request = new RequestRegisterUserJsonBuilder()
            .WithPassword(string.Empty)
            .Build();

        // Act
        var response = await Client.PostAsJsonAsync(ControllerUri, request);

        // Assert
        var userRegistredJson = await DeserializeResponse<Responses.Result<MessageResult>>(response);

        userRegistredJson.Should().NotBeNull();
        userRegistredJson.Data.Should().BeNull();
        userRegistredJson.Errors.Should().NotBeNullOrEmpty();
        userRegistredJson.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task UserController_ReturnsError_WhenPasswordIsInvalid()
    {
        // Arrange
        var request = new RequestRegisterUserJsonBuilder()
            .WithPassword("12345")
            .Build();

        // Act
        var response = await Client.PostAsJsonAsync(ControllerUri, request);

        // Assert
        var userRegistredJson = await DeserializeResponse<Responses.Result<MessageResult>>(response);

        userRegistredJson.Should().NotBeNull();
        userRegistredJson.Data.Should().BeNull();
        userRegistredJson.Errors.Should().NotBeNullOrEmpty();
        userRegistredJson.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task UserController_ReturnsError_WhenEmailIsInvalid()
    {
        // Arrange
        var request = new RequestRegisterUserJsonBuilder()
            .WithEmail("invaid-email")
            .Build();

        // Act
        var response = await Client.PostAsJsonAsync(ControllerUri, request);

        // Assert
        var userRegistredJson = await DeserializeResponse<Responses.Result<MessageResult>>(response);

        userRegistredJson.Should().NotBeNull();
        userRegistredJson.Data.Should().BeNull();
        userRegistredJson.Errors.Should().NotBeNullOrEmpty();
        userRegistredJson.IsSuccess.Should().BeFalse();
    }
}
