using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public enum ThemeType
{
    [Display(Name = "light")]
    Light,
    
    [Display(Name = "dark")]
    Dark
}
