using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Features.Builds.Types;
using HomeBoxLanding.Api.Features.Deploys.Types;

namespace HomeBoxLanding.Api.Features.Builds;

public class BuildsService
{
    private readonly IBuildsRepository _buildsRepository;

    public BuildsService(IBuildsRepository buildsRepository)
    {
        _buildsRepository = buildsRepository;
    }

    public BuildsResponse GetAllBuilds()
    {
        var builds = _buildsRepository.GetAll();

        return new BuildsResponse
        {
            Builds = builds.ConvertAll(x => new Build
            {
                Identifier = x.Identifier,
                FinishedAt = x.FinishedAt,
                StartedAt = x.StartedAt,
                Conclusion = x.Conclusion,
                Status = x.Status
            })
        };
    }

    public GetBuildResponse GetBuild(string githubBuildReference)
    {
        var response = new GetBuildResponse();

        var build = _buildsRepository.GetBuild(githubBuildReference);

        if (build == null)
        {
            response.AddError(new Error
            {
                Code = ErrorCode.BuildNotFound, 
                UserMessage = "Build not found.", 
                TechnicalMessage = $"Build for reference '{githubBuildReference}' not found."
            });
            return response;
        }
        
        return new GetBuildResponse
        {
            Build = new Build
            {
                Identifier = build.Identifier,
                FinishedAt = build.FinishedAt,
                StartedAt = build.StartedAt,
                Conclusion = build.Conclusion,
                Status = build.Status,
                GithubBuildReference = build.GithubBuildReference
            }
        };
    }

    public SaveBuildResponse SaveBuild(SaveBuildRequest request)
    {
        var response = new SaveBuildResponse();
        
        var builds = _buildsRepository.SaveBuild(request);

        if (builds.HasError)
        {
            response.AddError(new Error
                {
                    Code = ErrorCode.FailedToCreateBuild,
                    UserMessage = "Failed to create a build.",
                    TechnicalMessage = "Failed to create a build."
                }
            );
            return response;
        }
        
        return new SaveBuildResponse
        {
            BuildIdentifier = builds.BuildIdentifier
        };
    }
    public UpdateBuildResponse UpdateBuild(UpdateBuildRequest request)
    {
        var response = new UpdateBuildResponse();
        
        var builds = _buildsRepository.UpdateBuild(request);

        if (builds.HasError)
        {
            response.AddError(new Error
                {
                    Code = ErrorCode.FailedToUpdateBuild,
                    UserMessage = "Failed to create a build.",
                    TechnicalMessage = "Failed to create a build."
                }
            );
            return response;
        }
        
        return new UpdateBuildResponse
        {
            BuildIdentifier = builds.BuildIdentifier
        };
    }
}