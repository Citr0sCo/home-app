using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.FuelPricePoller.Types;

public class FuelStationsResponse
{
    public FuelStationsResponse()
    {
        Amenities = new List<string>();
        FuelTypes = new List<string>();
    }
    
    [JsonProperty("node_id")]
    public string NodeId;

    [JsonProperty("public_phone_number")]
    public string PublicPhoneNumber;

    [JsonProperty("trading_name")]
    public string TradingName;

    [JsonProperty("is_same_trading_and_brand_name")]
    public bool IsSameTradingAndBrandName;

    [JsonProperty("brand_name")]
    public string BrandName;

    [JsonProperty("temporary_closure")]
    public bool TemporaryClosure;

    [JsonProperty("permanent_closure")]
    public bool? PermanentClosure;

    [JsonProperty("permanent_closure_date")]
    public object PermanentClosureDate;

    [JsonProperty("is_motorway_service_station")]
    public bool IsMotorwayServiceStation;

    [JsonProperty("is_supermarket_service_station")]
    public bool IsSupermarketServiceStation;

    [JsonProperty("location")]
    public Location Location;

    [JsonProperty("amenities")]
    public List<string> Amenities;

    [JsonProperty("opening_times")]
    public OpeningTimes OpeningTimes;

    [JsonProperty("fuel_types")]
    public List<string> FuelTypes;
}

    public class BankHoliday
    {
        [JsonProperty("type")]
        public string Type;

        [JsonProperty("open_time")]
        public string OpenTime;

        [JsonProperty("close_time")]
        public string CloseTime;

        [JsonProperty("is_24_hours")]
        public bool Is24Hours;
    }

    public class Friday
    {
        [JsonProperty("open")]
        public string Open;

        [JsonProperty("close")]
        public string Close;

        [JsonProperty("is_24_hours")]
        public bool Is24Hours;
    }

    public class Location
    {
        [JsonProperty("address_line_1")]
        public string AddressLine1;

        [JsonProperty("address_line_2")]
        public string AddressLine2;

        [JsonProperty("city")]
        public string City;

        [JsonProperty("country")]
        public string Country;

        [JsonProperty("county")]
        public string County;

        [JsonProperty("postcode")]
        public string Postcode;

        [JsonProperty("latitude")]
        public double Latitude;

        [JsonProperty("longitude")]
        public double Longitude;
    }

    public class Day
    {
        [JsonProperty("open")]
        public string Open;

        [JsonProperty("close")]
        public string Close;

        [JsonProperty("is_24_hours")]
        public bool Is24Hours;
    }

    public class OpeningTimes
    {
        [JsonProperty("usual_days")]
        public UsualDays UsualDays;

        [JsonProperty("bank_holiday")]
        public BankHoliday BankHoliday;
    }

    public class UsualDays
    {
        [JsonProperty("monday")]
        public Day Monday;

        [JsonProperty("tuesday")]
        public Day Tuesday;

        [JsonProperty("wednesday")]
        public Day Wednesday;

        [JsonProperty("thursday")]
        public Day Thursday;

        [JsonProperty("friday")]
        public Day Friday;

        [JsonProperty("saturday")]
        public Day Saturday;

        [JsonProperty("sunday")]
        public Day Sunday;
    }

