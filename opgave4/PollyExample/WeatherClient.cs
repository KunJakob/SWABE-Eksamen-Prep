using Polly.CircuitBreaker;

namespace PollyExample;

using System.Net;
using Polly;
using Polly.Retry;

public class WeatherClient
{
    private readonly HttpClient _httpClient;

    private readonly AsyncRetryPolicy _retryPolicy;

    private readonly CircuitBreakerPolicy _circuitBreakerPolicy;
    
    public WeatherClient(IRetryDelayCalculator retryDelayCalculator)
    {
        _httpClient = new HttpClient();

        const int maxRetries = 3;
        const int maxFailures = 10;

        _retryPolicy = Policy.Handle<HttpRequestException>(ex => ex.StatusCode == HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(
                retryCount: maxRetries,
                sleepDurationProvider: retryDelayCalculator.Calculate,
                onRetry: (exception, sleepDuration, attemptNumber, context) =>
                {
                    Log($"Too many requests. Retrying in {sleepDuration}. {attemptNumber} / {maxRetries}");
                });
        

        _circuitBreakerPolicy = Policy.Handle<HttpRequestException>(ex => ex.StatusCode == HttpStatusCode.TooManyRequests)
            .CircuitBreaker(
                exceptionsAllowedBeforeBreaking: maxFailures,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (exception, duration) =>
                {
                    Log($"Circuit breaker opened. Will not retry for {duration}");
                },
                onReset: () =>
                {
                    Log("Circuit breaker reset");
                },
                onHalfOpen: () =>
                {
                    Log("Circuit breaker half-open");
                });
    }
    private static void Log(string message)
    {
        Console.WriteLine($"{DateTime.Now:hh:mm:ss.ffff} {message}");
    }
    
    public async Task<string> GetWeatherWithRetry()
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            var response = await _httpClient.GetAsync("https://localhost:12345/GetWeatherForecast");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        });
    }

    public async Task<string> GetWeatherWithCircuitBreaker()
    {
        return await _circuitBreakerPolicy.Execute(async () =>
        {
            var response = await _httpClient.GetAsync("https://localhost:12345/GetWeatherForecast");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        });
    }
}