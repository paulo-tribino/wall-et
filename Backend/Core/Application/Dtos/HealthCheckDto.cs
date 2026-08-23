namespace Application.Dtos;

public sealed record HealthCheckDto(
    string Status,
    DateTime Timestamp,
    string? Message = null
);
