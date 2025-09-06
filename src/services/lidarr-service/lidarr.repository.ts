import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { LidarrMapper } from './lidarr.mapper';
import { environment } from '../../environments/environment';
import { ILidarrActivity } from './types/radarr-activity.type';

@Injectable()
export class LidarrRepository {

    private _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public getActivity(): Observable<ILidarrActivity> {
        return this._httpClient.get(`${environment.apiBaseUrl}/api/lidarr/activity`)
            .pipe(
                map((response: any) => {
                    return LidarrMapper.mapActivity(response);
                })
            );
    }

}