using HomeBoxLanding.Api.Features.Columns.Types;
using Microsoft.AspNetCore.Mvc;

namespace HomeBoxLanding.Api.Features.Columns;

[ApiExplorerSettings(IgnoreApi = true)]
[Route("api/[controller]")]
public class ColumnsController : Controller
{
    private readonly ColumnsService _service;

    public ColumnsController()
    {
        _service = new ColumnsService(new ColumnsRepository());
    }
    
    
    [HttpGet("")]
    public async Task<ActionResult> Get()
    {
        var columns = await _service.GetAll();

        var response = new GetAllColumnsResponse
        {
            Columns = columns
        };
        
        return Ok(response);
    }
        
    [HttpPost("")]
    public async Task<ActionResult> Create([FromBody] CreateColumnRequest request)
    {
        var response = await _service.Create(request);

        return Ok(response);
    }
        
    [HttpPatch("{reference}")]
    public async Task<ActionResult> Update(Guid reference, [FromBody] UpdateColumnRequest request)
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

    [HttpPost("import")]
    public async Task<ImportColumnsResponse> Import([FromBody]ImportColumnsRequest request)
    {
        return await _service.Import(request);
    }
}