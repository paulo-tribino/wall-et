using Application.Abstractions.Messaging;
using Application.Errors;
using FluentValidation;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTests.Messaging;

public sealed class ValidationSenderDecoratorTests
{
    private readonly ISender _sender;
    private readonly IServiceProvider _serviceProvider;
    private readonly ValidationSenderDecorator _decorator;

    public ValidationSenderDecoratorTests()
    {
        _sender = Substitute.For<ISender>();
        _serviceProvider = Substitute.For<IServiceProvider>();

        _decorator = new ValidationSenderDecorator(_sender, _serviceProvider);
    }

    [Fact]
    public async Task SendAsync_ReturnsCombinedValidationErrors_WhenCommandIsInvalid()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var command = new CreateWidgetCommand(string.Empty, string.Empty);
        var validator = new CreateWidgetCommandValidator();

        _serviceProvider
            .GetService(typeof(IValidator<CreateWidgetCommand>))
            .Returns(validator);

        // Act
        var result = await _decorator.SendAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ValidationErrors.BadRequest(string.Empty).Code, result.Error.Code);
        Assert.Equal("Name is required, Email is required", result.Error.Description);
        await _sender.DidNotReceive().SendAsync(command, cancellationToken);
    }

    [Fact]
    public async Task SendAsync_DelegatesToInnerSender_WhenQueryIsValid()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var query = new GetWidgetQuery("widget-1");
        var expected = Result.Success("widget");
        var validator = new GetWidgetQueryValidator();

        _sender.SendAsync(query, cancellationToken).Returns(expected);

        _serviceProvider
            .GetService(typeof(IValidator<GetWidgetQuery>))
            .Returns(validator);

        // Act
        var result = await _decorator.SendAsync(query, cancellationToken);

        // Assert
        Assert.Same(expected, result);
        await _sender.Received(1).SendAsync(query, cancellationToken);
    }

    [Fact]
    public async Task SendAsync_DelegatesToInnerSender_WhenCommandHasNoValidator()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var command = new DeleteWidgetCommand();
        var expected = Result.Success();
        _sender.SendAsync(command, cancellationToken).Returns(expected);

        // Act
        var result = await _decorator.SendAsync(command, cancellationToken);

        // Assert
        Assert.Same(expected, result);
        await _sender.Received(1).SendAsync(command, cancellationToken);
    }

    private sealed record CreateWidgetCommand(string Name, string Email) : ICommand<Guid>;

    private sealed class CreateWidgetCommandValidator : AbstractValidator<CreateWidgetCommand>
    {
        public CreateWidgetCommandValidator()
        {
            RuleFor(command => command.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(command => command.Email).NotEmpty().WithMessage("Email is required");
        }
    }

    private sealed record GetWidgetQuery(string Id) : IQuery<string>;

    private sealed class GetWidgetQueryValidator : AbstractValidator<GetWidgetQuery>
    {
        public GetWidgetQueryValidator()
        {
            RuleFor(query => query.Id).NotEmpty();
        }
    }

    private sealed record DeleteWidgetCommand : ICommand;
}
