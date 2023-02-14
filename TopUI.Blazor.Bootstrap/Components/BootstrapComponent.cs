using Bootstrap.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using TopUI.Blazor.Core;

namespace TopUI.Blazor.Bootstrap.Components;

public abstract class BootstrapComponent : UiComponent
{
    [Parameter, ElementAttribute("data-bs-theme")] 
    public ThemeType? Theme { get; set; }
    
    [Parameter] 
    [DisplayName("Self Alignment")]
    public FlexSelfAlignment? SelfAlignment { get; set; }
    
    [Parameter, ElementClass("flex-fill")]
    [DisplayName("Flex Fill")]
    public bool? FlexFill { get; set; }
    
    [Parameter, ElementClass("flex-grow-1", AlternateClassName = "flex-grow-0")] 
    [DisplayName("Flex Grow")]
    public bool? FlexGrow { get; set; }

    [Parameter, ElementClass("flex-shrink-1", AlternateClassName = "flex-shrink-0")] 
    [DisplayName("Flex Shrink")]
    public bool? FlexShrink { get; set; }

    [DisplayName("Is Busy")]
    [Parameter] public bool IsBusy { get; set; }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        if (SelfAlignment != null)
            yield return SelfAlignment.GetDisplayName();
    }
}
