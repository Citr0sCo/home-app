import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { ConfigsMapper } from './configs.mapper';
import { environment } from '../../environments/environment';
import { mapNetworkError } from '../../core/map-network-error';
import { IConfigs } from './types/configs.type';
import { ISetting } from './types/setting.type';

@Injectable()
export class ConfigsRepository {

    private _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public getAllConfigs(): Observable<IConfigs> {
        return this._httpClient.get(`${environment.apiBaseUrl}/api/configs`)
            .pipe(
                mapNetworkError(),
                map((response: any) => {
                    return ConfigsMapper.map(response);
                })
            );
    }

    public getAllSettings(): Observable<Array<ISetting>> {
        return this._httpClient.get(`${environment.apiBaseUrl}/api/settings`)
            .pipe(
                mapNetworkError(),
                map((response: any) => response.Settings.map((setting: any) => ConfigsMapper.mapSetting(setting)))
            );
    }

    public updateSettings(settings: Array<ISetting>): Observable<Array<ISetting>> {
        return this._httpClient.put(`${environment.apiBaseUrl}/api/settings`, {
            Settings: settings.map((setting) => ({ Key: setting.key, Value: setting.value }))
        })
            .pipe(
                mapNetworkError(),
                map((response: any) => response.Settings.map((setting: any) => ConfigsMapper.mapSetting(setting)))
            );
    }
}