using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Core.Interops;

public sealed class ScrollSyncOptions
{
    public bool SyncHorizontal { get; set; } = true;
    public bool SyncVertical { get; set; } = true;
}
