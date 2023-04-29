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

app.MapGet("/rentdata", async (GetConnection connectionGetter) =>
    {
        using var con = await connectionGetter();
        var data = await con.QueryAsync<ImmoRentDataModelBase>(
            $"SELECT id, lon, lat, pricePerSqMBase, pricePerSqMService, pricePerSqMTotal, " +
            $"'condition', interiorQual, typeOfFlat, heatingType, yearConstructed " +
            $"FROM immoscout_rent");
        return data.ToList().Count > 0 ? Results.Ok(data.ToList()) : Results.NoContent();
    })
    .WithOpenApi();



app.MapGet("/regions/{name}", async (GetConnection connectionGetter, string name) =>
    {
        using var con = await connectionGetter();
        var data = await con.QueryAsync<GenericAggregatesModel>(
            $"SELECT agskey, {name} AS genericProperty FROM landkreise");
        return data.ToList().Count > 0 ? Results.Ok(data.ToList()) : Results.NoContent();
    })
    .WithOpenApi();


app.MapGet("/aggregates/{dataset}/{columnName}", async (GetConnection connectionGetter, 
        string dataset, string columnName) =>
    {
        if (dataset != "buy" && dataset != "rent" && dataset != "economic")
        {
            return Results.BadRequest();
        }

        dataset = dataset switch
        {
            "rent" => "immoscout_rent",
            "buy" => "immoscout_buy",
            _ => "landkreise"
        }; 
        
        using var con = await connectionGetter();
        var data = await con.QueryAsync<GenericAggregatesModel>(
            $"select * from (select agskey, avg({columnName}) as genericProperty " +
            $"from {dataset} group by agskey) a " +
            $"where a.agskey is not null;");
        return data.ToList().Count > 0 ? Results.Ok(data.ToList()) : Results.NoContent();
    })
    .WithOpenApi();


app.MapGet("/raw/{dataset}/{name}", async (GetConnection connectionGetter, string dataset, string name) =>
    {
        if (dataset != "buy" && dataset != "rent")
        {
            return Results.BadRequest();
        }

        dataset = dataset == "rent" ? "immoscout_rent" : "immoscout_buy";
        using var con = await connectionGetter();
        var data = await con.QueryAsync<GenericRawDataModel>(
            $"SELECT id, lat, lon, {name} AS genericProperty FROM {dataset} WHERE {name} is not null");
        return data.ToList().Count > 0 ? Results.Ok(data.ToList()) : Results.NoContent();
    })
    .WithOpenApi();


app.MapGet("/rentdata/scores/geo", async (GetConnection connectionGetter) =>
    {
        using var con = await connectionGetter();
        var data = await con.QueryAsync<Location>(
            $"SELECT * FROM rent_aggregates");
        return data.ToList().Count > 0 ? Results.Ok(data.ToList()) : Results.NoContent();
    })
    .WithOpenApi();


app.MapGet("/rentdata/scores/info/{top}", async (GetConnection connectionGetter, bool top) =>
    {
        using var con = await connectionGetter();
        string append = top ? "desc" : "asc";
        var data = await con.QueryAsync<RentScoresModel>(
            $"SELECT * FROM rent_aggregates_info order by score {append} limit 5");
        return data.ToList().Count > 0 ? Results.Ok(data.ToList()) : Results.NoContent();
    })
    .WithOpenApi();


app.MapGet("/buydata/scores/info/{top}", async (GetConnection connectionGetter, bool top) =>
    {
        using var con = await connectionGetter();
        string append = top ? "desc" : "asc";
        var data = await con.QueryAsync<BuyScoresModel>(
            $"SELECT buy_aggregates_info.*, lat, lon FROM " +
            $"buy_aggregates_info " +
            $"join immoscout_buy ib on buy_aggregates_info.id = ib.id " +
            $"order by score {append} limit 5");
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
