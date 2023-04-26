using System.Text.Json;
using BI_Core;

namespace BI_Dashboard_Webclient;

public class DataStatefulRepository
{
    private readonly IWebHostEnvironment _environment;
    private object? _geoJson;
    
    public DataStatefulRepository(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task Init()
    {
        var path = Path.Combine(_environment.WebRootPath, "geofiles", "landkreisegermany.geojson");
        string jsonString = await File.ReadAllTextAsync(path);
        _geoJson = JsonSerializer.Deserialize<object>(jsonString);
        if (_geoJson == null)
        {
            throw new IOException("Could not receive json data from file");
        }
    }
    
    public async Task<object> Get()
    {
        if (_geoJson == null)
        {
            await Init();
        }
        return _geoJson;
    }
}