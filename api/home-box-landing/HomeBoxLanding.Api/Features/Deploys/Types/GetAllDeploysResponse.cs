using HomeBoxLanding.Api.Core.Types;

namespace HomeBoxLanding.Api.Features.Deploys.Types
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