import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { RadarrMapper } from './radarr.mapper';
import { environment } from '../../environments/environment';
import { IRadarrActivity } from './types/radarr-activity.type';

@Injectable()
export class RadarrRepository {

    private _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public getActivity(identifier: string): Observable<IRadarrActivity> {
        return this._httpClient.get(`${environment.apiBaseUrl}/api/radarr/activity?identifier=${identifier}`)
            .pipe(
                map((response: any) => {
                    return RadarrMapper.mapActivity(response);
                })
            );
    }

}