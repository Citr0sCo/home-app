namespace HomeBoxLanding.Api.Features.Columns.Types;

public class ImportColumnsRequest
{
    public ImportColumnsRequest()
    {
        Columns = new List<Column>();
    }
    
    public List<Column> Columns { get; set; }
}