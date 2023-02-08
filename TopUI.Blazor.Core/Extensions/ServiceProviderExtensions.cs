
using TopUI.Blazor.Core.Services;
using TopUI.Blazor.Core.Services.Abstraction;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceProviderExtensions
{
    public static void AddTopUI(this IServiceCollection services)
    {
        services.AddScoped<ITopUiJs, TopUiJs>();
    }
}
