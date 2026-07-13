using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.FuelPricePoller.Types;

public class FuelDataResponse
{
    public FuelDataResponse()
    {
        FuelPrices = new List<FuelPrice>();
    }
    
    [JsonProperty("node_id")]
    public string NodeId;

    [JsonProperty("public_phone_number")]
    public string PublicPhoneNumber;

    [JsonProperty("trading_name")]
    public string TradingName;

    [JsonProperty("fuel_prices")]
    public List<FuelPrice> FuelPrices;    
}

public class FuelPrice
{
    [JsonProperty("fuel_type")]
    public string FuelType;

    [JsonProperty("price")]
    public double Price;

    [JsonProperty("price_last_updated")]
    public DateTime PriceLastUpdated;

    [JsonProperty("price_change_effective_timestamp")]
    public DateTime PriceChangeEffectiveTimestamp;
}