using System.ComponentModel.DataAnnotations;

namespace HomeBoxLanding.Api.Features.Builds.Types
{
    public class BuildRecord
    {
        [Key]
        public Guid Identifier { get; set; }
        public BuildStatus Status { get; set; }
        public BuildConclusion Conclusion { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public string GithubBuildReference { get; set; }
    }

    public enum BuildStatus
    {
        Unknown = 0,
        Completed,
        InProgress,
        Requested
    }

    public enum BuildConclusion
    {
        Unknown = 0,
        Success,
        Failure
    }
}