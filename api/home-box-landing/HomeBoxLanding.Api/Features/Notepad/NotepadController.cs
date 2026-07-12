using HomeBoxLanding.Api.Features.Notepad.Types;
using Microsoft.AspNetCore.Mvc;

namespace HomeBoxLanding.Api.Features.Notepad;

[ApiController]
[Route("api/[controller]")]
public class NotepadController : ControllerBase
{
    private readonly NotepadService _service;

    public NotepadController()
    {
        _service = new NotepadService(new NotepadRepository());
    }

    [HttpGet("")]
    public GetNotepadResponse GetNotepad()
    {
        return _service.GetNotepad();
    }

    [HttpPost("")]
    public CreateNotepadResponse CreateNotepad()
    {
        return _service.CreateNotepad();
    }

    [HttpPatch("")]
    public UpdateNotepadResponse UpdateNotepad([FromBody] UpdateNotepadRequest request)
    {
        return _service.UpdateNotepad(request);
    }
}