using TopUI.Blazor.Bootstrap.Services;
using TopUI.Blazor.Bootstrap.Services.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceProviderExtensions
{
    public static void AddTopUIBootstrap(this IServiceCollection services)
    {
        services.AddLocalization();
        services.AddScoped<IBootstrapJs, BootstrapJs>();
    }
}
