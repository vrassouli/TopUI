using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopUI.Blazor.Bootstrap.Components;

public enum TextColor
{
    [Display(Name = "primary")]
    Primary,

    [Display(Name = "primary-emphasis")]
    PrimaryEmphasis,

    [Display(Name = "secondary")]
    Secondary,

    [Display(Name = "secondary-emphasis")]
    SecondaryEmphasis,

    [Display(Name = "success")]
    Success,

    [Display(Name = "success-emphasis")]
    SuccessEmphasis,

    [Display(Name = "danger")]
    Danger,

    [Display(Name = "danger-emphasis")]
    DangerEmphasis,

    [Display(Name = "warning")]
    Warning,

    [Display(Name = "warning-emphasis")]
    WarningEmphasis,

    [Display(Name = "info")]
    Info,

    [Display(Name = "info-emphasis")]
    InfoEmphasis,

    [Display(Name = "light")]
    Light,

    [Display(Name = "light-emphasis")]
    LightEmphasis,

    [Display(Name = "dark")]
    Dark,

    [Display(Name = "dark-emphasis")]
    DarkEmphasis,

    [Display(Name = "body")]
    Body,

    [Display(Name = "muted")]
    Muted,

    [Display(Name = "body-emphasis")]
    BodyEmphasis,

    [Display(Name = "body-secondary")]
    BodySecondary,

    [Display(Name = "body-tertiary")]
    BodyTertiary,

    [Display(Name = "black")]
    Black,

    [Display(Name = "black-50")]
    Black50,

    [Display(Name = "white-50")]
    White50,
}
