using SharedKernel;

namespace Application.Errors;

public static class HealthErrors
{
    public static readonly Error DatabaseUnavailable = new("HealthCheck.DatabaseUnavailable", "Cannot connect to the database");

    public static Error UnexpectedError(string error) => new("HealthCheck.UnexpectedError", $"Health check failed: {error}");
}
