namespace HomeBoxLanding.Api.Features.Links.Types;

public class LinkLastAccess
{
    public Guid Identifier { get; set; }
    public DateTime LastAccessedAt { get; set; }
}
