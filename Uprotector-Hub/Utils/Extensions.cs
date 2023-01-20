using System;

namespace Uprotector_Hub.Utils;

public static class Extensions
{
    public static T Required<T>(this T? obj)
    {
        ArgumentNullException.ThrowIfNull(obj);
        return obj;
    }
}