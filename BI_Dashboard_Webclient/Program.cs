using BI_Core;
using BI_Core.Services;
using BI_Dashboard_Webclient;
using BI_Dashboard_Webclient.Models;
using BI_Dashboard_Webclient.ViewModels;
using Infrastructure;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<CsvParser<RegionsDataModel>>();
builder.Services.AddSingleton<DataStatefulRepository>();
builder.Services.AddSingleton<Repository<ImmoRentDataModelBase>>();
builder.Services.AddSingleton<Repository<GenericRawDataModel>>();
builder.Services.AddSingleton<Repository<RentDashboardModel>>();
builder.Services.AddSingleton<Repository<RegionsDataModel>>();
builder.Services.AddSingleton<Repository<GenericAggregatesModel>>();
builder.Services.AddSingleton<Repository<Location>>();
builder.Services.AddSingleton<Repository<RentScoresModel>>();
builder.Services.AddSingleton<Repository<BuyScoresModel>>();
builder.Services.AddSingleton<PredictionService>();
builder.Services.AddSingleton<RealEstateBookmark>();
builder.Services.AddSingleton<OSMRequestService>();
builder.Services.AddScoped<DropdownModel>();
builder.Services.AddTransient<DropdownViewModel>();
builder.Services.AddScoped<MapViewModel>();
builder.Services.AddMudServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();