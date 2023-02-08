using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TopUI.Blazor.Bootstrap.Components;

public enum FlexSelfAlignment
{
    [Display(Name = "align-self-start")]
    Start,
    [Display(Name = "align-self-end")]
    End,
    [Display(Name = "align-self-center")]
    Center,
    [Display(Name = "align-self-baseline")]
    Baseline,
    [Display(Name = "align-self-stretch")]
    Stretch,

}
