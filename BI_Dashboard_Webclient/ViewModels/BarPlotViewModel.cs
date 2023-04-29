using BI_Core;
using Infrastructure;
using Microsoft.AspNetCore.Components;

namespace BI_Dashboard_Webclient.ViewModels;

public class BarPlotViewModel
{
    private readonly Repository<ImmoRentDataModelBase> _repository;
    private List<ImmoRentDataModelBase> _cachedData = new();
    
    public BarPlotViewModel(Repository<ImmoRentDataModelBase> repository)
    {
        _repository = repository;
        string url = "https://localhost:6001/rentdata";
        _repository.Init(url);
    }

    public async Task Init()
    {
        if (_cachedData.Count == 0)
        {
            var rentData = await _repository.GetAll();
            _cachedData = rentData.Count > 0 ? rentData : _cachedData;
        }
    }
    
    public (List<object> years, List<string> heatingTypes, List<List<object>> heating) ExtractData()
    {
        var years = _cachedData
            .Select(x => x.YearConstructed)
            .Where(x => x > 1880)
            .Distinct()
            .ToList();
        var heatingTypes = _cachedData
            .Select(x => x.HeatingType)
            .Distinct()
            .ToList();
            
        List<List<double>> relativeVals = heatingTypes.Select(_ => new List<double>()).ToList();
            
        foreach (int year in years)
        {
            int countTotal = _cachedData.Count(x => x.YearConstructed == year);
            for (var i = 0; i < heatingTypes.Count; i++)
            {
                string ht = heatingTypes[i];
                int count = _cachedData.Count(x => x.HeatingType == ht && x.YearConstructed == year);
                double relative = (double) count / countTotal;
                relativeVals[i].Add(relative);
            }
        }
        
        var y = years.Cast<object>().ToList();
        var rv= relativeVals.Select(l => l.Cast<object>().ToList()).ToList();
        return (y, heatingTypes, rv);
    }
}