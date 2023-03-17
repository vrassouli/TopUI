using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Globalization;
using TopUI.Blazor.Demo.Bootstrap.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddTopUI();
builder.Services.AddTopUIBootstrap();

//var culture = new CultureInfo("fa-IR");
//culture.NumberFormat.NegativeSign = "-";
//culture.NumberFormat.NumberDecimalSeparator = ".";

//culture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";

//culture.DateTimeFormat.MonthNames = new[] { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", String.Empty };

//CultureInfo.CurrentCulture = culture;
//CultureInfo.CurrentUICulture = culture;
//CultureInfo.DefaultThreadCurrentCulture = culture;
//CultureInfo.DefaultThreadCurrentUICulture = culture;

await builder.Build().RunAsync();
