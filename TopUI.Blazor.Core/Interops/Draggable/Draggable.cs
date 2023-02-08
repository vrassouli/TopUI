﻿using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Core.Interops;

public sealed class Draggable : IAsyncDisposable
{
    private IJSObjectReference _jsRef;
    private IDraggableHandler _handler;
    private DotNetObjectReference<Draggable>? _objectReference;

    private DotNetObjectReference<Draggable> ObjectReference
    {
        get
        {
            if (_objectReference == null)
                _objectReference = DotNetObjectReference.Create(this);

            return _objectReference;
        }
    }

    public Draggable(IJSObjectReference jsRef, IDraggableHandler handler)
    {
        _jsRef = jsRef;
        _handler = handler;
    }

    public async ValueTask DisposeAsync()
    {
        await _jsRef.InvokeVoidAsync("dispose");
        await _jsRef.DisposeAsync();

        if (_objectReference != null)
            _objectReference.Dispose();
    }

    public async Task InitializeAsync(string id, Action<DraggableOptions> optionsBuilder)
    {
        var options = new DraggableOptions();
        optionsBuilder(options);

        await _jsRef.InvokeVoidAsync("initialize", ObjectReference, id, options);
    }
}
