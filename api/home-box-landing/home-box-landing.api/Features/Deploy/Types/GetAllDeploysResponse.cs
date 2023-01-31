using home_box_landing.api.Core.Types;

namespace home_box_landing.api.Features.Deploy.Types
{
    public class GetAllDeploysResponse : CommunicationResponse
    {
        public GetAllDeploysResponse()
        {
            Deploys = new List<DeployModel>();
        }
        
        public List<DeployModel> Deploys { get; set; }
    }
}