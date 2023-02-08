using System.Reflection;
using TopUI.Blazor.Core.Extensions;

namespace TopUI.Blazor.Core;

public sealed class ElementAttributeAttribute : Attribute
{
    public ElementAttributeAttribute(string attributeName)
    {
        AttributeName = attributeName;
    }

    public string AttributeName { get; }

    internal object? GetValue(UiComponent component, PropertyInfo property)
    {
        var value = property.GetValue(component);

        if (value?.GetType().IsEnum == true)
        {
            value = ((Enum)value).GetDisplayName();
        }

        return value;
    }
}
