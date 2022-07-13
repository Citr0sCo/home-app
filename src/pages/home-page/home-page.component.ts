import {Component, OnDestroy, OnInit} from '@angular/core';
import {WeatherService} from "../../services/weather-service/weather.service";
import {Subscription} from "rxjs";
import {IWeatherData} from "../../services/weather-service/types/weather-data.type";
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

    private _currentTime: Date = new Date();
    private _weather: IWeatherData | null = null;
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
                    this._weather = response;
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
    }

    public getWeatherIcon(): string {
        if (this._currentTime.getHours() < 6 && this._currentTime.getHours() > 18) {
            return 'fa fa-moon-o';
        }
        return 'fa fa-sun-o';
    }

    public getWeatherInfo(): string {

        if (this._weather) {
            return `${this._weather.name}, ${Math.round(this._weather.temperature)}℃`;
        }

        return 'No weather data available.';

    }

    public getGreeting(): string {

        let greeting = 'Welcome';

        if (this._currentTime.getHours() < 12) {
            greeting = 'Good Morning';
        }
        if (this._currentTime.getHours() > 12) {
            greeting = 'Good Afternoon';
        }
        if (this._currentTime.getHours() > 18) {
            greeting = 'Good Evening';
        }

        return greeting;
    }

    public getCurrentTime(): string {

        let hours = this._currentTime.getHours().toString();

        if (parseInt(hours) < 10) {
            hours = `0${hours}`;
        }

        let minutes = this._currentTime.getMinutes().toString();

        if (parseInt(minutes) < 10) {
            minutes = `0${minutes}`;
        }

        return `${hours}:${minutes}`;
    }

    public getCurrentDate(): string {

        let dayPostfix = 'th';

        if (this._currentTime.getDate() === 1) {
            dayPostfix = 'st';
        } else if (this._currentTime.getDate() === 2) {
            dayPostfix = 'nd';
        } else if (this._currentTime.getDate() === 2) {
            dayPostfix = 'rd';
        }

        return `${this.weekdays[this._currentTime.getDay() - 1]}, ${this._currentTime.getDate()}${dayPostfix} ${this.months[this._currentTime.getMonth()]} ${this._currentTime.getFullYear()}`;
    }

    public weekdays: string[] = [
        'Monday',
        'Tuesday',
        'Wednesday',
        'Thursday',
        'Friday',
        'Saturday',
        'Sunday'
    ];

    public months: string[] = [
        'Jan',
        'Feb',
        'Mar',
        'Apr',
        'May',
        'Jun',
        'Jul',
        'Aug',
        'Sep',
        'Oct',
        'Nov',
        'Dec'
    ];

    public ngOnDestroy(): void {
        this._subscriptions.unsubscribe();
    }
}
