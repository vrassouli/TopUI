using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Core.Interops;

public interface IDraggerHandler
{
    Task Dragged(DragMovment delta);
    Task Dragging(DragMovment delta);
}
