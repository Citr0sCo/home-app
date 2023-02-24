using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Features.Links.Types;

namespace HomeBoxLanding.Api.Features.Links
{
    public class AddLinkResponse : CommunicationResponse
    {
        public Link Link { get; set; }
    }
}