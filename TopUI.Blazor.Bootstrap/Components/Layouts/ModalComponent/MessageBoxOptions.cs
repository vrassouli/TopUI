using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed class MessageBoxOptions
{
    public MessageBoxButton Buttons { get; set; } = MessageBoxButton.Ok;
    public MessageBoxIcon Icon { get; set; } = MessageBoxIcon.None;
}
