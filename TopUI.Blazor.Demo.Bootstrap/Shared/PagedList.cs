using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Demo.Bootstrap.Shared;

public class PagedList<T>
{
	public int Total { get; set; }
	public List<T>? Items { get; set; }

	public PagedList()
	{

	}

	public PagedList(List<T> items, int total)
	{
        Items = items;
        Total = total;
    }
}
