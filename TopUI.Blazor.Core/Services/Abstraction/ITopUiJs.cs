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
    Task<Dragger> GetDraggerAsync(IDraggerHandler handler);
    Task<Draggable> GetDraggableAsync(IDraggableHandler handler);
    Task<Droppable> GetDroppableAsync(IDroppableHandler handler);
    Task<ScrollSync> GetScrollSyncAsync();
}
