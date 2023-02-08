using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Demo.Bootstrap.Client.Pages.Components.Dropdown;

internal class DropdownItemDto
{
    public string Name { get; set; }

    public DropdownItemDto(string name)
    {
        Name = name;
    }
}
