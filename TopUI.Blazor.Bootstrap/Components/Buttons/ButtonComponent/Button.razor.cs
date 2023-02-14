using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Core;
using TopUI.Blazor.Core.Abstractions;

namespace TopUI.Blazor.Bootstrap.Components;

public partial class Button : IBusyComponent
{
    [DefaultValue("Button")]
    [Parameter] public string? Text { get; set; }
    [Parameter] public string? TextClass { get; set; }
    [Parameter] public string? Icon { get; set; }

    [Parameter] 
    [Browsable(false)]
    public RenderFragment? ChildContent { get; set; }

    [Parameter] public ButtonMode Mode { get; set; } = ButtonMode.Primary;
    [Parameter] public ButtonSizes Size { get; set; } = ButtonSizes.Default;
    [Parameter, ElementAttribute("type")] public ButtonType Type { get; set; } = ButtonType.Button;
    [Parameter] public bool Outline { get; set; }
    [Parameter] public bool CanToggle { get; set; }
    [Parameter] public bool Toggled { get; set; }
    [Parameter]
    [Browsable(false)]
    public EventCallback<bool> ToggledChanged { get; set; }
    private bool IsLink => AdditionalAttributes.ContainsKey("href");


    protected override void OnParametersSet()
    {
        if (CanToggle)
        {
            AddAttribute("data-bs-toggle", "button");
        }
        else if (!CanToggle)
        {
            RemoveAttribute("data-bs-toggle", "button");
        }

        if (IsBusy)
        {
            AddAttribute("disabled", "disabled");
        }
        else if (!IsBusy && !Disabled)
        {
            RemoveAttribute("disabled");
        }

        base.OnParametersSet();
    }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "tui-btn";
        yield return "btn";

        if (Outline)
            yield return $"btn-outline-{Mode}".ToLower();
        else
            yield return $"btn-{Mode}".ToLower();

        if (Size == ButtonSizes.Small)
            yield return $"btn-sm";
        else if (Size == ButtonSizes.Large)
            yield return $"btn-lg";

        if (Toggled == true)
            yield return "active";
    }

    private async Task OnClicked()
    {
        if (CanToggle)
        {
            Toggled = !Toggled;

            await ToggledChanged.InvokeAsync(Toggled);
        }
    }
}
