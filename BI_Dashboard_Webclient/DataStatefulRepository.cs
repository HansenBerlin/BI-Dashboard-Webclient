using System.Text.Json;

namespace BI_Dashboard_Webclient;

public class DataStatefulRepository
{
    private readonly CsvParser<RdbDataModel> _parserRdb;
    private readonly CsvParser<ImmoRentDataModel> _parserRent;
    private readonly IWebHostEnvironment _environment;
    private List<RdbDataModel>? _rdbData;
    private List<ImmoRentDataModel>? _immoRentData;
    private object? _geoJson;
    
    public DataStatefulRepository(CsvParser<RdbDataModel> parserRdb, CsvParser<ImmoRentDataModel> parserRent, IWebHostEnvironment environment)
    {
        _parserRdb = parserRdb;
        _parserRent = parserRent;
        _environment = environment;
    }
    
    public async Task<object> GetGeoJson()
    {
        if (_geoJson == null)
        {
            var path = Path.Combine(_environment.WebRootPath, "geofiles", "landkreisegermany.geojson");
            string jsonString = await File.ReadAllTextAsync(path);
            _geoJson = JsonSerializer.Deserialize<object>(jsonString);
        }

        return _geoJson ?? new object();
    }

    public async Task<List<RdbDataModel>> GetRdbData()
    {
        return _rdbData ??= await _parserRdb.ParseAsync();
    }
    
    public async Task<List<ImmoRentDataModel>> GetRentData()
    {
        return _immoRentData ??= await _parserRent.ParseAsync();
    }

}