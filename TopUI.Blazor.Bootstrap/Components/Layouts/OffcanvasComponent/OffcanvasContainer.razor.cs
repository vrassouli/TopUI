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

public sealed partial class OffcanvasContainer : IAsyncDisposable
{
    private Type? _componentType;
    private bool _isShown;
    private BootstrapJs _bootstrapJs = default!;
    private OffcanvasOptions? _options;

    [Inject] private IBootstrapJs Bootstrap { get; set; } = default!;

    protected override void OnInitialized()
    {
        if (Bootstrap is not BootstrapJs bsJs)
            throw new ArgumentNullException($"{nameof(OffcanvasContainer)} requires services which would be registered by calling services.{nameof(ServiceProviderExtensions.AddTopUIBootstrap)}()");
        _bootstrapJs = bsJs;

        _bootstrapJs.OnOffcanvasOpen += OnOpen;
        _bootstrapJs.OnOffcanvasHidden += OnHide;

        base.OnInitialized();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!_isShown && _componentType != null)
        {
            _isShown = true;
            await _bootstrapJs.ShowOffcanvas();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    public ValueTask DisposeAsync()
    {
        _bootstrapJs.OnOffcanvasOpen -= OnOpen;
        _bootstrapJs.OnOffcanvasHidden -= OnHide;

        return ValueTask.CompletedTask;
    }

    private void OnOpen(Type componentType, OffcanvasOptions options)
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

    private string GetOffcanvasClasses()
    {
        var list = new List<string>();

        list.Add($"offcanvas-{_options?.Placement}".ToLower());

        return string.Join(' ', list);
    }

    private Dictionary<string, object> GetOffcanvasAttributes()
    {
        var list = new Dictionary<string, object>();

        if (_options?.Scrollable == true)
            list.Add("data-bs-scroll", "true");

        if (_options?.Backdrop == OffcanvasBackdrop.NoBackdrop)
            list.Add("data-bs-backdrop", "false");
        else if (_options?.Backdrop == OffcanvasBackdrop.Static)
            list.Add("data-bs-backdrop", "static");

        list.Add("data-bs-keyboard", (_options?.Keyboard ?? true).ToString().ToLower());

        return list;
    }
}
