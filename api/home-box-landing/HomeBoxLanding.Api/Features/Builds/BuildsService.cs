using HomeBoxLanding.Api.Features.Builds.Types;

namespace HomeBoxLanding.Api.Features.Builds;

public class BuildsService
{
    private readonly BuildsRepository _buildsRepository;

    public BuildsService(BuildsRepository buildsRepository)
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
}