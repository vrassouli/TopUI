using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public abstract class OrderByCommand { }
public sealed class OrderByCommand<TItem> : OrderByCommand
{
    public SortDirection Direction { get; set; }
    public Expression<Func<TItem, object?>>? Expression { get; set; }
}
