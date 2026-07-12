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
    
    public NotepadRecord? CreateNotepad()
    {
        using (var context = new DatabaseContext())
        using (var transaction = context.Database.BeginTransaction())
        {
            try
            {
                var notepad = new NotepadRecord
                {
                    Identifier = Guid.NewGuid(),
                    Note = "New note",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                
                context.Notepads
                    .Add(notepad);
                
                context.SaveChanges();
                transaction.Commit();
                
                return notepad;
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                return null;
            }
        }
    }
    
    public NotepadRecord? UpdateNotepad(string note)
    {
        using (var context = new DatabaseContext())
        using (var transaction = context.Database.BeginTransaction())
        {
            try
            {
                var notepad = context.Notepads
                                .FirstOrDefault();
                
                notepad!.Note = note;
                notepad!.UpdatedAt = DateTime.Now;
                
                context.Notepads
                    .Update(notepad);
                
                context.SaveChanges();
                transaction.Commit();
                
                return notepad;
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                return null;
            }
        }
    }
}