namespace HomeBoxLanding.Api.Features.Notepad.Types;

public class NotepadModel
{
    public Guid Identifier { get; set; }
    public string Note { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }   
}