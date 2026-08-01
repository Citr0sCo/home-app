using HomeBoxLanding.Api.Features.Folders.Types;
using Microsoft.AspNetCore.Mvc;

namespace HomeBoxLanding.Api.Features.Folders;

[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/[controller]")]
public class FoldersController : Controller
{
    private readonly FoldersService _service;

    public FoldersController()
    {
        _service = new FoldersService(new FoldersRepository());
    }

    [HttpGet("")]
    public async Task<ActionResult> Get()
    {
        var folders = await _service.GetAll();

        var response = new GetAllFoldersResponse
        {
            Folders = folders
        };

        return Ok(response);
    }

    [HttpPost("")]
    public async Task<ActionResult> Create([FromBody] CreateFolderRequest request)
    {
        var response = await _service.Create(request);

        return Ok(response);
    }

    [HttpPatch("{reference}")]
    public async Task<ActionResult> Update(Guid reference, [FromBody] UpdateFolderRequest request)
    {
        var response = await _service.Update(request);

        return Ok(response);
    }

    [HttpDelete("{identifier}")]
    public async Task<ActionResult> Delete(Guid identifier)
    {
        var response = await _service.Delete(identifier);

        return Ok(response);
    }
}
