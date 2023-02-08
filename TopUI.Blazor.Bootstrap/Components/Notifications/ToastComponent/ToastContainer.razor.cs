using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Bootstrap.Components.Notifications.ToastComponent;
using TopUI.Blazor.Bootstrap.Services;
using TopUI.Blazor.Bootstrap.Services.Abstractions;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class ToastContainer
{
    private Dictionary<string, ToastConfig> _toasts = new();

    private BootstrapJs _bootstrapJs = default!;
    [Inject] private IBootstrapJs Bootstrap { get; set; } = default!;

    protected override void OnInitialized()
    {
        if (Bootstrap is not BootstrapJs bsJs)
            throw new ArgumentNullException($"{nameof(ToastContainer)} requires services which would be registered by calling services.{nameof(ServiceProviderExtensions.AddTopUIBootstrap)}()");
        _bootstrapJs = bsJs;

        _bootstrapJs.OnToastOpen += OnOpen;
        _bootstrapJs.OnToastHidden += OnHide;

        base.OnInitialized();
    }

    private void OnOpen(string message, ToastOptions options)
    {
        var toastModel = new ToastConfig(message, options);

        _toasts[toastModel.Id] = toastModel;

        StateHasChanged();
    }

    private void OnHide(string id)
    {
        if (_toasts.ContainsKey(id))
            _toasts.Remove(id);

        StateHasChanged();
    }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "toast-container";
    }
}
