using Application.Abstractions.Database;
using Application.Abstractions.Messaging;
using Application.Constants.Health;
using Application.Dtos;
using Application.Dtos.Enums;
using Application.Errors;
using SharedKernel;
using SharedKernel.Extensions;

namespace Application.UseCases.Health.Queries.CheckHealth;

internal sealed class CheckHealthQueryHandler : IQueryHandler<CheckHealthQuery, HealthCheckDto>
{
    private readonly IDatabaseHealthChecker _databaseHealthChecker;

    public CheckHealthQueryHandler(IDatabaseHealthChecker databaseHealthChecker)
    {
        _databaseHealthChecker = databaseHealthChecker;
    }

    public async Task<Result<HealthCheckDto>> HandleAsync(
        CheckHealthQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var canConnect = await _databaseHealthChecker
                .CanConnectAsync(cancellationToken);

            if (!canConnect)
            {
                return Result.Failure<HealthCheckDto>(HealthErrors.DatabaseUnavailable);
            }

            var healthDto = new HealthCheckDto(
                Status: HealthCheckStatusType.Healthy.GetDescription(),
                Timestamp: DateTime.UtcNow,
                Message: HealthCheckMessages.AllSystemsOperational
            );

            return Result.Success(healthDto);
        }
        catch (Exception ex)
        {
            return Result.Failure<HealthCheckDto>(HealthErrors.UnexpectedError(ex.Message));
        }
    }
}
