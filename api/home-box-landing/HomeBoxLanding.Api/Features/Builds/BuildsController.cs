using HomeBoxLanding.Api.Features.Builds.Types;
using Microsoft.AspNetCore.Mvc;

namespace HomeBoxLanding.Api.Features.Builds
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildsController : ControllerBase
    {
        private readonly BuildsService _service;

        public BuildsController()
        {
            _service = new BuildsService(new BuildsRepository());
        }

        [HttpGet]
        public BuildsResponse GetAll()
        {
            return _service.GetAllBuilds();
        }
    }
}