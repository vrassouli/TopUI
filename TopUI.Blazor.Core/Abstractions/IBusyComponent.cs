using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Core.Abstractions;

public interface IBusyComponent
{
    bool IsBusy { get; set; }
}
