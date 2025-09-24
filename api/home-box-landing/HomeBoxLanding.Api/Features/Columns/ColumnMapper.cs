using HomeBoxLanding.Api.Features.Columns.Types;
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
            Links = record.Links.ConvertAll(LinkMapper.Map)
        };
    }
}