namespace FCG.API.Extensions;

public static class IntExtensions
{
    public static bool IsSuccessStatusCode(this int statusCode) => statusCode is >= 200 and < 300;
}