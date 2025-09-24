using HomeBoxLanding.Api.Features.Links.Types;

namespace HomeBoxLanding.Api.Features.Columns.Types;

public class Column
{
    public Column()
    {
        Links = new List<Link>();
    }
    
    public Guid? Identifier { get; set; }
    public string Name { get; set; }
    public int SortOrder { get; set; }
    public string Icon { get; set; }
    public List<Link> Links { get; set; }
}