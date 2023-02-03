using HomeBoxLanding.Api.Core.Types;

namespace HomeBoxLanding.Api.Features.Deploy.Types
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