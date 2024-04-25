import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { PiholeMapper } from './pihole.mapper';
import { environment } from '../../environments/environment';
import { IPiholeActivity } from './types/pihole-activity.type';

@Injectable()
export class PiholeRepository {

    private _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public getActivity(identifier: string): Observable<IPiholeActivity> {
        return this._httpClient.get(`${environment.apiBaseUrl}/api/pihole/activity?identifier=${identifier}`)
            .pipe(
                map((response: any) => {
                    return PiholeMapper.mapActivity(response);
                })
            );
    }

}