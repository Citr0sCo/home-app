namespace home_box_landing.api.Features.Deploy.Types
{
    public class GithubBuildRequest
    {
        public GithubBuildRequest()
        {
            Builds = new List<GithubBuild>();
        }
        
        public List<GithubBuild> Builds { get; set; }
        public GithubCommit Commit { get; set; }
    }

    public class GithubBuild
    {
        public string Stage { get; set; }
        public string Status { get; set; }
    }

    public class GithubCommit
    {
        public string Id { get; set; }
    }
}