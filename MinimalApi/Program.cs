using System.Data;
using BI_Core;
using Dapper;
using MinimalApi;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<GetConnection>(sp => 
    async () =>
    {

        string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                                  ?? throw new InvalidOperationException();
        var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();
        return connection;
    });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/rentdata/{limit:int}", async (GetConnection connectionGetter, int limit) =>
    {
        using var con = await connectionGetter();
        var data = await con.QueryAsync<ImmoRentDataModelBase>(
            $"SELECT Id, lon, lat, pricePerSqMBase, pricePerSqMService, pricePerSqMTotal, " +
            $"'condition', interiorQual, typeOfFlat, heatingType, yearConstructed " +
            $"FROM immoscout_rent LIMIT {limit}");
        return data.ToList().Count > 0 ? Results.Ok(data.ToList()) : Results.NoContent();
    })
    .WithOpenApi()
    .WithDescription("description")
    .Produces(200, responseType:typeof(List<ImmoRentDataModelBase>))
    .Produces(204, responseType:null);


app.MapGet("/rentdata", async (GetConnection connectionGetter) =>
    {
        using var con = await connectionGetter();
        var data = await con.QueryAsync<ImmoRentDataModelBase>(
            $"SELECT Id, lon, lat, pricePerSqMBase, pricePerSqMService, pricePerSqMTotal, " +
            $"'condition', interiorQual, typeOfFlat, heatingType, yearConstructed " +
            $"FROM immoscout_rent ");
        return data.ToList().Count > 0 ? Results.Ok(data.ToList()) : Results.NoContent();
    })
    .WithOpenApi();


app.MapGet("/regions", async (GetConnection connectionGetter) =>
    {
        using var con = await connectionGetter();
        var data = await con.QueryAsync<RegionsDataModel>(
            "SELECT agskey, buildingPermits, landPrices, householdIncome2019, consumerInsolvencies " +
            "FROM landkreise;");
        return data.ToList().Count > 0 ? Results.Ok(data.ToList()) : Results.NoContent();
    })
    .WithOpenApi();

app.MapGet("/rentdata/{name}", async (GetConnection connectionGetter, string name) =>
    {
        using var con = await connectionGetter();
        var data = await con.QueryAsync<ImmoRentGenericDataModel>(
            $"SELECT Id, lat, lon, {name} AS genericProperty FROM immoscout_rent");
        return data.ToList().Count > 0 ? Results.Ok(data.ToList()) : Results.NoContent();
    })
    .WithOpenApi();

app.MapGet("/regions/{name}", async (GetConnection connectionGetter, string name) =>
    {
        using var con = await connectionGetter();
        var data = await con.QueryAsync<GenericRegionsModel>(
            $"SELECT agskey, {name} AS genericProperty FROM landkreise");
        return data.ToList().Count > 0 ? Results.Ok(data.ToList()) : Results.NoContent();
    })
    .WithOpenApi();


app.MapGet("/rentdata/dashboard", async (GetConnection connectionGetter) =>
    {
        using var con = await connectionGetter();
        var data = await con.QueryAsync<RentDashboardModel>(
            "SELECT " +
        "ROUND(AVG(baseRent)) AS averageBaseRent, " +
            "ROUND(AVG(livingSpace)) AS averageLivingSpace, " +
            "ROUND(SUM(baseRent)) AS totalBaseRentIncomePerMonth, " +
            "COUNT(*) AS totalApartments, " +
            "ROUND(AVG(pricepersqmbase), 2) AS averagePricePerSqm, " +
            "YEAR(CURRENT_DATE) - ROUND(AVG(yearConstructed)) AS averageHouseAge, " +
        "(SELECT heatingType FROM " +
            "(SELECT heatingType, COUNT(*) AS count " +
            "FROM immoscout_rent " +
            "GROUP BY heatingType " +
            "ORDER BY count DESC LIMIT 1) AS heatingTypeCount) " +
            "AS mostCommonHeatingType, " +
        "(SELECT interiorQual FROM " +
            "(SELECT interiorQual, " +
            "COUNT(*) AS count FROM immoscout_rent " +
            "GROUP BY interiorQual " +
            "ORDER BY count DESC LIMIT 1) " +
            "AS interiorQualCount) AS mostCommonInteriorQual, " +
        "(SELECT typeOfFlat FROM " +
            "(SELECT typeOfFlat, COUNT(*) " +
            "AS count FROM immoscout_rent " +
            "GROUP BY typeOfFlat " +
            "ORDER BY count DESC LIMIT 1) " +
            "AS typeOfFlatCount) " +
            "AS mostCommonTypeOfFlat, " +
        "(SELECT `condition` FROM " +
            "(SELECT `condition`, " +
            "COUNT(*) AS count FROM immoscout_rent " +
            "GROUP BY `condition` " +
            "ORDER BY count DESC LIMIT 1) " +
            "AS conditionCount) " +
            "AS mostCommonCondition " +
        "FROM immoscout_rent;");
        return data.ToList().Count > 0 ? Results.Ok(data.ToList()) : Results.NoContent();
    })
    .WithOpenApi();



app.Run();



namespace MinimalApi
{
    public delegate Task<IDbConnection> GetConnection();
}