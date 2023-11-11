using HomeBoxLanding.Api.Data;
using HomeBoxLanding.Api.Features.FuelPricePoller.Types;
using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.FuelPricePoller;

public class FuelPriceRepository
{
    public async Task SaveFuelPricesFor(FuelProvider provider, string data)
    {
        var records = ParseDataBasedOnProvider(provider, data);

        using (var context = new DatabaseContext())
        using (var transaction = context.Database.BeginTransaction())
        {
            try
            {
                foreach (var record in records)
                {
                    var existingRecord = context.FuelPrices.FirstOrDefault(x => x.Name == record.Name);
                    
                    if(existingRecord == null)
                        await context.AddAsync(record).ConfigureAwait(false);
                    else
                    {
                        existingRecord.Address = record.Address;
                        existingRecord.Postcode = record.Postcode;
                        existingRecord.Provider = provider;
                        existingRecord.Brand = record.Brand;
                        existingRecord.Latitude = record.Latitude;
                        existingRecord.Longitude = record.Longitude;
                        existingRecord.Petrol_E5_Price = record.Petrol_E5_Price;
                        existingRecord.Petrol_E10_Price = record.Petrol_E10_Price;
                        existingRecord.Diesel_B7_Price = record.Diesel_B7_Price;
                        existingRecord.UpdatedAt = record.UpdatedAt;
                    }
                }

                await context.SaveChangesAsync().ConfigureAwait(false);
                await transaction.CommitAsync().ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync().ConfigureAwait(false);
            }
        }
    }

    private List<FuelPriceRecord> ParseDataBasedOnProvider(FuelProvider provider, string data)
    {
        var records = new List<FuelPriceRecord>();
        
        var parsedData = JsonConvert.DeserializeObject<TescoFuelDataResponse>(data);
        
        foreach (var station in parsedData?.Stations ?? new List<TescoFuelDataStation>())
        {
            records.Add(new FuelPriceRecord
            {
                Name = station.SiteId,
                Address = station.Address,
                Postcode = station.Postcode,
                Provider = provider,
                Brand = station.Brand,
                Latitude = station.Location.Latitude ?? 0,
                Longitude = station.Location.Longitude ?? 0,
                Petrol_E5_Price = station.Prices.Petrol_E5 ?? 0,
                Petrol_E10_Price = station.Prices.Petrol_E10 ?? 0,
                Diesel_B7_Price = station.Prices.Diesel_B7 ?? 0,
                UpdatedAt = new DateTime(parsedData.LastUpdated.Ticks, DateTimeKind.Utc),
                CreatedAt = DateTime.UtcNow
            });
        }

        return records;
    }
}