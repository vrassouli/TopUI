using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Bootstrap.Services;
using TopUI.Blazor.Bootstrap.Services.Abstractions;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class ModalContainer : IAsyncDisposable
{
    private Type? _componentType;
    private bool _isShown;
    private BootstrapJs _bootstrapJs = default!;
    private DialogOptions? _options;

    [Inject] private IBootstrapJs Bootstrap { get; set; } = default!;

    protected override void OnInitialized()
    {
        if (Bootstrap is not BootstrapJs bsJs)
            throw new ArgumentNullException($"{nameof(ModalContainer)} requires services which would be registered by calling services.{nameof(ServiceProviderExtensions.AddTopUIBootstrap)}()");
        _bootstrapJs = bsJs;

        _bootstrapJs.OnDialogOpen += OnOpen;
        _bootstrapJs.OnDialogHidden += OnHide;

        base.OnInitialized();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!_isShown && _componentType != null)
        {
            _isShown = true;
            await _bootstrapJs.ShowDialog();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    public ValueTask DisposeAsync()
    {
        _bootstrapJs.OnDialogOpen -= OnOpen;
        _bootstrapJs.OnDialogHidden -= OnHide;

        return ValueTask.CompletedTask;
    }

    private void OnOpen(Type componentType, DialogOptions options)
    {
        _componentType = componentType;
        _options = options;

        StateHasChanged();
    }

    private void OnHide()
    {
        _componentType = null;
        _options = null;

        _isShown = false;

        StateHasChanged();
    }

    private string GetModalClasses()
    {
        var list = new List<string>();

        if (_options?.Centered == true)
            list.Add("modal-dialog-centered");

        if (_options?.Size != DialogSize.Default)
            list.Add($"modal-{_options?.Size}".ToLower());

        return string.Join(' ', list);
    }

    private string GetDialogClasses()
    {
        var list = new List<string>();

        if (_options?.Scrollable == true)
            list.Add("modal-dialog-scrollable");

        if (_options?.FullScreen != DialogFullScreenBreakpoint.Never)
        {
            if (_options?.FullScreen == DialogFullScreenBreakpoint.Always)
                list.Add("modal-fullscreen");
            else
                list.Add($"modal-fullscreen-{_options?.FullScreen}-down".ToLower());
        }

        return string.Join(' ', list);
    }
    
    private Dictionary<string, object> GetModalAttributes()
    {
        var list = new Dictionary<string, object>();

        if (_options?.StaticBackdrop == true)
            list.Add("data-bs-backdrop", "static");

        list.Add("data-bs-keyboard", (_options?.Keyboard ?? true).ToString().ToLower());

        return list;
    }
}
