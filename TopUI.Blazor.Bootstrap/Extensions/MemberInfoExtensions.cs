using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Extensions;
public static class MemberInfoExtensions
{
    public static string GetDisplayName(this MemberInfo member)
    {
        var display = member.GetCustomAttribute<DisplayAttribute>();
        if (display != null && !string.IsNullOrEmpty(display.Name))
            return display.Name;

        var displayName = member.GetCustomAttribute<DisplayNameAttribute>();
        if (displayName != null && !string.IsNullOrEmpty(displayName.DisplayName))
            return displayName.DisplayName;

        return member.Name;
    }
    public static string? GetPrompt(this MemberInfo member)
    {
        var display = member.GetCustomAttribute<DisplayAttribute>();
        if (display != null && !string.IsNullOrEmpty(display.Prompt))
            return display.Prompt;

        return null;
    }
}
