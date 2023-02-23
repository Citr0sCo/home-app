namespace HomeBoxLanding.Api.Features.Builds.Types
{
    public class BuildsResponse
    {
        public BuildsResponse()
        {
            Builds = new List<Build>();
        }
        
        public List<Build> Builds { get; set; }
    }
}