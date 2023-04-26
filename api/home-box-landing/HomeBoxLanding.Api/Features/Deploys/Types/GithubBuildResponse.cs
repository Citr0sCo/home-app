using HomeBoxLanding.Api.Core.Types;

namespace HomeBoxLanding.Api.Features.Deploys.Types;

public class GitlabBuildResponse : CommunicationResponse
{
    public string? Message { get; set; } 
        
    public GitlabBuildResponse WithMessage(string message)
    {
        Message = message;
        return this;
    }
}