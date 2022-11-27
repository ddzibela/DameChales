using System;
using System.Globalization;
using System.Net.Http;
using AutoMapper.Internal;
using DameChales.Common.Extensions;
using DameChales.Web.App;
using DameChales.Web.BL.Extensions;
using DameChales.Web.BL.Installers;
using DameChales.Web.BL.Options;
using DameChales.Web.DAL.Installers;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

const string defaultCultureString = "cs";

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("app");

var apiBaseUrl = builder.Configuration.GetValue<string>("ApiBaseUrl");

builder.Services.AddInstaller<WebDALInstaller>();
builder.Services.AddInstaller<WebBLInstaller>(apiBaseUrl);
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddAutoMapper(configuration =>
{
    // This is a temporary fix - should be able to remove this when version 11.0.2 comes out
    // More information here: https://github.com/AutoMapper/AutoMapper/issues/3988
    configuration.Internal().MethodMappingEnabled = false;
}, typeof(WebBLInstaller));
builder.Services.AddLocalization();

builder.Services.Configure<LocalDbOptions>(options =>
{
    options.IsLocalDbEnabled = bool.Parse(builder.Configuration.GetSection(nameof(LocalDbOptions))[nameof(LocalDbOptions.IsLocalDbEnabled)]);
});

var host = builder.Build();

var jsRuntime = host.Services.GetRequiredService<IJSRuntime>();
var cultureString = (await jsRuntime.InvokeAsync<string>("blazorCulture.get"))
                    ?? defaultCultureString;

var culture = new CultureInfo(cultureString);
await jsRuntime.InvokeVoidAsync("blazorCulture.set", cultureString);

CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();
