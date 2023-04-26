using HomeBoxLanding.Api.Core.Types;

namespace HomeBoxLanding.Api.Features.Builds.Types;

public class UpdateBuildResponse : CommunicationResponse
{
    public Guid BuildIdentifier { get; set; }
}