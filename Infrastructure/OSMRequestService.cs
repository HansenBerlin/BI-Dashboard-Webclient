using BI_Core;
using Newtonsoft.Json.Linq;

namespace Infrastructure;

public class OSMRequestService
{
    private const string BaseUrl = "https://nominatim.openstreetmap.org/search?q=";
    private const string UrlEnd = "&format=json&polygon=1&addressdetails=1";
    private readonly HttpClient _client;

    public OSMRequestService(HttpClient client)
    {
        _client = client;
        _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        _client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
    }

    public async Task<Location> Get(AddressModel address)
    {
        string endpoint = $"{address.Street}+{address.StreetNumber}+{address.PostalCode}+{address.City}+germany";
        string url = $"{BaseUrl}{endpoint}{UrlEnd}";
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        using var response = await _client.SendAsync(request);
        try
        {
            if (!response.IsSuccessStatusCode) return new Location();
            string json = await response.Content.ReadAsStringAsync();
            var result = JArray.Parse(json)[0];
            var lat = result.Value<double>("lat");
            var lon = result.Value<double>("lon");
            return new Location { Lat = lat, Lon = lon };

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Location();
        }
    }
}