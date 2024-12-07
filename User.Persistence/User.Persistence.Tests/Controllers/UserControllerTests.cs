using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using User.Persistence.Api.Controllers.v1;
using User.Persistence.Application.UseCase.ChangePassword;
using User.Persistence.Application.UseCase.Register;
using User.Persistence.Communication.Request;
using User.Persistence.Communication.Response;
using User.Persistence.Exceptions.ExceptionBase;

namespace User.Persistence.Tests.Controllers;
public class UserControllerTests
{
    [Fact]
    public async Task RegisterUser_ReturnsCreatedResult_WithValidRequest()
    {
        // Arrange
        var mockUseCase = new Mock<IRegisterUserUseCase>();

        var request = new RequestRegisterUserJson
        {
            Email = "new@email.com",
            Password = "password",
            Name = "name",
        };

        var responseExpected = new MessageResult("Cadastro em processamento.");

        var output = new Result<MessageResult>();

        var expectedResponse = output.Success(responseExpected);

        mockUseCase.Setup(useCase => useCase.RegisterUserAsync(It.IsAny<RequestRegisterUserJson>()))
                   .ReturnsAsync(expectedResponse);

        var controller = new UserController();

        // Act
        var result = await controller.RegisterUserAsync(mockUseCase.Object, request) as OkObjectResult;

        // Assert
        var response = result?.Value;

        Assert.NotNull(result.Value);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal(expectedResponse, response);
    }

    [Fact]
    public async Task RegisterUser_ThrowsValidationErrorsException_WhenRequestIsInvalid()
    {
        // Arrange
        var mockUseCase = new Mock<IRegisterUserUseCase>();

        var request = new RequestRegisterUserJson
        {
            Name = "",
            Email = "",
            Password = ""
        };

        var validationErrors = new ValidationErrorsException(new List<string>
        {
            "Nome do usuário em branco",
            "Email do usuário em branco",
            "Senha do usuário em branco"
        });

        mockUseCase.Setup(useCase => useCase.RegisterUserAsync(request))
                   .ThrowsAsync(validationErrors);

        var controller = new UserController();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationErrorsException>(() =>
            controller.RegisterUserAsync(mockUseCase.Object, request));

        Assert.Equal(validationErrors.ErrorMessages, exception.ErrorMessages);
    }

    [Fact]
    public async Task ChangePassword_ReturnsNoContentResult_WhenRequestIsValid()
    {
        // Arrange
        var mockUseCase = new Mock<IChangePasswordUseCase>();

        var request = new RequestChangePasswordJson
        {
            CurrentPassword = "current_password",
            NewPassword = "new_password"
        };

        var responseExpected = new MessageResult("Senha alterada com sucesso.");

        var output = new Result<MessageResult>();

        var expectedResponse = output.Success(responseExpected);

        mockUseCase.Setup(useCase => useCase.ChangePassword(request))
                   .ReturnsAsync(expectedResponse);

        var controller = new UserController();

        // Act
        var result = await controller.ChangePassword(mockUseCase.Object, request) as OkObjectResult;

        // Assert
        var response = result?.Value;

        Assert.NotNull(result.Value);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal(expectedResponse, response);
    }

    [Fact]
    public async Task ChangePassword_ThrowsInvalidCurrentPasswordException_WhenCurrentPasswordIsInvalid()
    {
        // Arrange
        var mockUseCase = new Mock<IChangePasswordUseCase>();
        var request = new RequestChangePasswordJson
        {
            CurrentPassword = "wrong_current_password",
            NewPassword = "new_password"
        };

        var invalidPasswordException = new ValidationErrorsException(new List<string>
        {
            "Senha atual incorreta",
        });

        var output = new Result<MessageResult>();

        var expectedResponse = output.Failure(new List<string>() { "Senha atual incorreta" });

        mockUseCase.Setup(useCase => useCase.ChangePassword(request))
                   .ReturnsAsync(expectedResponse);

        var controller = new UserController();

        // Act
        var result = await controller.ChangePassword(mockUseCase.Object, request) as OkObjectResult;

        // Assert
        var response = result?.Value;

        Assert.NotNull(result.Value);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal(expectedResponse, response);
    }
}
