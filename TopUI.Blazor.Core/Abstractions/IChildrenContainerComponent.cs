using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Core.Abstractions;

public interface IChildrenContainerComponent<TChild>
    where TChild : IStateChangeNotification
{
    List<TChild> Children { get; }

    void AddChild(TChild child);

    void RemoveChild(TChild child);
}
