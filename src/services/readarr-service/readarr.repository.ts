import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { ReadarrMapper } from './readarr.mapper';
import { environment } from '../../environments/environment';
import { IReadarrActivity } from './types/readarr-activity.type';

@Injectable()
export class ReadarrRepository {

    private _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public getActivity(): Observable<IReadarrActivity> {
        return this._httpClient.get(`${environment.apiBaseUrl}/api/readarr/activity`)
            .pipe(
                map((response: any) => {
                    return ReadarrMapper.mapActivity(response);
                })
            );
    }

}