using TopUI.Blazor.Bootstrap.Components;

namespace TopUI.Blazor.Bootstrap.Services.Abstractions;

public interface IBootstrapJs
{
    Task SetTheme(string theme);
    Task<string> GetTheme();
    Task ToggleTheme();
    Task SetDir(string dir);
    Task<string> GetDir();
    Task ToggleDir();
    Task<dynamic?> OpenDialog<TComponent>(Action<DialogOptions> optionsBuilder);
    Task CloseDialog(dynamic? result);
    Task<dynamic?> ShowMessageBox(string title, string message, Action<MessageBoxOptions> optionsBuilder);
    Task<dynamic?> OpenOffcanvas<TComponent>(Action<OffcanvasOptions> optionsBuilder);
    Task CloseOffcanvas(dynamic? result);
    Task<dynamic?> AddToast(string message, Action<ToastOptions> optionsBuilder);
}
