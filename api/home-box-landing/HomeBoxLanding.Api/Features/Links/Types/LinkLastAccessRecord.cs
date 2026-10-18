using System.ComponentModel.DataAnnotations;

namespace HomeBoxLanding.Api.Features.Links.Types;

public class LinkLastAccessRecord
{
    [Key]
    public Guid LinkIdentifier { get; set; }
    public DateTime LastAccessedAt { get; set; }
}
