using System.Text.Json.Serialization;

namespace BI_Core;


public class ResponseBase
{
    [JsonIgnore] 
    public bool IsResponseSuccess { get; init; } = true;
}