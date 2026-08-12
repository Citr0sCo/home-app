using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.Tautulli.Types;

public class TautulliStatsResponse
{
    public Guid? Identifier { get; set; }

    [JsonProperty("total_movies")]
    public int TotalMovies { get; set; }

    [JsonProperty("total_shows")]
    public int TotalShows { get; set; }

    [JsonProperty("total_users")]
    public int TotalUsers { get; set; }
}
