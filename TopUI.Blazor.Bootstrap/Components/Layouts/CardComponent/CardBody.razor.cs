using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public sealed partial class CardBody
{
    [Parameter]
    [DefaultValue("Title")]
    public string? Title { get; set; }

    [Parameter]
    [DisplayName("Sub Title")]
    public string? SubTitle { get; set; }

    [Parameter]
    [Browsable(false)]
    public RenderFragment? ChildContent { get; set; }
}
