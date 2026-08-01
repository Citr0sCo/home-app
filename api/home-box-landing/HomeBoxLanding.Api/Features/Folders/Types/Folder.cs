using HomeBoxLanding.Api.Features.Links.Types;

namespace HomeBoxLanding.Api.Features.Folders.Types;

public class Folder
{
    public Folder()
    {
        Links = new List<Link>();
    }

    public Guid? Identifier { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
    public int SortOrder { get; set; }
    public Guid ColumnId { get; set; }
    public List<Link> Links { get; set; }
}
