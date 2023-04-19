using System.ComponentModel.DataAnnotations;
using BI_Core;

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
        if (_type == typeof(RegionsDataModel))
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
        var props = _type.GetProperties().Where(prop => prop.CanWrite);

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