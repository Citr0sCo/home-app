import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable, of } from 'rxjs';
import { IWeatherData } from './types/weather-data.type';
import { WeatherMapper } from './weather.mapper';

@Injectable()
export class WeatherService {

    private _apiKey: string = 'e270be42482a59c87d5587db1e283dbc';

    private _httpClient: HttpClient;
    private _cachedWeather: IWeatherData | null = null;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public getWeatherFor(latitude: number | null, longitude: number | null): Observable<IWeatherData | null> {

        if (localStorage.getItem('cachedWeather')) {
            this._cachedWeather = JSON.parse(`${localStorage.getItem('cachedWeather')}`);
        }

        if (this._cachedWeather !== null) {
            return of(this._cachedWeather);
        }

        return this.getLiveWeather(latitude, longitude);
    }

    public getLiveWeather(latitude: number | null, longitude: number | null): Observable<IWeatherData | null> {

        if (this._cachedWeather !== null) {
            const differenceInTime = new Date().getTime() - new Date(this._cachedWeather.timestamp).getTime();

            const hourInMilliseconds = 3600 * 1000;

            if (differenceInTime < hourInMilliseconds) {
                return of(this._cachedWeather);
            }
        }

        if(latitude === 0 && longitude === 0) {
            return of(null);
        }

        return this._httpClient.get(`https://api.openweathermap.org/data/2.5/weather?lat=${latitude}&lon=${longitude}&appid=${this._apiKey}&units=metric`)
            .pipe(map((response) => {
                localStorage.setItem('cachedWeather', JSON.stringify(WeatherMapper.map(response)));
                this._cachedWeather = WeatherMapper.map(response);
                return WeatherMapper.map(response);
            }));
    }

}