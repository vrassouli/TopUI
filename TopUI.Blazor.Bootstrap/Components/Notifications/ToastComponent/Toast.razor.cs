using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bootstrap.Blazor.Extensions;
using TopUI.Blazor.Bootstrap.Services.Abstractions;
using TopUI.Blazor.Bootstrap.Services;

namespace TopUI.Blazor.Bootstrap.Components.Notifications.ToastComponent;

public partial class Toast
{
    private BootstrapJs _bootstrapJs = default!;
    [Inject] private IBootstrapJs Bootstrap { get; set; } = default!;
    [Parameter] public ToastConfig Config { get; set; } = default!;

    protected override void OnInitialized()
    {
        if (Bootstrap is not BootstrapJs bsJs)
            throw new ArgumentNullException($"{nameof(Toast)} requires services which would be registered by calling services.{nameof(ServiceProviderExtensions.AddTopUIBootstrap)}()");
        _bootstrapJs = bsJs;

        base.OnInitialized();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await _bootstrapJs.ShowToast(Config.Id);
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private string GetToastClasses()
    {
        var list = new List<string>();

        if (Config.Options.Background != null)
            list.Add($"bg-{Config.Options.Background.GetDisplayName()}".ToLower());

        if (Config.Options.Color != null)
            list.Add($"text-{Config.Options.Color.GetDisplayName()}".ToLower());

        return string.Join(" ", list);
    }

    private Dictionary<string, object> GetToastAttributes()
    {
        var list = new Dictionary<string, object>();

        list.Add("id", Config.Id);
        list.Add("data-bs-delay", Config.Options.Delay);
        list.Add("data-bs-autohide", Config.Options.AutoHide.ToString().ToLower());

        return list;
    }
}
