using HomeBoxLanding.Api.Features.Columns.Types;
using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.Links.Types;

namespace HomeBoxLanding.Api.Features.Columns;

public class ColumnsService
{
    private readonly ColumnsRepository _repository;

    public ColumnsService(ColumnsRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Column>> GetAll()
    {
        var records = await _repository.GetAll();

        return records.ConvertAll(x => new Column
        {
            Identifier = x.Identifier,
            Name = x.Name,
            SortOrder = x.SortOrder,
            Icon = x.Icon,
            Links = x.Links.ConvertAll(LinkMapper.Map)
        });
    }
}