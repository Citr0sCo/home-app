using HomeBoxLanding.Api.Features.Folders.Types;
using HomeBoxLanding.Api.Features.Links;

namespace HomeBoxLanding.Api.Features.Folders;

public class FolderMapper
{
    public static Folder Map(FolderRecord record)
    {
        return new Folder
        {
            Identifier = record.Identifier,
            Name = record.Name,
            Icon = record.Icon,
            SortOrder = record.SortOrder,
            ColumnId = record.ColumnIdentifier,
            Links = record.Links
                .OrderBy(x => x.SortOrder)
                .Select(LinkMapper.Map)
                .ToList()
        };
    }
}
