using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Bootstrap.Interops;
using TopUI.Blazor.Bootstrap.Services.Abstractions;
using TopUI.Blazor.Core.Interops;

namespace TopUI.Blazor.Bootstrap.Services;

internal class TopUIBootstrapJs : ITopUIBootstrapJs, IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    public TopUIBootstrapJs(IJSRuntime jsRuntime)
    {
        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/TopUI.Blazor.Bootstrap/tui-bootstrap.blazor.interops.bundle.min.js").AsTask());
    }

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }

    public async Task<DataGridInterop> GetDataGridAsync()
    {
        var module = await _moduleTask.Value;
        var jsRef = await module.InvokeAsync<IJSObjectReference>("getDataGrid");

        return new DataGridInterop(jsRef);
    }
}
