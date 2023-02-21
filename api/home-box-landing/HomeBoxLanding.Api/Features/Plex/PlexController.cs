using HomeBoxLanding.Api.Core.Shell;
using HomeBoxLanding.Api.Features.Plex.Types;
using Microsoft.AspNetCore.Mvc;

namespace HomeBoxLanding.Api.Features.Plex
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlexController : ControllerBase
    {
        private readonly PlexService _service;

        public PlexController()
        {
            _service = new PlexService(new HttpClient());
        }

        [HttpGet("activity")]
        public PlexActivityResponse GetActivity()
        {
            return _service.GetActivity();
        }
    }
}