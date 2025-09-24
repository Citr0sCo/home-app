using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Features.Columns.Types;
using HomeBoxLanding.Api.Features.Links;

namespace HomeBoxLanding.Api.Features.Columns;

public class ColumnsService
{
    private readonly IColumnsRepository _repository;

    public ColumnsService(IColumnsRepository repository)
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
            Links = x.Links
                .ConvertAll(LinkMapper.Map)
                .OrderBy(x => x.SortOrder)
                .ToList()
        });
    }

    public async Task<CreateColumnResponse> Create(CreateColumnRequest request)
    {
        var record = await _repository.Create(request);

        return record;
    }

    public async Task<UpdateColumnResponse> Update(UpdateColumnRequest request)
    {
        var record = await _repository.Update(request);

        return record;
    }

    public async Task<CommunicationResponse> Delete(Guid identifier)
    {
        return await _repository.Delete(identifier);
    }

    public async Task<ImportColumnsResponse> Import(ImportColumnsRequest request)
    {
        var response = new ImportColumnsResponse();

        var importResponse = await _repository.Import(request);

        if (importResponse.HasError)
        {
            response.AddError(importResponse.Error);
            return response;
        }

        response.Columns = importResponse.Columns;
        return response;
    }
}