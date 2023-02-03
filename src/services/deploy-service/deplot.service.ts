import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { IDeploy } from './types/deploy.type';
import { Observable, of } from 'rxjs';

@Injectable()
export class DeployService {

    private _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public getAll(): Observable<Array<IDeploy>> {
        return of([]);
    }

}