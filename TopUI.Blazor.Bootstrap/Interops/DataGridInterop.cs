using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Core.Interops;

namespace TopUI.Blazor.Bootstrap.Interops;

internal class DataGridInterop : IAsyncDisposable
{
    private IJSObjectReference _jsRef;
    private DotNetObjectReference<DataGridInterop>? _objectReference;

    private DotNetObjectReference<DataGridInterop> ObjectReference
    {
        get
        {
            if (_objectReference == null)
                _objectReference = DotNetObjectReference.Create(this);

            return _objectReference;
        }
    }

    public DataGridInterop(IJSObjectReference jsRef)
    {
        _jsRef = jsRef;
    }

    public async Task InitializeAsync(string id)
    {
        await _jsRef.InvokeVoidAsync("initialize", ObjectReference, id);
    }

    public async ValueTask DisposeAsync()
    {
        await _jsRef.InvokeVoidAsync("dispose");
        await _jsRef.DisposeAsync();

        if (_objectReference != null)
            _objectReference.Dispose();
    }
}
