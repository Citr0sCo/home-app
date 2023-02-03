using HomeBoxLanding.Api.Core.Types;

namespace HomeBoxLanding.Api.Features.Deploy.Types
{
    public class SaveDeployResponse : CommunicationResponse
    {
        public Guid DeployIdentifier { get; set; }
    }
}