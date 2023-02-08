using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Core.Generators;

public static class CodeGenerator
{
    public static string GenerateComponentInitializer<TComponent>(Dictionary<string, object?>? parameters)
    {
        return GenerateComponentInitializer(typeof(TComponent), parameters);
    }
    
    public static string GenerateComponentInitializer(Type componentType, Dictionary<string, object?>? parameters)
    {
        return GenerateComponentInitializer(componentType.Name, parameters);
    }

    public static string GenerateComponentInitializer(string component, Dictionary<string, object?>? parameters)
    {
        return RazorCodeGenerator.GenerateComponentInitializer(component, parameters);
    }
}
