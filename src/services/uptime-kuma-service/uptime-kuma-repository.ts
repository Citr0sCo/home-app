import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { UptimeKumaMapper } from './uptime-kuma.mapper';
import { environment } from '../../environments/environment';
import { IUptimeKumaActivity } from './types/uptime-kuma-activity.type';

@Injectable()
export class UptimeKumaRepository {

    private _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public getActivity(identifier: string): Observable<IUptimeKumaActivity> {
        return this._httpClient.get(`${environment.apiBaseUrl}/api/uptimekuma/activity?identifier=${identifier}`)
            .pipe(
                map((response: any) => {
                    return UptimeKumaMapper.mapActivity(response);
                })
            );
    }

}