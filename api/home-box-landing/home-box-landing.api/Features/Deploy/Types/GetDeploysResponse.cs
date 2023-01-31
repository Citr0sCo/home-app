using home_box_landing.api.Core.Types;

namespace home_box_landing.api.Features.Deploy.Types
{
    public class GetDeploysResponse : CommunicationResponse
    {
        public GetDeploysResponse()
        {
            Deploys = new List<DeployRecord>();
        }
        public List<DeployRecord> Deploys { get; set; }
    }
}