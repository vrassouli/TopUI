﻿using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Core.Interops;

public sealed class ScrollSyncInterop : IAsyncDisposable
{
    private IJSObjectReference _jsRef;
    private DotNetObjectReference<ScrollSyncInterop>? _objectReference;

    private DotNetObjectReference<ScrollSyncInterop> ObjectReference
    {
        get
        {
            if (_objectReference == null)
                _objectReference = DotNetObjectReference.Create(this);

            return _objectReference;
        }
    }

    public ScrollSyncInterop(IJSObjectReference jsRef)
    {
        _jsRef = jsRef;
    }

    public async Task InitializeAsync(string source, List<string> targets, Action<ScrollSyncOptions> optionsBuilder)
    {
        var options = new ScrollSyncOptions();
        optionsBuilder(options);

        await _jsRef.InvokeVoidAsync("initialize", ObjectReference, source, targets, options);
    }

    public async ValueTask DisposeAsync()
    {
        await _jsRef.InvokeVoidAsync("dispose");
        await _jsRef.DisposeAsync();

        if (_objectReference != null)
            _objectReference.Dispose();
    }
}
