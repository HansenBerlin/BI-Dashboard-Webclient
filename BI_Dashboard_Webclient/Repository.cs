using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using BI_Core;
using MudBlazor;

namespace BI_Dashboard_Webclient;


public class Repository<TResponse>
    where TResponse : ResponseBase, new()
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

    public async Task<List<TResponse>> GetAll()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<TResponse>>($"{_uri}", _options);
            return response ?? Enumerable.Empty<TResponse>().ToList();
        }
        catch (Exception e) when (e is HttpRequestException or JsonException)
        {
            Debug.WriteLine(e);
            return new List<TResponse> {new() {IsResponseSuccess = false}};
        }
    }
    
    public async Task<List<TResponse>> GetAll(string propertyName)
    {
        try
        {
            var response = await _httpClient
                .GetFromJsonAsync<List<TResponse>>($"{_uri}/{propertyName}", _options);
            return response ?? Enumerable.Empty<TResponse>().ToList();
        }
        catch (Exception e) when (e is HttpRequestException or JsonException)
        {
            Debug.WriteLine(e);
            return new List<TResponse> {new() {IsResponseSuccess = false}};
        }
    }

    public async Task<TResponse> Get(string id)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<TResponse>($"{_uri}/{id}", _options);
            return response ?? new TResponse();
        }
        catch (Exception e) when (e is HttpRequestException or JsonException)
        {
            Debug.WriteLine(e);
            return new TResponse
            {
                IsResponseSuccess = false
            };
        }
    }
}
