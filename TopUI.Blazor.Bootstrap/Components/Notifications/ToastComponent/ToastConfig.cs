using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components.Notifications.ToastComponent;

public sealed class ToastConfig
{
    public string Id { get; private set; }

    public string Message { get; set; }

    public ToastOptions Options { get; set; }

    public ToastConfig(string message, ToastOptions options)
    {
        Message = message;
        Options = options;

        Id = $"_{Guid.NewGuid()}".Replace('-', '_');
    }
}
