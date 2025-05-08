using System.Globalization;
using FCG.API.Models;
using FCG.API.Models.Enums;

namespace FCG.API.Extensions;

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