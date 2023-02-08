using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Core.Abstractions;
using TopUI.Blazor.Core.Extensions;

namespace TopUI.Blazor.Core;

public abstract class UiComponent : UiComponentBase, IAsyncDisposable
{
    private string? _id;
    private string? _userClasses;
    private string? _userStyles;
    private Dictionary<string, object> _additionalAttributes = new();

#pragma warning disable BL0007 // Component parameters should be auto properties
    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the created element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    [Browsable(false)]
    public Dictionary<string, object> AdditionalAttributes
    {
        get
        {
            MergeStyles();
            MergeClasses();
            MergeId();

            return _additionalAttributes;
        }
        set
        {
            if (_additionalAttributes != value)
            {
                _additionalAttributes = value;

                ExtractStyleAttribute();
                ExtractClassAttribute();
                ExtractIdAttribute();
            }
        }
    }
#pragma warning restore BL0007 // Component parameters should be auto properties
    
    /// <summary>
    /// Gets a unique Id for the component
    /// </summary>
    public virtual string Id => GetComponentId();
    
    /// <summary>
    /// Gets a value indicating if the HTML disabled attribute is provided or not
    /// </summary>
    protected bool Disabled => IsDisabled();

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        UpdateAttributes();

        base.OnParametersSet();
    }

    private void ExtractIdAttribute()
    {
        if (_additionalAttributes.ContainsKey("id") && string.IsNullOrEmpty(_id))
        {
            _id = _additionalAttributes["id"]?.ToString();
        }
    }

    private void ExtractClassAttribute()
    {
        if (_additionalAttributes.ContainsKey("class"))
        {
            _userClasses = _additionalAttributes["class"]?.ToString();
        }
    }

    private void ExtractStyleAttribute()
    {
        if (_additionalAttributes.ContainsKey("style"))
            _userStyles = _additionalAttributes["style"]?.ToString();
    }

    private void MergeId()
    {
        _additionalAttributes["id"] = Id;
    }

    private void MergeClasses()
    {
        var classes = GetClasses();
        var classAttrib = $"{string.Join(' ', classes)} {_userClasses}".Trim();

        _additionalAttributes["class"] = classAttrib;
    }

    private void MergeStyles()
    {
        var styleAttribute = string.Empty;
        var propertyStyles = GetPropertyStyles();

        if (!string.IsNullOrEmpty(_userStyles) && !string.IsNullOrEmpty(propertyStyles))
            styleAttribute = $"{propertyStyles};{_userStyles}";
        else if (!string.IsNullOrEmpty(_userStyles) && string.IsNullOrEmpty(propertyStyles))
            styleAttribute = _userStyles;
        else if (string.IsNullOrEmpty(_userStyles) && !string.IsNullOrEmpty(propertyStyles))
            styleAttribute = propertyStyles;

        if (!string.IsNullOrEmpty(styleAttribute))
            _additionalAttributes["style"] = styleAttribute;
    }

    private string GetPropertyStyles()
    {
        var list = new List<string>();

        var type = GetType();
        var properties = type.GetProperties();
        foreach (var property in properties)
        {
            var attrib = property.GetCustomAttribute<ElementStyleAttribute>();

            if (attrib != null)
            {
                var styleName = attrib.StyleName;
                var valueStr = attrib.GetValue(this, property);

                if (!string.IsNullOrEmpty(valueStr))
                    list.Add($"{styleName}:{valueStr}");
            }
        }

        return string.Join(";", list);
    }

    private string GetComponentId()
    {
        if (string.IsNullOrEmpty(_id))
            _id = GenerateUniqueId();

        return _id;
    }

    private void UpdateAttributes()
    {
        var type = GetType();
        var properties = type.GetProperties();
        foreach (var property in properties)
        {
            var attrib = property.GetCustomAttribute<ElementAttributeAttribute>();

            if (attrib != null)
            {
                var attribName = attrib.AttributeName;
                var value = attrib.GetValue(this, property);
                if (value != null)
                    AddAttribute(attribName, value);
            }
        }
    }

    private bool IsDisabled()
    {
        if (!AdditionalAttributes.ContainsKey("disabled"))
            return false;

        var disabledValueStr = AdditionalAttributes["disabled"]?.ToString();
        //Console.WriteLine(disabledValueStr);

        if (bool.TryParse(disabledValueStr, out var disabledValue) && disabledValue)
            return true;

        return false;
    }

    /// <summary>
    /// Provides the list of the classes required by the component.
    /// </summary>
    /// <returns>IEnumerable<string> of the classes</returns>
    protected virtual IEnumerable<string> GetClasses()
    {
        // Add IElementClassProvider classes
        if (this is IElementClassProvider classProvider)
            foreach (var c in classProvider.GetClasses())
                yield return c;

        var boolProperties = GetType().GetProperties().Where(x => x.PropertyType == typeof(bool));

        // add ElementClassAttribute classes
        foreach (var prop in boolProperties)
        {
            var elClassAttr = prop.GetCustomAttribute<ElementClassAttribute>();
            if (elClassAttr != null)
            {
                var val = prop.GetValue(this) as bool? ?? false;

                if (val)
                    yield return elClassAttr.ClassName;
                else if (!val && !string.IsNullOrEmpty(elClassAttr.AlternateClassName))
                    yield return elClassAttr.AlternateClassName;
            }
        }

    }


    /// <summary>
    /// Adds an attribute to the component.
    /// </summary>
    /// <param name="attributeName">Attribute name</param>
    /// <param name="value">Attribute value</param>
    protected void AddAttribute(string attributeName, object value)
    {
        AdditionalAttributes.Set(attributeName, value);
    }

    protected void RemoveAttribute(string attributeName, object? value = null)
    {
        if (AdditionalAttributes.ContainsKey(attributeName) && (value == null || AdditionalAttributes[attributeName] == value))
            AdditionalAttributes.Remove(attributeName);
    }

    protected string GenerateUniqueId()
    {
        return $"_{Guid.NewGuid()}".Replace('-', '_');
    }

    public virtual ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}
