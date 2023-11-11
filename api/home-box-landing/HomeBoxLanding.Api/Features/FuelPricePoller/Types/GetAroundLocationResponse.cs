namespace HomeBoxLanding.Api.Features.FuelPricePoller.Types;

public class GetAroundLocationResponse
{
    public List<FuelPriceRecord> Stations { get; set; }
}