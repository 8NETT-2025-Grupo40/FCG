using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FCG.API.Models;

public class ApiStructuredLog(string endpoint)
{
    public string Endpoint { get; set; } = endpoint;

    [JsonIgnore] public DateTime RequestStart { get; } = DateTime.Now;

    [JsonIgnore]
    public FcgLogLevel FcgLogLevel { set => LogLevel = value.ToString(); }

    [JsonPropertyName("Response status code")]
    public string ResponseStatusCode { get; set; } = string.Empty;
    
    [JsonPropertyName("Log level")]
    public string LogLevel { get; set; } = string.Empty;
    
    [JsonPropertyName("Request start time")]
    public string RequestStartTime { get; set; } = string.Empty;
    
    [JsonPropertyName("Request end time")]
    public string RequestEndTime { get; set; } = string.Empty;
    
    [JsonPropertyName("Elapsed milliseconds to process request")]
    public string ElapsedTime { get; set; } = string.Empty;
    
    [JsonPropertyName("Information message")]
    public string InformationMessage { get; set; } = string.Empty;
    
    [JsonPropertyName("Error message")]
    public string ErrorMessage { get; set; } = string.Empty;
    
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };

    public override string ToString() => 
        JsonSerializer.Serialize(this, _jsonSerializerOptions);
}

public static class ApiStructuredLogExtensions
{
    public static void TransformIntoSuccessfulLog(this ApiStructuredLog apiStructuredLog, int statusCode)
    {
        const string success = "Success";
        apiStructuredLog.FcgLogLevel = FcgLogLevel.Information;
        apiStructuredLog.InformationMessage = success;
        apiStructuredLog.ResponseStatusCode = statusCode.ToString();
    }

    public static void TransformIntoErrorLog(this ApiStructuredLog apiStructuredLog, string errorMessage, int statusCode)
    {
        const string informationMessage =
            $"Please check {nameof(apiStructuredLog.ErrorMessage)} property for more details.";
        apiStructuredLog.FcgLogLevel = FcgLogLevel.Error;
        apiStructuredLog.InformationMessage = informationMessage;
        apiStructuredLog.ErrorMessage = errorMessage;
        apiStructuredLog.ResponseStatusCode = statusCode.ToString();
    }

    public static void FinishRequest(this ApiStructuredLog apiStructuredLog)
    {
        const string iso8601Format = "yyyy-MM-dd HH:mm:ss.fff";
        
        var datetimeNow = DateTime.Now;
        
        apiStructuredLog.RequestStartTime = apiStructuredLog.RequestStart.ToString(iso8601Format);
        apiStructuredLog.RequestEndTime = datetimeNow.ToString(iso8601Format);
        var time = datetimeNow - apiStructuredLog.RequestStart;
        apiStructuredLog.ElapsedTime = time.TotalMilliseconds.ToString(CultureInfo.CurrentCulture);
    }
}