using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public enum FlexItemAlignment
{
    [Display(Name = "align-items-start")]
    Start,

    [Display(Name = "align-items-end")]
    End,

    [Display(Name = "align-items-center")]
    Center,

    [Display(Name = "align-items-baseline")]
    Baseline,

    [Display(Name = "align-items-stretch")]
    Stretch,
}
