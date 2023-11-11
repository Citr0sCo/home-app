import { Injectable } from '@angular/core';
import { IFuelPrice } from './types/fuel-price.type';
import { Observable, of, tap } from 'rxjs';
import { FuelPriceRepository } from './fuel-price.repository';
import { ILocationData } from '../location-service/types/location-data.type';

@Injectable()
export class FuelPriceService {

    private _fuelPriceRepository: FuelPriceRepository;
    private _cachedFuelStations: Array<IFuelPrice> | null = null;

    constructor(fuelPriceRepository: FuelPriceRepository) {
        this._fuelPriceRepository = fuelPriceRepository;
    }

    public refreshCache(locationData: ILocationData): Observable<Array<IFuelPrice>> {
        return this._fuelPriceRepository.getAroundLocation(locationData.latitude ?? null, locationData.longitude ?? null)
            .pipe(
                tap((fuelStations) => {
                    this._cachedFuelStations = fuelStations;
                    localStorage.setItem('cachedFuelStations', JSON.stringify(fuelStations));
                })
            );
    }

    public getAroundLocation(locationData: ILocationData): Observable<Array<IFuelPrice>> {

        if (localStorage.getItem('cachedFuelStations')) {
            this._cachedFuelStations = JSON.parse(`${localStorage.getItem('cachedFuelStations')}`);
        }

        if (this._cachedFuelStations !== null && this._cachedFuelStations.length > 0) {
            return of(this._cachedFuelStations);
        }

        return this.refreshCache(locationData);
    }
}
