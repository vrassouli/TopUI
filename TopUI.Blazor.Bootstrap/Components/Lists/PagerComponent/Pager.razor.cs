using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class Pager
{
    [Inject] IStringLocalizer<Lists.PagerComponent.Resources.Pager> Localizer { get; set; } = default!;

    [Parameter]
    public int Page { get; set; } = 1;

    [Parameter]
    [Browsable(false)]
    public EventCallback<int> PageChanged { get; set; }

    [Parameter]
    [Browsable(false)]
    public EventCallback OnRefresh { get; set; }
   
    [Parameter] 
    [DefaultValue(10)]
    public int TotalPages { get; set; }
    
    [Parameter] public bool DisplayRefresh { get; set; } = true;
    [Parameter] public string RefreshIcon { get; set; } = "bi bi-arrow-clockwise";
    
    [Parameter] public bool DisplayNext { get; set; } = true;
    [Parameter] public string NextIcon { get; set; } = "bi bi-chevron-right";
    
    [Parameter] public bool DisplayLast { get; set; } = true;
    [Parameter] public string LastIcon { get; set; } = "bi bi-chevron-double-right";
    
    [Parameter] public bool DisplayPrevius { get; set; } = true;
    [Parameter] public string PreviusIcon { get; set; } = "bi bi-chevron-left";

    [Parameter] public bool DisplayFirst { get; set; } = true;
    [Parameter] public string FirstIcon { get; set; } = "bi bi-chevron-double-left";

    [Parameter] public bool DisplayPageButtons { get; set; } = true;
    [Parameter] public int MaxPageButtons { get; set; } = 5;

    [Parameter] public PagerSize Size { get; set; } = PagerSize.Default;

    private int From => Math.Max(1, (((Page-1) / MaxPageButtons) * MaxPageButtons) + 1);
    private int To => Math.Min(TotalPages + 1, From + MaxPageButtons);

    private async Task SetPage(int page)
    {
        Page = page;
        await PageChanged.InvokeAsync(page);
    }

    protected override IEnumerable<string> GetClasses()
    {
        foreach (var c in base.GetClasses())
            yield return c;

        yield return "pagination";

        if (Size != PagerSize.Default)
            yield return $"pagination-{Size}".ToLower();
    }
}
