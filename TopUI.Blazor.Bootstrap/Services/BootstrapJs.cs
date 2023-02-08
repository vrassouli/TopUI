using Microsoft.JSInterop;
using TopUI.Blazor.Bootstrap.Components;
using TopUI.Blazor.Bootstrap.Components.Layouts.ModalComponent;
using TopUI.Blazor.Bootstrap.Services.Abstractions;

namespace TopUI.Blazor.Bootstrap.Services;

internal sealed class BootstrapJs : IBootstrapJs, IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
    private IJSObjectReference? _topUiBootstrapReference;
    private DotNetObjectReference<BootstrapJs>? _objectReference;

    private DotNetObjectReference<BootstrapJs> ObjectReference
    {
        get
        {
            if (_objectReference == null)
                _objectReference = DotNetObjectReference.Create(this);

            return _objectReference;
        }
    }

    public BootstrapJs(IJSRuntime jsRuntime)
    {
        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/TopUI.Blazor.Bootstrap/bootstrap.blazor.interops.bundle.js").AsTask());
    }
    private async Task<IJSObjectReference> GetNewTopUiBootstrapAsync()
    {
        var module = await _moduleTask.Value;
        return await module.InvokeAsync<IJSObjectReference>("getTopUiBootstrap");
    }

    private async Task<IJSObjectReference> GetJsClassAsync()
    {
        if (_topUiBootstrapReference == null)
            _topUiBootstrapReference = await GetNewTopUiBootstrapAsync();

        return _topUiBootstrapReference;
    }

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module =  await _moduleTask.Value;
            await module.DisposeAsync();
        }

        if (_objectReference != null)
            _objectReference.Dispose();
    }

    #region Theme

    public async Task SetTheme(string theme)
    {
        var topUiBootstrap = await GetJsClassAsync();
        await topUiBootstrap.InvokeVoidAsync("setTheme", theme);
    }

    public async Task<string> GetTheme()
    {
        var topUiBootstrap = await GetJsClassAsync();
        return await topUiBootstrap.InvokeAsync<string>("getTheme");
    }

    public async Task ToggleTheme()
    {
        var topUiBootstrap = await GetJsClassAsync();
        await topUiBootstrap.InvokeVoidAsync("toggleTheme");
    }

    #endregion

    #region Direction

    public async Task SetDir(string dir)
    {
        var topUiBootstrap = await GetJsClassAsync();
        await topUiBootstrap.InvokeVoidAsync("setDir", dir);
    }

    public async Task<string> GetDir()
    {
        var topUiBootstrap = await GetJsClassAsync();
        return await topUiBootstrap.InvokeAsync<string>("getDir");
    }

    public async Task ToggleDir()
    {
        var topUiBootstrap = await GetJsClassAsync();
        await topUiBootstrap.InvokeVoidAsync("toggleDir");
    }

    #endregion

    #region Dialog

    TaskCompletionSource<dynamic?>? _dialogResultTCS;
    /// <summary>
    /// Occures when a new dialog is requested to be open.
    /// </summary>
    internal event Action<Type, DialogOptions>? OnDialogOpen;

    /// <summary>
    /// Occures when a dialog gets hidden.
    /// </summary>
    internal event Action? OnDialogHidden;

    /// <summary>
    /// Opens a new dialog
    /// </summary>
    /// <typeparam name="TComponent">Content component type</typeparam>
    /// <param name="builder">Options builder</param>
    /// <returns>Dialog result</returns>
    public Task<dynamic?> OpenDialog<TComponent>(Action<DialogOptions> builder)
    {
        var options = new DialogOptions("Dialog");
        builder(options);

        _dialogResultTCS = new TaskCompletionSource<dynamic?>();
        OnDialogOpen?.Invoke(typeof(TComponent), options);

        return _dialogResultTCS.Task;
    }

    public async Task CloseDialog(dynamic? result)
    {
        if (_dialogResultTCS != null && _dialogResultTCS.Task != null && !_dialogResultTCS.Task.IsCompleted)
        {
            _dialogResultTCS.SetResult(result);
        }

        var topUiBootstrap = await GetJsClassAsync();
        await topUiBootstrap.InvokeVoidAsync("closeModal");
    }

    public Task<dynamic?> ShowMessageBox(string title, string message, Action<MessageBoxOptions> builder)
    {
        var options = new MessageBoxOptions();
        builder(options);

        var parameters = new Dictionary<string, object>()
        {
            { nameof(MessageBoxComponent.Message), message },
            { nameof(MessageBoxComponent.Buttons), options.Buttons },
            { nameof(MessageBoxComponent.Icon), options.Icon },
        };

        _dialogResultTCS = new TaskCompletionSource<dynamic?>();
        OnDialogOpen?.Invoke(typeof(MessageBoxComponent), new DialogOptions(title)
        {
            Parameters = parameters,
        });

        return _dialogResultTCS.Task;
    }

    internal async Task ShowDialog()
    {
        var topUiBootstrap = await GetJsClassAsync();
        await topUiBootstrap.InvokeVoidAsync("showModal", ObjectReference);
    }

    [JSInvokable]
    public void OnDialogHide()
    {
        OnDialogHidden?.Invoke();

        if (_dialogResultTCS != null && _dialogResultTCS.Task != null && !_dialogResultTCS.Task.IsCompleted)
        {
            _dialogResultTCS.SetResult(null);
        }
    }


    #endregion

    #region Offcanvas

    TaskCompletionSource<dynamic?>? _offcanvasResultTCS;
    /// <summary>
    /// Occures when a new offcanvas is requested to be open.
    /// </summary>
    internal event Action<Type, OffcanvasOptions>? OnOffcanvasOpen;

    /// <summary>
    /// Occures when a offcanvas gets hidden.
    /// </summary>
    internal event Action? OnOffcanvasHidden;

    /// <summary>
    /// Opens a new offcanvas
    /// </summary>
    /// <typeparam name="TComponent">Content component type</typeparam>
    /// <param name="builder">Options builder</param>
    /// <returns>Dialog result</returns>
    public Task<dynamic?> OpenOffcanvas<TComponent>(Action<OffcanvasOptions> builder)
    {
        var options = new OffcanvasOptions("Dialog");
        builder(options);

        _offcanvasResultTCS = new TaskCompletionSource<dynamic?>();
        OnOffcanvasOpen?.Invoke(typeof(TComponent), options);

        return _offcanvasResultTCS.Task;
    }

    public async Task CloseOffcanvas(dynamic? result)
    {
        if (_offcanvasResultTCS != null && _offcanvasResultTCS.Task != null && !_offcanvasResultTCS.Task.IsCompleted)
        {
            _offcanvasResultTCS.SetResult(result);
        }

        var topUiBootstrap = await GetJsClassAsync();
        await topUiBootstrap.InvokeVoidAsync("closeOffcanvas");
    }

    internal async Task ShowOffcanvas()
    {
        var topUiBootstrap = await GetJsClassAsync();
        await topUiBootstrap.InvokeVoidAsync("showOffcanvas", ObjectReference);
    }

    [JSInvokable]
    public void OnOffcanvasHide()
    {
        OnOffcanvasHidden?.Invoke();

        if (_offcanvasResultTCS != null && _offcanvasResultTCS.Task != null && !_offcanvasResultTCS.Task.IsCompleted)
        {
            _offcanvasResultTCS.SetResult(null);
        }
    }


    #endregion

    #region Toasts

    TaskCompletionSource<dynamic?>? _toastResultTCS;
    /// <summary>
    /// Occures when a new toast is requested to be open.
    /// </summary>
    internal event Action<string, ToastOptions>? OnToastOpen;

    /// <summary>
    /// Occures when a toast gets hidden.
    /// </summary>
    internal event Action<string>? OnToastHidden;

    /// <summary>
    /// Opens a new toast
    /// </summary>
    /// <typeparam name="TComponent">Content component type</typeparam>
    /// <param name="builder">Options builder</param>
    /// <returns>Dialog result</returns>
    public Task<dynamic?> AddToast(string message, Action<ToastOptions> builder)
    {
        var options = new ToastOptions();
        builder(options);

        _toastResultTCS = new TaskCompletionSource<dynamic?>();
        OnToastOpen?.Invoke(message, options);

        return _toastResultTCS.Task;
    }

    public async Task CloseToast(dynamic? result)
    {
        if (_toastResultTCS != null && _toastResultTCS.Task != null && !_toastResultTCS.Task.IsCompleted)
        {
            _toastResultTCS.SetResult(result);
        }

        var topUiBootstrap = await GetJsClassAsync();
        await topUiBootstrap.InvokeVoidAsync("closeToast");
    }

    internal async Task ShowToast(string toastId)
    {
        var topUiBootstrap = await GetJsClassAsync();
        await topUiBootstrap.InvokeVoidAsync("showToast", ObjectReference, toastId);
    }

    [JSInvokable]
    public void OnToastHide(string id)
    {
        OnToastHidden?.Invoke(id);

        if (_toastResultTCS != null && _toastResultTCS.Task != null && !_toastResultTCS.Task.IsCompleted)
        {
            _toastResultTCS.SetResult(null);
        }
    }


    #endregion
}
