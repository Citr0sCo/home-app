import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { BuildMapper } from './build.mapper';
import { environment } from '../../environments/environment';
import { IBuild } from './types/build.type';

@Injectable()
export class BuildRepository {

    private _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public getAll(): Observable<Array<IBuild>> {
        return this._httpClient.get(`${environment.apiBaseUrl}/api/builds`)
            .pipe(
                map((response: any) => {
                    return BuildMapper.map(response);
                })
            );
    }

    public updateAllDockerApps(): Observable<void> {
        return this._httpClient.post(`${environment.apiBaseUrl}/api/builds/docker-apps`, {})
            .pipe(
                map((response: any) => {
                    //return BuildMapper.map(response);
                })
            );
    }

}