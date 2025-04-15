using HomeBoxLanding.Api.Features.Links;
using Microsoft.AspNetCore.Mvc;

namespace HomeBoxLanding.Api.Features.Files;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly LinksService _linkService;
    private readonly FilesCache _filesCache;

    public FilesController()
    {
        _linkService = new LinksService(new LinksRepository());
        _filesCache = FilesCache.Instance();
    }
    
    [HttpGet("{linkReference:guid}")]
    public IActionResult GetFile([FromRoute] Guid linkReference)
    {
        if (_filesCache.Has(linkReference))
        {
            var linkUrl = _filesCache.Get(linkReference);
            var file = System.IO.File.OpenRead(linkUrl);
            return File(file, $"image/{Path.GetExtension(linkUrl).Replace(".", "")}");
        }

        var link = _linkService.GetLinkByReference(linkReference);

        if (link == null)
            return NotFound();
            
        if (link.IconUrl.StartsWith("http") || link.IconUrl.StartsWith("."))
            return Ok(link.IconUrl);

        _filesCache.Add(linkReference, link.IconUrl);
        var image = System.IO.File.OpenRead(link.IconUrl);
        return File(image, $"image/{Path.GetExtension(link.IconUrl).Replace(".", "")}");
    }

    [HttpDelete("cache")]
    public IActionResult BustCache()
    {
        _filesCache.BustCache();
        return Ok();
    }
}