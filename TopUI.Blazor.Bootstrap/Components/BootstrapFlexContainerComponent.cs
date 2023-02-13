using Bootstrap.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Core;
using TopUI.Blazor.Core.Abstractions;

namespace TopUI.Blazor.Bootstrap.Components;

public abstract class BootstrapFlexContainerComponent : BootstrapComponent
{
    [Parameter]
    [Browsable(false)]
    public RenderFragment? ChildContent { get; set; }

    [Parameter] 
    [DisplayName("Item Alignment")]
    public FlexItemAlignment? ItemAlignment { get; set; }

    [Parameter] 
    [DisplayName("Content Justification")]
    public FlexContentJustification? ContentJustification { get; set; }

    [Parameter] 
    [DisplayName("Gap")]
    public int? Gap { get; set; }

    [Parameter, ElementClass("flex-wrap", AlternateClassName = "flex-nowrap")]
    [DisplayName("Flex Wrap")]
    public bool? FlexWrap { get; set; }

    [Parameter, ElementClass("overflow-auto")]
    [DisplayName("Auto Overflow")]
    public bool AutoOverflow { get; set; }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        if (ItemAlignment != null)
            yield return ItemAlignment.GetDisplayName();

        if (ContentJustification != null)
            yield return ContentJustification.GetDisplayName();

        if (Gap != null)
            yield return $"gap-{Gap}";
    }
}
