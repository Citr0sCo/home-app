namespace HomeBoxLanding.Api.Features.Deploys.Types;

public class GithubBuildRequest
{
    public GithubBuildRequest()
    {
        workflow_run = new GithubWorkflowRun();
    }
        
    public GithubWorkflowRun workflow_run { get; set; }
}

public class GithubWorkflowRun
{
    public string status { get; set; }
    public string? conclusion { get; set; }
    public string head_sha { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}