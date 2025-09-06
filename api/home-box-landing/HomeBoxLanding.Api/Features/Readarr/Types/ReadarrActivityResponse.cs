namespace HomeBoxLanding.Api.Features.Readarr.Types;

public class ReadarrActivityResponse
{
    public ReadarrActivityResponse()
    {
        Health = new List<ReadarrHealth>();
    }
    
    public int TotalNumberOfBooks { get; set; }
    public int TotalNumberOfQueuedBooks { get; set; }
    public int TotalMissingBooks { get; set; }
    public List<ReadarrHealth> Health { get; set; }
}