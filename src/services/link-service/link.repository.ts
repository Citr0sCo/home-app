import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { LinkMapper } from './link.mapper';
import { environment } from '../../environments/environment';
import { ILink } from './types/link.type';

@Injectable()
export class LinkRepository {

    private _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public getAllLinks(): Observable<any> {
        return this._httpClient.get(`${environment.apiBaseUrl}/api/links`)
            .pipe(
                map((response: any) => {
                    return LinkMapper.map(response.links);
                })
            );
    }

    public addLink(link: ILink): Observable<any> {
        return this._httpClient.post(`${environment.apiBaseUrl}/api/links`, { Link: link })
            .pipe(
                map((response: any) => {
                    return LinkMapper.mapSingle(response.link);
                })
            );
    }

    public deleteLink(identifier: string): Observable<any> {
        return this._httpClient.delete(`${environment.apiBaseUrl}/api/links/${identifier}`);
    }
}