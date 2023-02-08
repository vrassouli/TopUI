using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Bootstrap.Services.Abstractions;

namespace TopUI.Blazor.Demo.Bootstrap.Client.Components;

public sealed partial class DirectionSwitch
{
    private string _dir = "light";
    [Inject] private IBootstrapJs Bootstrap { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _dir = await Bootstrap.GetDir();

        await base.OnInitializedAsync();
    }

    private string GetActionIcon()
    {
        if (_dir == "ltr")
            return "bi-moon";

        return "bi-sun";
    }
    private string GetActionTitle()
    {
        if (_dir == "ltr")
            return "Switch to RTL";

        return "Switch to LTR";
    }

    private async Task Switch()
    {
        if (_dir == "ltr")
            _dir = "rtl";
        else
            _dir = "ltr";

        await Bootstrap.SetDir(_dir);
    }
}
