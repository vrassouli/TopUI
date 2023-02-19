using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Demo.Bootstrap.Shared;

public class CityDto : TreeViewItemDto
{
    public string Name { get; set; }
    public int? Population { get; set; }

    public CityDto()
    {
        Name = string.Empty;
    }

    public CityDto(string name, int population)
    {
        Name = name;
        Population = population;
    }
}
