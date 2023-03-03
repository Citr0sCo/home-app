using HomeBoxLanding.Api.Core.Types;

namespace HomeBoxLanding.Api.Features.Deploys.Types
{
    public class SaveDeployResponse : CommunicationResponse
    {
        public Guid DeployIdentifier { get; set; }
        public string CommitId { get; set; }
        public DateTime StartedAt { get; set; }
    }
}