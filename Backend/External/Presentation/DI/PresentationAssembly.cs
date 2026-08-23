using System.Reflection;

namespace Infrastructure.DI;

internal static class PresentationAssembly
{
    public static readonly Assembly Assembly = typeof(PresentationAssembly).Assembly;
}
