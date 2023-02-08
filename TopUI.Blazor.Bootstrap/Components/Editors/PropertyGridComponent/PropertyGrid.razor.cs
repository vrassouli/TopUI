using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Bootstrap.Extensions;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class PropertyGrid
{
    private object? _source;
    private List<PropertyInfo> _properties = new();
    private Dictionary<string, object?> _defaultValues = new();
    private Dictionary<string, object?> _updatedValues = new();

    [Parameter, EditorRequired] public object? ValueSource { get; set; }
    [Parameter] public EventCallback<Dictionary<string, object?>> PropertyValuesChanged { get; set; }
    
    protected override void OnParametersSet()
    {
        if (_source != ValueSource && ValueSource != null)
        {
            // Type changed...
            ReloadProperties();
            ReloadValues();

            NotifyValuesChanged();
        }

        _source = ValueSource;
        base.OnParametersSet();
    }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "tui-property-grid";
    }

    private void ReloadProperties()
    {
        if (ValueSource == null)
            return;

        var type = ValueSource.GetType();
        _properties = new();

        if (type != null)
        {
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.CanRead && p.CanWrite);

            foreach (var prop in properties)
            {
                if (prop.IsBrowseable())
                {
                    _properties.Add(prop);
                }
            }
        }
    }

    private void ReloadValues()
    {
        foreach (var property in _properties)
        {
            var value = GetPropertyValue(property);
            _defaultValues[property.Name] = value;
            _updatedValues[property.Name] = value;
        }
    }

    private object? GetPropertyValue(PropertyInfo property)
    {
        object? value = null;

        if (ValueSource is not null)
        {
            value = property.GetValue(ValueSource, null);
        }

        if (value is null)
        {
            value = property.GetDefaultValue();
        }

        return value;
    }

    private void ResetValue(PropertyInfo property)
    {
        if (_updatedValues.ContainsKey(property.Name) && _defaultValues.ContainsKey(property.Name))
        {
            _updatedValues[property.Name] = _defaultValues[property.Name];
        }
        else if (_updatedValues.ContainsKey(property.Name))
            _updatedValues[property.Name] = default;

        NotifyValuesChanged();
    }

    private bool HasChanged(PropertyInfo property)
    {
        if (_updatedValues.ContainsKey(property.Name) && _defaultValues.ContainsKey(property.Name))
            return !Equals(_defaultValues[property.Name], _updatedValues[property.Name]);

        return false;
    }

    private void OnValueChanged(PropertyInfo property, object? value)
    {
        _updatedValues[property.Name] = value;

        NotifyValuesChanged();
    }

    private void NotifyValuesChanged()
    {
        InvokeAsync(async () => { await PropertyValuesChanged.InvokeAsync(_updatedValues); });
    }

    private EventCallback<T> BuildOnChangedCallback<T>(PropertyInfo prop)
    {
        return EventCallback.Factory.Create<T>(this, v => { OnValueChanged(prop, v); });
    }

    private T? GetValue<T>(PropertyInfo property)
    {
        if (_updatedValues.ContainsKey(property.Name))
            return (T)_updatedValues[property.Name]!;

        return default;
    }

    private MethodInfo GetValueMethod()
    {
        var method = GetType().GetMethod(nameof(GetValue), BindingFlags.NonPublic | BindingFlags.Instance);

        if (method == null)
            throw new MissingMethodException(nameof(GetValue));

        return method;
    }

    private MethodInfo GetCallbackMethod()
    {
        var method = GetType().GetMethod(nameof(BuildOnChangedCallback), BindingFlags.NonPublic | BindingFlags.Instance);

        if (method == null)
            throw new MissingMethodException(nameof(BuildOnChangedCallback));

        return method;
    }
}
