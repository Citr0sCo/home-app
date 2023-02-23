using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.Deploys.Types
{
    public class GithubBuildRequest
    {
        public GithubBuildRequest()
        {
            HeadCommit = new GithubCommit();
        }
        
        [JsonProperty("head_commit")]
        public GithubCommit HeadCommit { get; set; }
    }

    public class GithubCommit
    {
        [JsonProperty("id")]
        public string Identifier { get; set; }
    }
}