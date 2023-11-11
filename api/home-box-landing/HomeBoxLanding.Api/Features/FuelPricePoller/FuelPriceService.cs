using HomeBoxLanding.Api.Features.FuelPricePoller.Types;

namespace HomeBoxLanding.Api.Features.FuelPricePoller;

public class FuelPriceService
{
    private readonly FuelPriceRepository _repository;

    public FuelPriceService(FuelPriceRepository repository)
    {
        _repository = repository;
    }

    public List<FuelPriceRecord> GetClosestTo(double latitude, double longitude)
    {
        return _repository.GetFuelPrices(latitude, longitude);
    }
}