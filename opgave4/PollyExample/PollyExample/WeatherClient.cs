namespace PollyExample;

using System.Net;
using Polly;
using Polly.Retry;

public class WeatherClient
{
    private readonly HttpClient _httpClient;

    private readonly AsyncRetryPolicy _retryPolicy;
    
    public WeatherClient(IRetryDelayCalculator retryDelayCalculator)
    {
        _httpClient = new HttpClient();

        const int maxRetries = 3;

        _retryPolicy = Policy.Handle<HttpRequestException>(ex => ex.StatusCode == HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(
                retryCount: maxRetries,
                sleepDurationProvider: retryDelayCalculator.Calculate,
                onRetry: (exception, sleepDuration, attemptNumber, context) =>
                {
                    Log($"Too many requests. Retrying in {sleepDuration}. {attemptNumber} / {maxRetries}");
                });
    }
    private void Log(string message)
    {
        Console.WriteLine($"{DateTime.Now:hh:mm:ss.ffff} {message}");
    }
    
    public async Task<string> GetWeather()
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            var response = await _httpClient.GetAsync("https://localhost:12345/GetWeatherForecast");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        });
    }
}