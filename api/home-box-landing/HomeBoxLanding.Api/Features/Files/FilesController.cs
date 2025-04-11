using HomeBoxLanding.Api.Features.Links;
using Microsoft.AspNetCore.Mvc;

namespace HomeBoxLanding.Api.Features.Files;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly LinksService _linkService;

    public FilesController()
    {
        _linkService = new LinksService(new LinksRepository());
    }
    
    [HttpGet("{linkReference:guid}")]
    public IActionResult GetFile([FromRoute] Guid linkReference)
    {
        var link = _linkService.GetLinkByReference(linkReference);

        if (link == null)
            return NotFound();
        
        var image = System.IO.File.OpenRead(link.IconUrl);
        return File(image, $"image/{Path.GetExtension(link.IconUrl).Replace(".", "")}");
    }
}