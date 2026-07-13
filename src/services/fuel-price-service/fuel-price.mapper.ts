import { IFuelPrice } from './types/fuel-price.type';

export class FuelPriceMapper {

    public static map(response: any): Array<IFuelPrice> {
        return response.map((fuelStation: any) => FuelPriceMapper.mapSingle(fuelStation));
    }

    public static mapSingle(fuelPriceRecord: any): IFuelPrice {

        const brandInfo = this.generateColourFromBrand(fuelPriceRecord.Brand);

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
            colour: brandInfo.colour,
            logo: brandInfo.logo
        };
    }

    public static generateColourFromBrand(brand: string): { colour: string; logo: string } {

        brand = brand?.toLowerCase().trim();

        if (brand.indexOf('tesco') > -1) {
            return {
                colour: '#00539F',
                logo: './assets/fuel-providers/tesco-logo.webp'
            };
        }

        if (brand.indexOf('sainsbury') > -1) {
            return {
                colour: '#F06C00',
                logo: './assets/fuel-providers/sainsburys-logo.png'
            };
        }

        if (brand.indexOf('texaco') > -1) {
            return {
                colour: '#e93330',
                logo: './assets/fuel-providers/texaco-logo.png'
            };
        }

        if (brand.indexOf('esso') > -1) {
            return {
                colour: '#a50e91',
                logo: './assets/fuel-providers/esso-logo.png'
            };
        }

        if (brand.indexOf('asda') > -1) {
            return {
                colour: '#78BE20',
                logo: './assets/fuel-providers/asda-logo.png'
            };
        }

        if (brand.indexOf('jet') > -1) {
            return {
                colour: '#f7c801',
                logo: './assets/fuel-providers/jet-logo.png'
            };
        }

        if (brand.indexOf('shell') > -1) {
            return {
                colour: '#FFD500',
                logo: './assets/fuel-providers/shell-logo.png'
            };
        }

        if (brand.indexOf('applegreen') > -1) {
            return {
                colour: '#6ebd00',
                logo: './assets/fuel-providers/applegreen-logo.png'
            };
        }

        if (brand.indexOf('morrisons') > -1) {
            return {
                colour: '#00712f',
                logo: './assets/fuel-providers/morrisons-logo.png'
            };
        }

        if (brand.indexOf('bp') > -1) {
            return {
                colour: '#007f00',
                logo: './assets/fuel-providers/bp-logo.png'
            };
        }

        if (brand.indexOf('essar') > -1) {
            return {
                colour: '#f03e35',
                logo: './assets/fuel-providers/essar-logo.png'
            };
        }

        return {
            colour: '#414141',
            logo: ''
        };
    };
}