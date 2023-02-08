using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Extensions;

internal static class PropertyExtensions
{
    public static bool IsBrowseable(this PropertyInfo property)
    {
        var attribute = property.GetCustomAttribute<BrowsableAttribute>();

        return attribute == null || attribute.Browsable;
    }

    public static string GetDisplayName(this PropertyInfo property)
    {
        var attribute = property.GetCustomAttribute<DisplayNameAttribute>();

        return attribute?.DisplayName ?? property.Name;
    }

    public static object? GetDefaultValue(this PropertyInfo property)
    {
        var attribute = property.GetCustomAttribute<DefaultValueAttribute>();

        return attribute?.Value;
    }
}
