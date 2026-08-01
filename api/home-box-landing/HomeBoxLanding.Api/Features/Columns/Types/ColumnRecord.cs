using System.ComponentModel.DataAnnotations;
using HomeBoxLanding.Api.Features.Folders.Types;
using HomeBoxLanding.Api.Features.Links.Types;

namespace HomeBoxLanding.Api.Features.Columns.Types;

public class ColumnRecord
{
    public ColumnRecord()
    {
        Links = new List<LinkRecord>();
        Folders = new List<FolderRecord>();
    }

    [Key]
    public Guid Identifier { get; set; }
    public string Name { get; set; }
    public int SortOrder { get; set; }
    public string Icon { get; set; }
    public List<LinkRecord> Links { get; set; }
    public List<FolderRecord> Folders { get; set; }
}