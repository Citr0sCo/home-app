using HomeBoxLanding.Api.Features.Columns.Types;
using HomeBoxLanding.Api.Features.Folders;
using HomeBoxLanding.Api.Features.Links;

namespace HomeBoxLanding.Api.Features.Columns;

public class ColumnMapper
{
    public static Column Map(ColumnRecord record)
    {
        return new Column
        {
            Identifier = record.Identifier,
            Name = record.Name,
            SortOrder = record.SortOrder,
            Icon = record.Icon,
            // Links held by a folder are returned on that folder, never loose on the column.
            Links = record.Links
                .Where(x => x.FolderIdentifier == null)
                .OrderBy(x => x.SortOrder)
                .Select(LinkMapper.Map)
                .ToList(),
            Folders = record.Folders
                .OrderBy(x => x.SortOrder)
                .Select(FolderMapper.Map)
                .ToList()
        };
    }
}
