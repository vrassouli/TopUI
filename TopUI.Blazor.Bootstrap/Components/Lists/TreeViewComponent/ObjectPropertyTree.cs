using Microsoft.AspNetCore.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public class ObjectPropertyTree : DataTreeView<object>
{
    Func<PropertyInfo, bool>? _compiledPropertyFilter;
    Func<PropertyInfo, string>? _compiledPropertyTitle;

    private Func<PropertyInfo, bool> CompiledPropertyFilter
    {
        get
        {
            if (_compiledPropertyFilter == null)
                _compiledPropertyFilter = PropertyFilter.Compile();

            return _compiledPropertyFilter;
        }
    }
    private Func<PropertyInfo, string> CompiledPropertyTitle
    {
        get
        {
            if (_compiledPropertyTitle == null)
                _compiledPropertyTitle = PropertyTitle.Compile();

            return _compiledPropertyTitle;
        }
    }

    [Parameter] public Expression<Func<PropertyInfo, bool>> PropertyFilter { get; set; } = x => true;
    [Parameter] public Expression<Func<PropertyInfo, string>> PropertyTitle { get; set; } = x => x.Name;
    [Parameter] public bool BrowseEnums { get; set; }

    protected override void OnInitialized()
    {
        ItemText = x => GetObjectTitle(x);
        ItemHasChildren = x => GetObjectHasChildren(x);
        ItemSubItems = x => GetObjectSubItems(x);

        base.OnInitialized();
    }

    private IEnumerable<object> GetObjectSubItems(object item)
    {
        if (item is Type type)
        {
            return GetTypeProperties(GetTargetType(type));
        }

        if (item is PropertyInfo propInfo)
        {
            var propertyType = GetTargetType(propInfo.PropertyType);

            if (propertyType.IsEnum && BrowseEnums)
                return GetEnumValues(propertyType);
            else
                return GetTypeProperties(propertyType);
        }

        return GetTypeProperties(GetTargetType(item.GetType()));
    }

    private bool GetObjectHasChildren(object item)
    {
        return GetObjectSubItems(item).Any();
    }

    private string GetObjectTitle(object item)
    {
        if (item is Type type)
            return type.Name;

        if (item is PropertyInfo propInfo)
            return CompiledPropertyTitle.Invoke(propInfo);

        return item.ToString() ?? "object";
    }

    private IEnumerable<object> GetEnumValues(Type enumType)
    {
        return Enum.GetValues(enumType).ToDynamicList();
    }

    private IEnumerable<PropertyInfo> GetTypeProperties(Type type)
    {
        int i = 0;

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        return properties.Where(x => CompiledPropertyFilter.Invoke(x));
    }

    private Type GetTargetType(Type type)
    {
        if(IsNullable(type))
            return Nullable.GetUnderlyingType(type) ?? type;

        if (IsEnumerable(type))
            return type.GenericTypeArguments[0];

        return type;
    }

    private static bool IsNullable(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }
    private static bool IsEnumerable(Type type)
    {
        if (type == typeof(string))
            return false;

        return typeof(IEnumerable).IsAssignableFrom(type);
    }
}
