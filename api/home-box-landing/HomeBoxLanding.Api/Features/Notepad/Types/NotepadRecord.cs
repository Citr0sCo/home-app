using System.ComponentModel.DataAnnotations;

namespace HomeBoxLanding.Api.Features.Notepad.Types;

public class NotepadRecord
{
    [Key]
    public Guid Identifier { get; set; }
    public string Note { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}