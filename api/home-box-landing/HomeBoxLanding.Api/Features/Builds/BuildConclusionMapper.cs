using HomeBoxLanding.Api.Features.Builds.Types;

namespace HomeBoxLanding.Api.Features.Builds
{
    public class BuildConclusionMapper
    {
        public static BuildConclusion Map(string? conclusion)
        {
            return conclusion switch
            {
                "success" => BuildConclusion.Success,
                "failure" => BuildConclusion.Failure,
                _ => BuildConclusion.Unknown
            };
        }
    }
}