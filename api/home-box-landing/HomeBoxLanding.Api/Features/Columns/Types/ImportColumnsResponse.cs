using HomeBoxLanding.Api.Core.Types;

namespace HomeBoxLanding.Api.Features.Columns.Types;

public class ImportColumnsResponse : CommunicationResponse
{
    public ImportColumnsResponse()
    {
        Columns = new List<Column>();
    }
    
    public List<Column> Columns { get; set; }
}