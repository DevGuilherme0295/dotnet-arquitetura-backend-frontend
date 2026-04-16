using System;
using System.Globalization;
using System.Reflection;

namespace Company.ClassLibrary1;

public static class StringResource
{
    public static string GetStringByKey(string key, params object?[] args)
    {
        var message = ResourceAttributes.ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? string.Empty;

        return string.Format(message, args);
    }
}
