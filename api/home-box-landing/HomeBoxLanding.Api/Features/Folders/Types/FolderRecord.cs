using System.ComponentModel.DataAnnotations;
using HomeBoxLanding.Api.Features.Columns.Types;
using HomeBoxLanding.Api.Features.Links.Types;

namespace HomeBoxLanding.Api.Features.Folders.Types;

public class FolderRecord
{
    public FolderRecord()
    {
        Links = new List<LinkRecord>();
    }

    [Key]
    public Guid Identifier { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
    public int SortOrder { get; set; }
    public Guid ColumnIdentifier { get; set; }
    public ColumnRecord Column { get; set; }
    public List<LinkRecord> Links { get; set; }
}
