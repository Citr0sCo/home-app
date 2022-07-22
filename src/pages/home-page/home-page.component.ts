import { Component, OnDestroy, OnInit } from '@angular/core';
import { WeatherService } from "../../services/weather-service/weather.service";
import { Subscription } from "rxjs";
import { IWeatherData } from "../../services/weather-service/types/weather-data.type";
import { LinkService } from "../../services/link-service/link.service";
import { ILink } from "../../services/link-service/types/link.type";

@Component({
    selector: 'home-page',
    templateUrl: './home-page.component.html',
    styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit, OnDestroy {

    public mediaLinks: Array<ILink> = [];
    public systemLinks: Array<ILink> = [];
    public homeAutomationLinks: Array<ILink> = [];
    public weather: IWeatherData | null = null;
    public currentTime: Date = new Date();
    public isCheckingWeather: boolean = false;

    private _subscriptions: Subscription = new Subscription();

    private readonly _weatherService: WeatherService;
    private readonly _linkService: LinkService;

    constructor(weatherService: WeatherService, linkService: LinkService) {
        this._weatherService = weatherService;
        this._linkService = linkService;
    }

    public ngOnInit(): void {
        this._subscriptions.add(
            this._weatherService.getWeatherFor('52.824162', '-1.6396672')
                .subscribe((response) => {
                    this.weather = response;
                })
        );
        this._subscriptions.add(
            this._linkService.getMediaLinks()
                .subscribe((response) => {
                    this.mediaLinks = response;
                })
        );
        this._subscriptions.add(
            this._linkService.getSystemLinks()
                .subscribe((response) => {
                    this.systemLinks = response;
                })
        );
        this._subscriptions.add(
            this._linkService.getHomeAutomationLinks()
                .subscribe((response) => {
                    this.homeAutomationLinks = response;
                })
        );

        setInterval(() => {
            this.currentTime = new Date();
        }, 1000);
    }

    public getWeatherIcon(weatherDescription: string): string {

        if (weatherDescription.toUpperCase() === 'CLOUDS') {
            return 'fa fa-cloud';
        }

        if (weatherDescription.toUpperCase() === 'RAIN') {
            return 'fa fa-tint';
        }

        if (this.currentTime.getHours() < 6 && this.currentTime.getHours() > 18) {
            return 'fa fa-moon-o';
        }
        return 'fa fa-sun-o';
    }

    public getGreeting(): string {

        let greeting = 'Welcome';

        if (this.currentTime.getHours() < 12) {
            greeting = 'Good Morning';
        }
        if (this.currentTime.getHours() > 12) {
            greeting = 'Good Afternoon';
        }
        if (this.currentTime.getHours() > 18) {
            greeting = 'Good Evening';
        }

        return greeting;
    }

    public roundWeatherTemperature(temperature: number): string {
        return `${Math.round(temperature)}℃`;
    }

    public titleCase(input: string): string {
        var splitStr = input.toLowerCase().split(' ');
        for (var i = 0; i < splitStr.length; i++) {
            splitStr[i] = splitStr[i].charAt(0).toUpperCase() + splitStr[i].substring(1);
        }
        return splitStr.join(' ');
    }

    public refreshWeather(): void {
        this.isCheckingWeather = true;

        this._weatherService.getLiveWeather('52.824162', '-1.6396672')
            .subscribe((response) => {
                this.weather = response;
                this.isCheckingWeather = false;
            })
    }

    public ngOnDestroy(): void {
        this._subscriptions.unsubscribe();
    }
}
