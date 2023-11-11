import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { LocationService } from '../../services/location-service/location.service';
import { IFuelPrice } from '../../services/fuel-price-service/types/fuel-price.type';
import { FuelPriceService } from '../../services/fuel-price-service/fuel-price.service';
import { ILocationData } from '../../services/location-service/types/location-data.type';

@Component({
    selector: 'fuel-prices',
    templateUrl: './fuel-prices.component.html',
    styleUrls: ['./fuel-prices.component.scss']
})
export class FuelPricesComponent implements OnInit, OnDestroy {

    public fuelStations: Array<IFuelPrice> = [];

    private _locationService: LocationService;
    private _subscriptions: Subscription = new Subscription();
    private readonly _fuelPriceService: FuelPriceService;
    private _locationData: ILocationData | null = null;

    constructor(locationService: LocationService, fuelPriceService: FuelPriceService) {
        this._locationService = locationService;
        this._fuelPriceService = fuelPriceService;
    }

    public ngOnInit() {
        this._subscriptions.add(
            this._locationService.getLocation()
                .subscribe((response) => {
                    this._locationData = response;
                })
        );
    }

    public triggerFuelStationLookup(): void {
        this._fuelPriceService.getAroundLocation(this._locationData!)
            .subscribe((fuelStations) => {
                this.fuelStations = fuelStations;
            });
    }

    public ngOnDestroy() {
        this._subscriptions.unsubscribe();
    }
}
