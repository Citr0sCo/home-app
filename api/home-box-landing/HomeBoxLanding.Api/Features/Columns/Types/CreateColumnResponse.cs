using HomeBoxLanding.Api.Core.Types;

namespace HomeBoxLanding.Api.Features.Columns.Types;

public class CreateColumnResponse : CommunicationResponse
{
    public Column Column { get; set; }
}