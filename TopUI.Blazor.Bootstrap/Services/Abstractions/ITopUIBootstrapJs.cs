using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopUI.Blazor.Bootstrap.Interops;
using TopUI.Blazor.Core.Interops;

namespace TopUI.Blazor.Bootstrap.Services.Abstractions;

internal interface ITopUIBootstrapJs
{
    Task<DataGridInterop> GetDataGridAsync();
}
