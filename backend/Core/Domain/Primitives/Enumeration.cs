using System.Reflection;

namespace Domain.Primitives;

public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{
    private static readonly IDictionary<int, TEnum> Enumerations = CreateEnumerations();

    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; protected init; }

    public string Name { get; protected init; } = string.Empty;

    public static TEnum? FromValue(int value)
    {
        return Enumerations.TryGetValue(
            value,
            out TEnum? enumeration) ?
                enumeration :
                default;
    }

    public static TEnum? FromName(string name)
    {
        return Enumerations
             .Values
             .SingleOrDefault(e => e.Name == name);
    }

    public static IReadOnlyCollection<TEnum> GetValues()
    {
        return Enumerations
            .Values
            .ToArray();
    }

    public bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null)
        {
            return false;
        }

        return GetType() == other.GetType() &&
            Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return obj is Enumeration<TEnum> other &&
            Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return Name;
    }

    private static IDictionary<int, TEnum> CreateEnumerations()
    {
        var enumerationType = typeof(TEnum);

        var types = enumerationType
            .GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy)
            .Where(fieldInfo =>
                enumerationType.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo =>
                (TEnum)fieldInfo.GetValue(default)!);

        return types.ToDictionary(t => t.Id);
    }
}
