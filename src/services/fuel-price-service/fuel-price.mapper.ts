import { IFuelPrice } from './types/fuel-price.type';

export class FuelPriceMapper {

    public static map(response: any): Array<IFuelPrice> {
        return response.map((fuelStation: any) => FuelPriceMapper.mapSingle(fuelStation));
    }

    public static mapSingle(fuelPriceRecord: any): IFuelPrice {
        return {
            identifier: fuelPriceRecord.Identifier,
            name: fuelPriceRecord.Name,
            address: fuelPriceRecord.Address,
            postcode: fuelPriceRecord.Postcode,
            provider: fuelPriceRecord.Provider,
            brand: fuelPriceRecord.Brand,
            latitude: fuelPriceRecord.Latitude,
            longitude: fuelPriceRecord.Longitude,
            petrol_e5_price: fuelPriceRecord.Petrol_E5_Price,
            petrol_e10_price: fuelPriceRecord.Petrol_E10_Price,
            diesel_b7_price: fuelPriceRecord.Diesel_B7_Price,
            updatedAt: fuelPriceRecord.UpdatedAt,
            createdAt: fuelPriceRecord.CreatedAt,
            distanceInMeters: fuelPriceRecord.DistanceInMeters
        };
    }
}