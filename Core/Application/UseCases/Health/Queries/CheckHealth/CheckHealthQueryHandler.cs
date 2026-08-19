using Application.Abstractions.Database;
using Application.Abstractions.Messaging;
using Application.Constants.Health;
using Application.Dtos;
using Application.Dtos.Enums;
using Application.Errors;
using Application.Extensions;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.UseCases.Health.Queries.CheckHealth;

internal sealed class CheckHealthQueryHandler : IQueryHandler<CheckHealthQuery, HealthCheckDto>
{
    private readonly IApplicationDbContext _dbContext;

    public CheckHealthQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<HealthCheckDto>> Handle(
        CheckHealthQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (_dbContext is not DbContext dbContext)
            {
                return Result.Failure<HealthCheckDto>(HealthErrors.DatabaseUnavailable);
            }

            var canConnect = await dbContext.Database.CanConnectAsync(cancellationToken);

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
