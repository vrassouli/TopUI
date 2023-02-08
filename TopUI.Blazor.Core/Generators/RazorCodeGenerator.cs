using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Core.Generators;

internal class RazorCodeGenerator
{
    internal static string GenerateComponentInitializer(string component, Dictionary<string, object?>? parameters)
    {
        var attributes = parameters == null ? string.Empty : GenerateComponentAttributes(parameters);
        return $"<{component} {attributes}>\n</{component}>";
    }

    private static string GenerateComponentAttributes(Dictionary<string, object?> parameters)
    {
        var list = new List<string>();
        foreach (var item in parameters.OrderBy(x => x.Key))
        {
            if (item.Value != null)
                list.Add($"{item.Key}=\"{item.Value}\"");
        }

        return string.Join("\n\t", list);
    }
}
