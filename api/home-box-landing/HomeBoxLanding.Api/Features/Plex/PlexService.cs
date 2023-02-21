using System.Diagnostics;
using System.Text.Json.Serialization;
using HomeBoxLanding.Api.Core.Shell;
using HomeBoxLanding.Api.Features.Plex.Types;
using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.Plex
{
    public class PlexService
    {
        private readonly HttpClient _httpClient;

        public PlexService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public PlexActivityResponse GetActivity()
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(2);
            var result = _httpClient.GetAsync("http://192.168.1.161:8181/api/v2?apikey=cf459903c7454eb5a05544422cdcb12c&cmd=get_activity").Result;
            var response = result.Content.ReadAsStringAsync().Result;
            
            return JsonConvert.DeserializeObject<PlexActivityResponse>(response) ?? new PlexActivityResponse();
        }
    }
}