using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Features.Folders.Types;

namespace HomeBoxLanding.Api.Features.Folders;

public class FoldersService
{
    private readonly IFoldersRepository _repository;

    public FoldersService(IFoldersRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Folder>> GetAll()
    {
        var records = await _repository.GetAll();

        return records.ConvertAll(FolderMapper.Map);
    }

    public async Task<CreateFolderResponse> Create(CreateFolderRequest request)
    {
        return await _repository.Create(request);
    }

    public async Task<UpdateFolderResponse> Update(UpdateFolderRequest request)
    {
        return await _repository.Update(request);
    }

    public async Task<CommunicationResponse> Delete(Guid identifier)
    {
        return await _repository.Delete(identifier);
    }
}
