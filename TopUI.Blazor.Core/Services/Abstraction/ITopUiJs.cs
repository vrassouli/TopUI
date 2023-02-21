using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Core.Interops;

namespace TopUI.Blazor.Core.Services.Abstraction;

public interface ITopUiJs
{
    Task InvokeClickEventAsync(string selector);
    Task DownloadFileFromStreamAsync(Stream stream, string fileName);

    Task<DraggerInterop> GetDraggerAsync(IDraggerHandler handler);
    Task<DraggableInterop> GetDraggableAsync(IDraggableHandler handler);
    Task<DroppableInterop> GetDroppableAsync(IDroppableHandler handler);
    Task<ScrollSyncInterop> GetScrollSyncAsync();
}
