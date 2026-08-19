using System.Reflection;

namespace Application.DI;

internal static class ApplicationAssembly
{
    public static readonly Assembly Assembly = typeof(ApplicationAssembly).Assembly;
}
