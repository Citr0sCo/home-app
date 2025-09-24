import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { LinkMapper } from './link.mapper';
import { environment } from '../../environments/environment';
import { ILink } from './types/link.type';
import { mapNetworkError } from '../../core/map-network-error';
import { ColumnMapper } from './column.mapper';
import { IColumn } from './types/column.type';

@Injectable()
export class LinkRepository {

    private _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public getAllLinks(): Observable<any> {
        return this._httpClient.get(`${environment.apiBaseUrl}/api/links`)
            .pipe(
                mapNetworkError(),
                map((response: any) => {
                    return LinkMapper.map(response.Links);
                })
            );
    }

    public getAllColumns(): Observable<any> {
        return this._httpClient.get(`${environment.apiBaseUrl}/api/columns`)
            .pipe(
                mapNetworkError(),
                map((response: any) => {
                    return ColumnMapper.map(response.Columns);
                })
            );
    }

    public addLink(link: ILink): Observable<any> {
        return this._httpClient.post(`${environment.apiBaseUrl}/api/links`, { Link: link })
            .pipe(
                mapNetworkError(),
                map((response: any) => {
                    return LinkMapper.mapSingle(response.Link);
                })
            );
    }

    public importColumns(columns: Array<IColumn>): Observable<any> {
        return this._httpClient.post(`${environment.apiBaseUrl}/api/columns/import`, { Columns: ColumnMapper.mapToApi(columns) })
            .pipe(
                mapNetworkError(),
                map((response: any) => {
                    return ColumnMapper.map(response.Columns);
                })
            );
    }

    public importLinks(links: Array<ILink>): Observable<any> {
        return this._httpClient.post(`${environment.apiBaseUrl}/api/links/import`, { Links: LinkMapper.mapToApi(links) })
            .pipe(
                mapNetworkError(),
                map((response: any) => {
                    return LinkMapper.map(response.Links);
                })
            );
    }

    public updateLink(link: ILink): Observable<any> {
        return this._httpClient.patch(`${environment.apiBaseUrl}/api/links/${link.identifier}`, { Link: link })
            .pipe(
                mapNetworkError(),
                map((response: any) => {
                    return LinkMapper.mapSingle(response.Link);
                })
            );
    }

    public deleteLink(identifier: string): Observable<any> {
        return this._httpClient.delete(`${environment.apiBaseUrl}/api/links/${identifier}`)
            .pipe(
                mapNetworkError()
            );
    }

    public uploadLogo(identifier: string, data: FormData): Observable<string> {
        return this._httpClient.post(`${environment.apiBaseUrl}/api/links/${identifier}/logo`, data)
            .pipe(
                mapNetworkError(),
                map((response: any) => {
                    return response.IconUrl;
                })
            );
    }

    public createColumn(column: IColumn): Observable<any> {
        return this._httpClient.post(`${environment.apiBaseUrl}/api/columns`, { Column: column })
            .pipe(
                mapNetworkError()
            );
    }

    public updateColumn(column: IColumn): Observable<any> {
        return this._httpClient.patch(`${environment.apiBaseUrl}/api/columns/${column.identifier}`, { Column: column })
            .pipe(
                mapNetworkError()
            );
    }

    public deleteColumn(identifier: string): Observable<any> {
        return this._httpClient.delete(`${environment.apiBaseUrl}/api/columns/${identifier}`)
            .pipe(
                mapNetworkError()
            );
    }

    public refreshCache(): Observable<any> {
        return this._httpClient.delete(`${environment.apiBaseUrl}/api/files/cache`, {})
            .pipe(
                mapNetworkError()
            );
    }
}