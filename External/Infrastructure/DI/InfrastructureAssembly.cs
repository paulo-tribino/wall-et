using System.Reflection;

namespace Infrastructure.DI;

internal static class InfrastructureAssembly
{
    public static readonly Assembly Assembly = typeof(InfrastructureAssembly).Assembly;
}
