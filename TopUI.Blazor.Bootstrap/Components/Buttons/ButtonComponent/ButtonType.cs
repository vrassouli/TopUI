using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public enum ButtonType
{
    [Display(Name = "button")]
    Button,
    
    [Display(Name = "submit")]
    Submit,
    
    [Display(Name = "reset")]
    Reset
}
