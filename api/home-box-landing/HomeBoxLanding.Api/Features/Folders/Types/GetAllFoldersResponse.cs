namespace HomeBoxLanding.Api.Features.Folders.Types;

public class GetAllFoldersResponse
{
    public GetAllFoldersResponse()
    {
        Folders = new List<Folder>();
    }

    public List<Folder> Folders { get; set; }
}
