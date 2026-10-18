import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable, switchMap } from 'rxjs';
import { LinkMapper } from './link.mapper';
import { environment } from '../../environments/environment';
import { ILink } from './types/link.type';
import { mapNetworkError } from '../../core/map-network-error';
import { ColumnMapper } from './column.mapper';
import { IColumn } from './types/column.type';
import { FolderMapper } from './folder.mapper';
import { IFolder } from './types/folder.type';

@Injectable()
export class LinkRepository {

    private _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public getAllLinks(): Observable<Array<ILink>> {
        return this._httpClient.get(`${environment.apiBaseUrl}/api/links`)
            .pipe(
                mapNetworkError(),
                map((response: any) => LinkMapper.map(response.Links)),
                switchMap((links) => this.withLastAccesses(links))
            );
    }

    public getAllColumns(): Observable<Array<IColumn>> {
        return this._httpClient.get(`${environment.apiBaseUrl}/api/columns`)
            .pipe(
                mapNetworkError(),
                map((response: any) => ColumnMapper.map(response.Columns)),
                switchMap((columns) => this.getLastAccesses().pipe(
                    map((accesses) => columns.map((column) => ({
                        ...column,
                        links: this.withAccesses(column.links, accesses),
                        folders: column.folders.map((folder) => ({
                            ...folder,
                            links: this.withAccesses(folder.links, accesses)
                        }))
                    })))
                ))
            );
    }

    public addLink(link: ILink): Observable<any> {
        return this._httpClient.post(`${environment.apiBaseUrl}/api/links`, { Link: LinkMapper.mapToApiSingle(link) })
            .pipe(
                mapNetworkError(),
                map((response: any) => {
                    return LinkMapper.mapSingle(response.Link);
                })
            );
    }

    public importColumns(columns: Array<IColumn>): Observable<Array<IColumn>> {
        return this._httpClient.post(`${environment.apiBaseUrl}/api/columns/import`, { Columns: ColumnMapper.mapToApi(columns) })
            .pipe(
                mapNetworkError(),
                switchMap(() => this.getAllColumns())
            );
    }

    public importLinks(links: Array<ILink>): Observable<Array<ILink>> {
        return this._httpClient.post(`${environment.apiBaseUrl}/api/links/import`, { Links: LinkMapper.mapToApi(links) })
            .pipe(
                mapNetworkError(),
                switchMap(() => this.getAllLinks())
            );
    }

    public updateLink(link: ILink): Observable<any> {
        return this._httpClient.patch(`${environment.apiBaseUrl}/api/links/${link.identifier}`, { Link: LinkMapper.mapToApiSingle(link) })
            .pipe(
                mapNetworkError(),
                map((response: any) => {
                    return LinkMapper.mapSingle(response.Link);
                })
            );
    }

    public recordLinkClick(identifier: string): Observable<void> {
        return this._httpClient.post(`${environment.apiBaseUrl}/api/links/${identifier}/click`, {})
            .pipe(
                mapNetworkError(),
                map(() => undefined)
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

    public createColumn(column: IColumn): Observable<IColumn> {
        return this._httpClient.post(`${environment.apiBaseUrl}/api/columns`, { Column: column })
            .pipe(
                mapNetworkError(),
                map((response: any) => ColumnMapper.mapSingle(response.Column))
            );
    }

    public updateColumn(column: IColumn): Observable<IColumn> {
        return this._httpClient.patch(`${environment.apiBaseUrl}/api/columns/${column.identifier}`, { Column: column })
            .pipe(
                mapNetworkError(),
                map((response: any) => ColumnMapper.mapSingle(response.Column))
            );
    }

    public deleteColumn(identifier: string): Observable<any> {
        return this._httpClient.delete(`${environment.apiBaseUrl}/api/columns/${identifier}`)
            .pipe(
                mapNetworkError()
            );
    }

    public createFolder(folder: IFolder): Observable<any> {
        return this._httpClient.post(`${environment.apiBaseUrl}/api/folders`, { Folder: FolderMapper.mapToApiSingle(folder) })
            .pipe(
                mapNetworkError(),
                map((response: any) => {
                    return FolderMapper.mapSingle(response.Folder);
                })
            );
    }

    public updateFolder(folder: IFolder): Observable<any> {
        return this._httpClient.patch(`${environment.apiBaseUrl}/api/folders/${folder.identifier}`, { Folder: FolderMapper.mapToApiSingle(folder) })
            .pipe(
                mapNetworkError()
            );
    }

    public deleteFolder(identifier: string): Observable<any> {
        return this._httpClient.delete(`${environment.apiBaseUrl}/api/folders/${identifier}`)
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

    private getLastAccesses(): Observable<Map<string, string>> {
        return this._httpClient.get(`${environment.apiBaseUrl}/api/links/last-accessed`)
            .pipe(
                mapNetworkError(),
                map((response: any) => new Map<string, string>(
                    (response.Links ?? []).map((link: any): [string, string] => [
                        link.Identifier.toLowerCase(),
                        link.LastAccessedAt
                    ])
                ))
            );
    }

    private withLastAccesses(links: Array<ILink>): Observable<Array<ILink>> {
        return this.getLastAccesses().pipe(map((accesses) => this.withAccesses(links, accesses)));
    }

    private withAccesses(links: Array<ILink>, accesses: Map<string, string>): Array<ILink> {
        return links.map((link) => ({
            ...link,
            lastClickedAt: link.identifier ? accesses.get(link.identifier.toLowerCase()) ?? null : null
        }));
    }
}