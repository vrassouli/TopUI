using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Core.Interops;

public sealed class DragMovment
{
    public double Dx { get; set; }
    public double Dy { get; set; }

    public DragMovment()
    {

    }

    public DragMovment(double dx, double dy)
    {
        Dx = dx;
        Dy = dy;
    }
}
