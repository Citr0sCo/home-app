using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.Deploys.Types
{
    public class GithubBuildRequest
    {
        public GithubBuildRequest()
        {
            WorkflowRun = new GithubWorkflowRun();
        }
        
        [JsonProperty("workflow_run")]
        public GithubWorkflowRun WorkflowRun { get; set; }
    }

    public class GithubWorkflowRun
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        
        [JsonProperty("conclusion")]
        public string Conclusion { get; set; }
        
        [JsonProperty("head_sha")]
        public string HeadCommitIdentifier { get; set; }
    }
}