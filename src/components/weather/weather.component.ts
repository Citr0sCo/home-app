import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { WeatherService } from '../../services/weather-service/weather.service';
import { IWeatherData } from '../../services/weather-service/types/weather-data.type';
import { LocationService } from '../../services/location-service/location.service';

@Component({
    selector: 'weather',
    templateUrl: './weather.component.html',
    styleUrls: ['./weather.component.scss']
})
export class WeatherComponent implements OnInit, OnDestroy {

    public isCheckingWeather: boolean = false;
    public weather: IWeatherData | null = null;

    public currentTime: Date = new Date();
    private _locationService: LocationService;
    private _subscriptions: Subscription = new Subscription();
    private readonly _weatherService: WeatherService;

    constructor(locationService: LocationService, weatherService: WeatherService) {
        this._locationService = locationService;
        this._weatherService = weatherService;
    }

    public ngOnInit() {
        this._subscriptions.add(
            this._locationService.getLocation()
                .subscribe((response) => {
                    this._weatherService.getWeatherFor(response.latitude, response.longitude)
                        .subscribe((response) => {
                            this.weather = response;
                        });
                })
        );
    }

    public getWeatherIcon(weatherDescription: string): string {

        if (weatherDescription.toUpperCase() === 'CLOUDS') {
            return 'fa fa-cloud';
        }

        if (weatherDescription.toUpperCase() === 'RAIN') {
            return 'fa fa-tint';
        }

        if (weatherDescription.toUpperCase() === 'CLEAR') {
            return 'fa fa-sun-o';
        }

        if (this.currentTime.getHours() < 6 && this.currentTime.getHours() > 18) {
            return 'fa fa-moon-o';
        }

        return 'fa fa-sun-o';
    }

    public roundWeatherTemperature(temperature: number): string {
        return `${Math.round(temperature * 100) / 100}℃`;
    }

    public titleCase(input: string): string {
        const splitStr = input.toLowerCase().split(' ');
        for (let i = 0; i < splitStr.length; i++) {
            splitStr[i] = splitStr[i].charAt(0).toUpperCase() + splitStr[i].substring(1);
        }
        return splitStr.join(' ');
    }

    public refreshWeather(): void {
        this.isCheckingWeather = true;

        this._locationService.getCurrentLocation()
            .subscribe((location) => {
                this._weatherService.getLiveWeather(location.latitude, location.longitude)
                    .subscribe((response) => {
                        this.weather = response;
                        this.isCheckingWeather = false;
                    });
            });
    }

    public ngOnDestroy() {
        this._subscriptions.unsubscribe();
    }
}
