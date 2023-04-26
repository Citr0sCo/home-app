using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Features.Builds.Types;

namespace HomeBoxLanding.Api.Features.Deploys.Types;

public class GetBuildResponse : CommunicationResponse
{
    public Build Build { get; set; }
}