using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Core.Services.Abstraction;

namespace TopUI.Blazor.Bootstrap.Components;
public partial class FileSelector
{
    private string? _inputId;

    private string InputId
    {
        get
        {
            if (string.IsNullOrEmpty(_inputId))
                _inputId = GenerateUniqueId();

            return _inputId;
        }
    }

    [Parameter] public bool Multiple { get; set; }
    [Parameter] public EventCallback<InputFileChangeEventArgs> OnChange { get; set; }

    [Inject] private ITopUiJs TopUi { get; set; } = default!;

    protected override void OnInitialized()
    {
        if (TopUi == null)
            throw new ArgumentNullException($"{nameof(FileSelector)} requires {nameof(ITopUiJs)} service.");

#pragma warning disable CS8974 // Converting method group to non-delegate type
        AdditionalAttributes["onclick"] = OnClick;
#pragma warning restore CS8974 // Converting method group to non-delegate type

        base.OnInitialized();
    }

    private async Task OnClick()
    {
        await TopUi.InvokeClickEventAsync($"#{InputId}");
    }
}
