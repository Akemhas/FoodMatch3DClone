using System;
using Random = UnityEngine.Random;
public static class EnumUtils<T> where T : Enum
{
    private static Array _enumArray;
    private static Array EnumArray => _enumArray ??= GetEnumValues();

    public static T GetEnumValue(int index)
    {
        if (index >= EnumArray.Length || index < 0) throw new IndexOutOfRangeException($"Enum Index Out Of Range for Enum {typeof(T)} Requested Index {index}");
        return (T)EnumArray.GetValue(index);
    }

    /// <summary>
    /// Returns an Enum value of type T from minInclusive value to maxExclusive
    /// </summary>
    public static T GetRandomEnumValue(int minInclusive, int maxExclusive)
    {
        return (T)EnumArray.GetValue(Random.Range(minInclusive, maxExclusive));
    }
    /// <summary>
    /// Returns an Enum value of type T from minInclusive value to ArrayLength
    /// </summary>
    public static T GetRandomEnumValue(int minInclusive)
    {
        return (T)EnumArray.GetValue(Random.Range(minInclusive, EnumArray.Length));
    }
    public static Array GetEnumValues()
    {
        return Enum.GetValues(typeof(T));
    }
}
