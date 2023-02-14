using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Core.Extensions;

namespace TopUI.Blazor.Core;

public sealed class ElementStyleAttribute : Attribute
{
    public ElementStyleAttribute(string styleName)
    {
        StyleName = styleName;
    }

    public string StyleName { get; }

    internal string? GetValue(UiComponent component, PropertyInfo property)
    {
        var value = property.GetValue(component);

        if (value?.GetType().IsEnum == true)
        {
            value = ((Enum)value).GetDisplayName();
        }

        return value?.ToString();
    }
}
