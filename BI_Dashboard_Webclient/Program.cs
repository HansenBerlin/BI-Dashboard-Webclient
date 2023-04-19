using BI_Core;
using BI_Dashboard_Webclient;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<CsvParser<RegionsDataModel>>();
builder.Services.AddScoped<CsvParser<ImmoRentDataModel>>();
builder.Services.AddScoped<CsvParser<ImmoBuyDataModel>>();
builder.Services.AddScoped<DataStatefulRepository>();
builder.Services.AddSingleton<Repository<ImmoRentDataModelBase, ImmoRentDataModelBase>>();
builder.Services.AddSingleton<Repository<ImmoRentGenericDataModel, ImmoRentGenericDataModel>>();
builder.Services.AddSingleton<Repository<RentDashboardModel, RentDashboardModel>>();
builder.Services.AddSingleton<Repository<RegionsDataModel, RegionsDataModel>>();
builder.Services.AddSingleton<Repository<GenericRegionsModel, GenericRegionsModel>>();
builder.Services.AddMudServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();


app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();