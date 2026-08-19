using Application.Abstractions.Messaging;
using Application.Dtos;

namespace Application.UseCases.Health.Queries.CheckHealth;

public sealed record CheckHealthQuery : IQuery<HealthCheckDto>;
