using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Features.Links.Types;
using Microsoft.AspNetCore.Mvc;

namespace HomeBoxLanding.Api.Features.Links
{
    [ApiController]
    [Route("api/[controller]")]
    public class LinksController : ControllerBase
    {
        private readonly LinksService _service;

        public LinksController()
        {
            _service = new LinksService(new LinksRepository());
        }

        [HttpGet]
        public LinksResponse GetAll()
        {
            return _service.GetAllLinks();
        }

        [HttpPost]
        public AddLinkResponse AddLink([FromBody]AddLinkRequest request)
        {
            return _service.AddLink(request);
        }

        [HttpDelete("{linkReference}")]
        public CommunicationResponse DeleteLink(Guid linkReference)
        {
            return _service.DeleteLink(linkReference);
        }
    }
}