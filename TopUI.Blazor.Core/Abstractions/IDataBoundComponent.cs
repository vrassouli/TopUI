using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Core.Abstractions;

public interface IDataBoundComponent<TItem>
{
    IList<TItem>? Items { get; set; }
    ItemsProviderDelegate<TItem>? ItemsProvider { get; set; }
}
