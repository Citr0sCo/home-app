using HomeBoxLanding.Api.Features.Builds.Types;

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

    public BuildsResponse GetBuild(string githubBuildReference)
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

    public SaveBuildResponse SaveBuild(SaveBuildRequest request)
    {
        var builds = _buildsRepository.SaveBuild(request);

        return new SaveBuildResponse
        {
            BuildIdentifier = builds.BuildIdentifier
        };
    }
}