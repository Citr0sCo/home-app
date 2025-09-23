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
    public ActionResult Create([FromBody] CreateColumnRequest request)
    {
        
        
        return Ok();
    }
}