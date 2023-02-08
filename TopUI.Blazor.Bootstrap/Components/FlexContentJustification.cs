using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TopUI.Blazor.Bootstrap.Components;

public enum FlexContentJustification
{
    [Display(Name = "justify-content-start")]
    Start,
    [Display(Name = "justify-content-end")]
    End,
    [Display(Name = "justify-content-center")]
    Center,
    [Display(Name = "justify-content-between")]
    Between,
    [Display(Name = "justify-content-around")]
    Around,
    [Display(Name = "justify-content-evenly")]
    Evenly,
}
