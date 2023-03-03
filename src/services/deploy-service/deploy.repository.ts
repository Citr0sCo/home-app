import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IDeploy } from './types/deploy.type';
import { map, Observable, of } from 'rxjs';
import { environment } from '../../environments/environment';
import { DeployMapper } from './deploy.mapper';

@Injectable()
export class DeployRepository {

    private _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public getAll(): Observable<Array<IDeploy>> {
        return this._httpClient.get(`${environment.apiBaseUrl}/api/deploys`)
            .pipe(
                map((response: any) => {
                    return DeployMapper.map(response);
                })
            );
    }

}