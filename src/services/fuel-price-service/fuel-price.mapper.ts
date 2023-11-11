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
            distanceInMeters: fuelPriceRecord.DistanceInMeters,
            colour: this.generateColourFromBrand(fuelPriceRecord.Brand)
        };
    }

    public static generateColourFromBrand(brand: string) {

        if(brand.toLowerCase() === 'tesco') {
            return '#00539F';
        }

        if(brand.toLowerCase() === 'sainsbury\'s') {
            return '#F06C00';
        }

        if(brand.toLowerCase() === 'texaco') {
            return '#e93330';
        }

        if(brand.toLowerCase() === 'esso') {
            return '#a50e91';
        }

        if(brand.toLowerCase() === 'asda') {
            return '#78BE20';
        }

        if(brand.toLowerCase() === 'jet') {
            return '#f7c801';
        }

        if(brand.toLowerCase() === 'shell') {
            return '#FFD500';
        }

        if(brand.toLowerCase() === 'applegreen') {
            return '#6ebd00';
        }

        if(brand.toLowerCase() === 'applegreen') {
            return '#6ebd00';
        }

        if(brand.toLowerCase() === 'morrisons') {
            return '#00712f';
        }

        if(brand.toLowerCase() === 'bp') {
            return '#007f00';
        }

        return '#414141';
    };
}