using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components
{
    public delegate ValueTask<IList<TItem>> TreeViewItemsProviderDelegate<TItem>(TItem? parent);
}
