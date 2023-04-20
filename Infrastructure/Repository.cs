using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using BI_Core;

namespace Infrastructure;


public class Repository<T>
    where T : ResponseBase, new()
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options;
    private string _uri;

    public Repository(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _options = new JsonSerializerOptions {PropertyNameCaseInsensitive = true};
    }

    public void Init(string url)
    {
        _uri = url;
    }

    public async Task<List<T>> GetAll()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<T>>($"{_uri}", _options);
            return response ?? Enumerable.Empty<T>().ToList();
        }
        catch (Exception e) when (e is HttpRequestException or JsonException)
        {
            Debug.WriteLine(e);
            return new List<T> {new() {IsResponseSuccess = false}};
        }
    }
    
    public async Task<List<T>> GetAll(string propertyName)
    {
        try
        {
            var response = await _httpClient
                .GetFromJsonAsync<List<T>>($"{_uri}/{propertyName}", _options);
            return response ?? Enumerable.Empty<T>().ToList();
        }
        catch (Exception e) when (e is HttpRequestException or JsonException)
        {
            Debug.WriteLine(e);
            return new List<T> {new() {IsResponseSuccess = false}};
        }
    }

    public async Task<T> Get(string id)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<T>($"{_uri}/{id}", _options);
            return response ?? new T();
        }
        catch (Exception e) when (e is HttpRequestException or JsonException)
        {
            Debug.WriteLine(e);
            return new T
            {
                IsResponseSuccess = false
            };
        }
    }
}
