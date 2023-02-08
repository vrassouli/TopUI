using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class Col
{
    [Parameter]
    [Browsable(false)]
    public RenderFragment? ChildContent { get; set; }

    [Parameter] public string? WidthXs { get; set; }
    [Parameter] public string? WidthSm { get; set; }
    [Parameter] public string? WidthMd { get; set; }
    [Parameter] public string? WidthLg { get; set; }
    [Parameter] public string? WidthXl { get; set; }
    [Parameter] public string? WidthXxl { get; set; }

    [Parameter] public int? OrderXs { get; set; }
    [Parameter] public int? OrderSm { get; set; }
    [Parameter] public int? OrderMd { get; set; }
    [Parameter] public int? OrderLg { get; set; }
    [Parameter] public int? OrderXl { get; set; }
    [Parameter] public int? OrderXxl { get; set; }

    [Parameter] public int? OffsetXs { get; set; }
    [Parameter] public int? OffsetSm { get; set; }
    [Parameter] public int? OffsetMd { get; set; }
    [Parameter] public int? OffsetLg { get; set; }
    [Parameter] public int? OffsetXl { get; set; }
    [Parameter] public int? OffsetXxl { get; set; }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        /* Col & Width definition */
        if (string.IsNullOrEmpty(WidthXs) &&
            string.IsNullOrEmpty(WidthSm) &&
            string.IsNullOrEmpty(WidthMd) &&
            string.IsNullOrEmpty(WidthLg) &&
            string.IsNullOrEmpty(WidthXl) &&
            string.IsNullOrEmpty(WidthXxl))
            yield return "col";

        if(!string.IsNullOrEmpty(WidthXs))
            yield return $"col-{WidthXs}";

        if(!string.IsNullOrEmpty(WidthSm))
            yield return $"col-sm-{WidthSm}";

        if(!string.IsNullOrEmpty(WidthMd))
            yield return $"col-md-{WidthMd}";

        if(!string.IsNullOrEmpty(WidthLg))
            yield return $"col-lg-{WidthLg}";

        if(!string.IsNullOrEmpty(WidthXl))
            yield return $"col-xl-{WidthXl}";

        if(!string.IsNullOrEmpty(WidthXxl))
            yield return $"col-xxl-{WidthXxl}";

        /* Col order definition*/
        if (OrderXs != null)
            yield return $"order-{OrderXs}";

        if (OrderSm != null)
            yield return $"order-sm-{OrderSm}";

        if (OrderMd != null)
            yield return $"order-md-{OrderMd}";

        if (OrderLg != null)
            yield return $"order-lg-{OrderLg}";

        if (OrderXl != null)
            yield return $"order-xl-{OrderXl}";

        if (OrderXxl != null)
            yield return $"order-xxl-{OrderXxl}";

        /* Col offset definition*/
        if (OffsetXs != null)
            yield return $"offset-{OffsetXs}";

        if (OffsetSm != null)
            yield return $"offset-sm-{OffsetSm}";

        if (OffsetMd != null)
            yield return $"offset-md-{OffsetMd}";

        if (OffsetLg != null)
            yield return $"offset-lg-{OffsetLg}";

        if (OffsetXl != null)
            yield return $"offset-xl-{OffsetXl}";

        if (OffsetXxl != null)
            yield return $"offset-xxl-{OffsetXxl}";

    }
}
