using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace TopUI.Blazor.Demo.Bootstrap.Client.Pages.Components.Form;

public sealed class SampleModel
{
    [DisplayName("First Name")]
    [Display(Prompt = "First Name")]
    public string FirstName { get; set; } = default!;

    [DisplayName("Last Name")]
    [Display(Prompt = "First Name")]
    public string LastName { get; set; } = default!;

    public string EMail { get; set; } = default!;
    
    [DisplayName("Phone")]
    public string PhoneNumber { get; set; } = default!;
    
    [DisplayName("Address")]
    [Display(Prompt = "Your address")]
    public string Address { get; set; } = default!;

    public int Age { get; set; } = default!;

    public DateTime Birthdate { get; set; } = default!;

    public string Country { get; set; } = default!;

    public bool? Gender { get; set; }
    public bool Hired { get; set; }
    public int Revenue { get; set; }
    public Color HairColor { get; set; } = Color.Brown;
    
}
