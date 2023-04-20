using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using BI_Core;

namespace Infrastructure;

public class PredictionService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options;
    private string _uri;

    public PredictionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _options = new JsonSerializerOptions {PropertyNameCaseInsensitive = true};
    }

    public void Init(string url)
    {
        _uri = url;
    }
    
    public async Task<PredictionResponseModel> Post(PredictionRequestModel requestModel)
    {
        try
        {
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _httpClient.PostAsJsonAsync(_uri, requestModel, _options);
            var responseString = await response.Content.ReadFromJsonAsync<PredictionResponseModel>();
            return responseString;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return new PredictionResponseModel()
            {
            };
        }
    }
}