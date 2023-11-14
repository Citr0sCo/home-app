import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subject, Subscription } from 'rxjs';
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

    @Input()
    public refreshCache: Subject<boolean> = new Subject<boolean>();

    @Input()
    public showResults: boolean = false;

    public fuelStations: Array<IFuelPrice> = [];
    public locationRange: string = '5';
    public locationData: ILocationData | null = null;
    public isLoading: boolean = false;

    private readonly _locationService: LocationService;
    private readonly _subscriptions: Subscription = new Subscription();
    private readonly _fuelPriceService: FuelPriceService;

    constructor(locationService: LocationService, fuelPriceService: FuelPriceService) {
        this._locationService = locationService;
        this._fuelPriceService = fuelPriceService;
    }

    public ngOnInit() {
        this._subscriptions.add(
            this._locationService.getLocation()
                .subscribe((response) => {
                    this.locationData = response;
                    this.triggerFuelStationLookup();
                })
        );

        this._subscriptions.add(
            this.refreshCache
                .subscribe((shouldRefresh) => {
                    if (shouldRefresh) {
                        this.refreshFuelPrices();
                    }
                })
        );
    }

    public triggerFuelStationLookup(): void {
        this.isLoading = true;
        this._fuelPriceService.getAroundLocation(this.locationData!, this.locationRange, true)
            .subscribe((fuelStations) => {
                this.isLoading = false;
                this.showResults = true;
                this.fuelStations = fuelStations;
            });
    }

    public handleMapClick(fuelStation: IFuelPrice): void {
        const element = document.querySelector(`.${fuelStation.name}`);
        element!.classList.add('highlight-station');

        setTimeout(() => {
            element!.classList.remove('highlight-station');
        }, 10000);
    }

    public refreshFuelPrices(): void {
        this.isLoading = true;

        this._locationService.getCurrentLocation(true)
            .subscribe((response) => {
                this._fuelPriceService.refreshCache(this.locationData!, this.locationRange)
                    .subscribe((fuelStations) => {
                        this.isLoading = false;
                        this.fuelStations = fuelStations;
                    });
            });
    }

    public ngOnDestroy() {
        this._subscriptions.unsubscribe();
    }
}
