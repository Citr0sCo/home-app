using HomeBoxLanding.Api.Core.Types;

namespace HomeBoxLanding.Api.Features.Folders.Types;

public class UpdateFolderResponse : CommunicationResponse
{
    public Folder Folder { get; set; }
}
