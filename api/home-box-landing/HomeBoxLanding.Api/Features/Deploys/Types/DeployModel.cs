namespace HomeBoxLanding.Api.Features.Deploys.Types
{
    public class DeployModel
    {
        public string CommitId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
    }
}