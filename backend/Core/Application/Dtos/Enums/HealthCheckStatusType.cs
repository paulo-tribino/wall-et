using System.ComponentModel;

namespace Application.Dtos.Enums
{
    public enum HealthCheckStatusType
    {
        [Description("Healthy")]
        Healthy,
        [Description("Degraded")]
        Degraded,
        [Description("Unhealthy")]
        Unhealthy
    }
}
