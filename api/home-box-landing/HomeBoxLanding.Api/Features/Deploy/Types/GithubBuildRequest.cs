namespace HomeBoxLanding.Api.Features.Deploy.Types
{
    public class GithubBuildRequest
    {
        public GithubBuildRequest()
        {
            head_commit = new GithubCommit();
        }
        
        public GithubCommit head_commit { get; set; }
    }

    public class GithubCommit
    {
        public string id { get; set; }
    }
}