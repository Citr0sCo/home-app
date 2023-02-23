using HomeBoxLanding.Api.Core.Types;

namespace HomeBoxLanding.Api.Features.Builds.Types
{
    public class SaveBuildResponse : CommunicationResponse
    {
        public Guid BuildIdentifier { get; set; }
    }
}