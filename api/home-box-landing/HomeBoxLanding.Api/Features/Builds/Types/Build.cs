namespace HomeBoxLanding.Api.Features.Builds.Types;

public class Build
{
    public Guid Identifier { get; set; }
    public string? Status { get; set; }
    public string? Conclusion { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public string? GithubBuildReference { get; set; }
}