using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public enum BackgroundColor
{
    [Display(Name = "primary")]
    Primary,

    [Display(Name = "primary-subtle")]
    PrimarySubtle,

    [Display(Name = "secondary")]
    Secondary,

    [Display(Name = "secondary-subtle")]
    SecondarySubtle,

    [Display(Name = "success")]
    Success,

    [Display(Name = "success-subtle")]
    SuccessSubtle,

    [Display(Name = "danger")]
    Danger,

    [Display(Name = "danger-subtle")]
    DangerSubtle,

    [Display(Name = "warning")]
    Warning,

    [Display(Name = "warning-subtle")]
    WarningSubtle,

    [Display(Name = "info")]
    Info,

    [Display(Name = "info-subtle")]
    InfoSubtle,

    [Display(Name = "light")]
    Light,

    [Display(Name = "light-subtle")]
    LightSubtle,

    [Display(Name = "dark")]
    Dark,

    [Display(Name = "dark-subtle")]
    DarkSubtle,

    [Display(Name = "body-secondary")]
    BodySecondary,

    [Display(Name = "body-tertiary")]
    BodyTertiary,

    [Display(Name = "body")]
    Body,

    [Display(Name = "black")]
    Black,

    [Display(Name = "white")]
    White,

    [Display(Name = "transparent")]
    transparent,

}
