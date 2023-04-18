using System.ComponentModel.DataAnnotations;

namespace BI_Dashboard_Webclient;

public class CsvParser<T>
    where T : class, new()
{
    private readonly string _file;
    private readonly Type _type;
    
    public CsvParser(IWebHostEnvironment environment)
    {
        _type = typeof(T);
        _file = GetPath(environment);
    }

    public async Task<List<T>> ParseAsync()
    {
        var csvData = new List<T>();
        using var streamReader = new StreamReader(_file);
        var headerLine = await streamReader.ReadLineAsync();
        var columnNames = headerLine.Split(',');

        while (await streamReader.ReadLineAsync() is { } line)
        {
            var values = line.Split(',');
            var data = Map(values, columnNames);
            csvData.Add(data);
        }

        return csvData;
    }

    private string GetPath(IWebHostEnvironment environment)
    {
        string file = "";
        if (_type == typeof(RdbDataModel))
        {
            file = "merged_bau_einkommen_insolvenz.csv";
        }
        else if (_type == typeof(ImmoBuyDataModel))
        {
            file = "immoscout_buy.csv";
        }
        else if (_type == typeof(ImmoRentDataModel))
        {
            file = "immoscout_rent.csv";
        }
        return Path.Combine(environment.WebRootPath, "data", file);
    }

    private T Map(IReadOnlyList<string> values, IReadOnlyList<string> columnNames)
    {
        var model = new T();
        var props = typeof(RdbDataModel).GetProperties().Where(prop => prop.CanWrite);

        for (int i = 0; i < values.Count; i++)
        {
            var prop = props
                .FirstOrDefault(p => p.Name
                .ToLower() == columnNames[i]
                .ToLower()
                .Replace(" ", ""));
            if (prop == null) continue;
            var value = Convert.ChangeType(values[i], prop.PropertyType);
            prop.SetValue(model, value);
        }

        return model;
    }
}


public class RdbDataModel
{
    public string AgsKey { get; set; }
    public double BuildingPermits { get; set; }
    public double LandPrices { get; set; }
    public double HouseholdIncome2019 { get; set; }
    public double ConsumerInsolvencies { get; set; }
}

public class ImmoBuyDataModel
{
    
}

public class ImmoRentDataModel
{
    public int Id { get; set; }
    public double PriceTrend { get; set; }
    public double TotalRent { get; set; }
    public double ServiceCharge { get; set; }
    public double BaseRent { get; set; }
    public double LivingSpace { get; set; }
    public int NoRooms { get; set; }
    public int NoParkSpaces { get; set; }
    public bool Balcony { get; set; }
    public bool HasKitchen { get; set; }
    public int Cellar { get; set; }
    public string Condition { get; set; }
    public string InteriorQual { get; set; }
    public int Lift { get; set; }
    public string TypeOfFlat { get; set; }
    public double TelekomUploadSpeed { get; set; }
    public int Floor { get; set; }
    public int Garden { get; set; }
    public string HeatingType { get; set; }
    public string FiringTypes { get; set; }
    public int YearConstructed { get; set; }
    public double Lon { get; set; }
    public double Lat { get; set; }
    public double PricePerSqMBase { get; set; }
    public double PricePerSqMService { get; set; }
    public double PricePerSqMTotal { get; set; }
}
