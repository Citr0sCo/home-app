using System.Net.Http.Headers;
using HomeBoxLanding.Api.Core.Events.Types;
using HomeBoxLanding.Api.Features.FuelPricePoller.Types;
using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.FuelPricePoller;

public class FuelPricePoller : ISubscriber
{
    private static FuelPricePoller _instance;
    private bool _isPolling = false;

    private readonly FuelPriceRepository _repository;

    public FuelPricePoller()
    {
        _repository = new FuelPriceRepository();
    }

    public static ISubscriber Instance()
    {
        if (_instance == null)
            _instance = new FuelPricePoller();

        return _instance;
    }
    
    private async Task<string> GetToken()
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(10);
        httpClient.DefaultRequestHeaders.Add("Accept","application/json");
        httpClient.DefaultRequestHeaders.Add("Accept-Language","en-GB");
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

        var request = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
        {
            new("grant_type", "client_credentials"),
            new("client_id", Environment.GetEnvironmentVariable("ASPNETCORE_FUEL_FINDER_CLIENT_ID")!),
            new("client_secret", Environment.GetEnvironmentVariable("ASPNETCORE_FUEL_FINDER_CLIENT_SECRET")!),
            new("scope", "fuelfinder.read")
        });
        
        var result = await httpClient.PostAsync("https://www.fuel-finder.service.gov.uk/api/v1/oauth/generate_access_token", request).ConfigureAwait(false);
        var response = JsonConvert.DeserializeObject<FuelDataTokenResponse>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));

        return response!.Data.AccessToken;
    }
    
    private async Task<List<FuelStationsResponse>> GetStations(string token)
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(10);
        httpClient.DefaultRequestHeaders.Add("Accept","application/json");
        httpClient.DefaultRequestHeaders.Add("Accept-Language","en-GB");
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
        var result = await httpClient.GetAsync("https://www.fuel-finder.service.gov.uk/api/v1/pfs?batch-number=1").ConfigureAwait(false);
        var response = JsonConvert.DeserializeObject<List<FuelStationsResponse>>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));
                
        return response ?? new List<FuelStationsResponse>();
    }
    
    private async Task<List<FuelDataResponse>> GetPrices(string token)
    {
        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(10);
        httpClient.DefaultRequestHeaders.Add("Accept","application/json");
        httpClient.DefaultRequestHeaders.Add("Accept-Language","en-GB");
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
        var result = await httpClient.GetAsync("https://www.fuel-finder.service.gov.uk/api/v1/pfs/fuel-prices?batch-number=1").ConfigureAwait(false);
        var response = JsonConvert.DeserializeObject<List<FuelDataResponse>>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));

        return response ?? new List<FuelDataResponse>();
    }

    private async Task StartPolling()
    {
        while (_isPolling)
        {
            Console.WriteLine("Grabbing latest data from fuel providers...");

            try
            {
                var token = await GetToken();
                var stations = await GetStations(token);
                var prices = await GetPrices(token);
                await _repository.SaveFuelPricesFor(stations, prices).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to grab fuel data.");
                Console.WriteLine($"Exception: {e.Message}");
            }
            
            Console.WriteLine("Finished grabbing latest data from fuel providers, waiting for 24 hours...");
            Thread.Sleep(1000 * 60 * 60 * 24); // 24 hours
        }
    }

    public void OnStarted()
    {
        _isPolling = true;
        StartPolling().ConfigureAwait(false);
    }

    public void OnStopping()
    {
        _isPolling = false;
    }

    public void OnStopped()
    {
        _isPolling = false;
    }
}