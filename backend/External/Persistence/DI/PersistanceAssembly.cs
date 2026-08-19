using System.Reflection;

namespace Persistence.DI;

internal static class PersistanceAssembly
{
    public static readonly Assembly Assembly = typeof(PersistanceAssembly).Assembly;
}
