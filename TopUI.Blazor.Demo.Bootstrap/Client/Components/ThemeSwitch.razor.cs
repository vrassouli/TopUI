using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Bootstrap.Services.Abstractions;

namespace TopUI.Blazor.Demo.Bootstrap.Client.Components;

public sealed partial class ThemeSwitch
{
    private string _theme = "light";
    [Inject] private IBootstrapJs Bootstrap { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _theme = await Bootstrap.GetTheme();

        await base.OnInitializedAsync();
    }

    private string GetActionIcon()
    {
        if (_theme == "light")
            return "bi-moon";

        return "bi-sun";
    }
    private string GetActionTitle()
    {
        if (_theme == "light")
            return "Switch to dark mode";

        return "Switch to light mode";
    }

    private async Task Switch()
    {
        if (_theme == "light")
            _theme = "dark";
        else
            _theme = "light";

        await Bootstrap.SetTheme(_theme);
    }
}
