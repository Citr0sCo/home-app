namespace HomeBoxLanding.Api.Features.Builds.Types
{
    public class Build
    {
        public Guid Identifier { get; set; }
        public BuildStatus Status { get; set; }
        public BuildConclusion Conclusion { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public string GithubBuildReference { get; set; }
    }
}