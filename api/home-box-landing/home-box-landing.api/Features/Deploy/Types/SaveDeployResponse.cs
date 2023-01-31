using home_box_landing.api.Core.Types;

namespace home_box_landing.api.Features.Deploy.Types
{
    public class SaveDeployResponse : CommunicationResponse
    {
        public Guid DeployIdentifier { get; set; }
    }
}