using HomeBoxLanding.Api.Features.Builds.Types;

namespace HomeBoxLanding.Api.Features.Builds;

public class BuildStatusMapper
{
    public static BuildStatus Map(string status)
    {
        return status switch
        {
            "completed" => BuildStatus.Completed,
            "in_progress" => BuildStatus.InProgress,
            "requested" => BuildStatus.Requested,
            "queued" => BuildStatus.Queued,
            _ => BuildStatus.Unknown
        };
    }
}