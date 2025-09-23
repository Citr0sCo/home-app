namespace HomeBoxLanding.Api.Features.Columns.Types;

public class GetAllColumnsResponse
{
    public GetAllColumnsResponse()
    {
        Columns = new List<Column>();
    }
    
    public List<Column> Columns { get; set; }
}