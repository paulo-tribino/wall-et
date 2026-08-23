using Application.Abstractions.Database;
using Application.Constants.Health;
using Application.Dtos.Enums;
using Application.Errors;
using Application.UseCases.Health.Queries.CheckHealth;
using NSubstitute;
using SharedKernel.Extensions;

namespace Application.UnitTests.Health.Queries;

public sealed class CheckHealthQueryTests
{
    private readonly IDatabaseHealthChecker _databaseHealthChecker;
    private readonly CheckHealthQueryHandler _handler;

    public CheckHealthQueryTests()
    {
        _databaseHealthChecker = Substitute.For<IDatabaseHealthChecker>();
        _handler = new CheckHealthQueryHandler(_databaseHealthChecker);
    }

    [Fact]
    public async Task HandleAsync_ReturnsError_WhenDatabaseCannotConnect()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        _databaseHealthChecker
            .CanConnectAsync(cancellationToken)
            .Returns(false);

        // Act
        var result = await _handler.HandleAsync(
            new CheckHealthQuery(),
            cancellationToken);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(HealthErrors.DatabaseUnavailable, result.Error);
        Assert.Null(result.Value);
    }

    [Fact]
    public async Task HandleAsync_ReturnsError_WhenDatabaseCheckThrows()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        const string exceptionMessage = "Database connection failed";

        _databaseHealthChecker
            .CanConnectAsync(cancellationToken)
            .Returns(Task.FromException<bool>(
                new InvalidOperationException(exceptionMessage)));

        // Act
        var result = await _handler.HandleAsync(
            new CheckHealthQuery(),
            cancellationToken);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(HealthErrors.UnexpectedError(exceptionMessage), result.Error);
        Assert.Null(result.Value);
    }

    [Fact]
    public async Task HandleAsync_ReturnsSuccess_WhenDatabaseCanConnect()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;
        var before = DateTime.UtcNow;

        _databaseHealthChecker
            .CanConnectAsync(cancellationToken)
            .Returns(true);

        // Act
        var result = await _handler.HandleAsync(
            new CheckHealthQuery(),
            cancellationToken);

        var after = DateTime.UtcNow;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(HealthCheckStatusType.Healthy.GetDescription(), result.Value.Status);
        Assert.Equal(HealthCheckMessages.AllSystemsOperational, result.Value.Message);
        Assert.InRange(result.Value.Timestamp, before, after);
    }
}
