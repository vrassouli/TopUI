using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Core.Interops;

public class DraggableOptions
{
    public string DropEffect { get; set; } = "copy";

    public Dictionary<string, string> DataTransferItems { get; private set; } = new();
}
