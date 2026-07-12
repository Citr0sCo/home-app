using HomeBoxLanding.Api.Data;
using HomeBoxLanding.Api.Features.Notepad.Types;

namespace HomeBoxLanding.Api.Features.Notepad;

public interface INotepadRepository
{
    NotepadRecord? GetNotepad();
}

public class NotepadRepository : INotepadRepository
{
    public NotepadRecord? GetNotepad()
    {
        using (var context = new DatabaseContext())
        {
            try
            {
                return context.Notepads
                    .FirstOrDefault();
            }
            catch (Exception exception)
            {
                return null;
            }
        }
    }
}