using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Core;

public class ElementClassAttribute : Attribute
{
    public string ClassName { get; set; }
    public string? AlternateClassName { get; set; }

    public ElementClassAttribute(string className)
    {
        ClassName = className;
    }
}
