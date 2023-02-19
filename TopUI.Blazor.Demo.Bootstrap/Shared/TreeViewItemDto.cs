using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Demo.Bootstrap.Shared;

public class TreeViewItemDto
{
    public string Name { get; set; } = default!;
    public int? Population { get; set; }

    public IList<TreeViewItemDto>? SubItems { get; set; }
}
