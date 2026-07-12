using HomeBoxLanding.Api.Features.Notepad.Types;

namespace HomeBoxLanding.Api.Features.Notepad;

public class NotepadService
{
    private readonly NotepadRepository _notepadRepository;

    public NotepadService(NotepadRepository notepadRepository)
    {
        _notepadRepository = notepadRepository;
    }

    public GetNotepadResponse GetNotepad()
    {
        var notepad = _notepadRepository.GetNotepad();

        if (notepad == null)
            return new GetNotepadResponse();

        return new GetNotepadResponse()
        {
            Notepad = new NotepadModel
            {
                Identifier = notepad.Identifier,
                Note = notepad.Note,
                CreatedAt = notepad.CreatedAt,
                UpdatedAt = notepad.UpdatedAt
            }
        };
    }

    public CreateNotepadResponse CreateNotepad()
    {
        var notepad = _notepadRepository.CreateNotepad();

        if (notepad == null)
            return new CreateNotepadResponse();

        return new CreateNotepadResponse()
        {
            Notepad = new NotepadModel
            {
                Identifier = notepad.Identifier,
                Note = notepad.Note,
                CreatedAt = notepad.CreatedAt,
                UpdatedAt = notepad.UpdatedAt
            }
        };
    }

    public UpdateNotepadResponse UpdateNotepad(UpdateNotepadRequest request)
    {
        var notepad = _notepadRepository.UpdateNotepad(request.Note);

        if (notepad == null)
            return new UpdateNotepadResponse();

        return new UpdateNotepadResponse()
        {
            Notepad = new NotepadModel
            {
                Identifier = notepad.Identifier,
                Note = notepad.Note,
                CreatedAt = notepad.CreatedAt,
                UpdatedAt = notepad.UpdatedAt
            }
        };
    }
}