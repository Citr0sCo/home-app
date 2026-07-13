using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.FuelPricePoller.Types;

internal class FuelDataTokenResponse
{
    [JsonProperty("success")]
    public bool Success;

    [JsonProperty("data")]
    public FuelDataTokenResponseData Data;

    [JsonProperty("message")]
    public string Message;
}

public class FuelDataTokenResponseData
{
    [JsonProperty("access_token")]
    public string AccessToken;

    [JsonProperty("token_type")]
    public string TokenType;

    [JsonProperty("expires_in")]
    public int ExpiresIn;

    [JsonProperty("refresh_token")]
    public string RefreshToken;

    [JsonProperty("refresh_token_expires_in")]
    public int RefreshTokenExpiresIn;
}

