import { Injectable } from '@angular/core';
import { ILink } from './types/link.type';
import { Observable, of, Subject, tap } from 'rxjs';
import { LinkRepository } from './link.repository';
import { IColumn } from './types/column.type';

@Injectable()
export class LinkService {

    private _linkRepository: LinkRepository;
    private _cachedLinks: Array<ILink> | null = null;
    private _cachedColumns: Array<IColumn> | null = null;

    constructor(linkRepository: LinkRepository) {
        this._linkRepository = linkRepository;
    }

    public getUpdatedLinks(): Observable<Array<ILink>> {
        return this._linkRepository.getAllLinks()
            .pipe(
                tap((links) => {
                    this._cachedLinks = links;
                    localStorage.setItem('cachedLinks', JSON.stringify(links));
                })
            );
    }

    public getUpdatedColumns(): Observable<Array<IColumn>> {
        return this._linkRepository.getAllColumns()
            .pipe(
                tap((columns) => {
                    this._cachedColumns = columns;
                    localStorage.setItem('cachedColumns', JSON.stringify(columns));
                })
            );
    }

    public getAllColumns(): Observable<Array<IColumn>> {

        if (localStorage.getItem('cachedColumns')) {
            this._cachedColumns = JSON.parse(`${localStorage.getItem('cachedColumns')}`);
        }

        if (this._cachedColumns !== null) {
            return of(this._cachedColumns);
        }

        return this.getUpdatedColumns();
    }

    public getAllLinks(): Observable<Array<ILink>> {

        if (localStorage.getItem('cachedLinks')) {
            this._cachedLinks = JSON.parse(`${localStorage.getItem('cachedLinks')}`);
        }

        if (this._cachedLinks !== null) {
            return of(this._cachedLinks);
        }

        return this.getUpdatedLinks();
    }

    public addLink(link: ILink): Observable<ILink> {
        return this._linkRepository.addLink(link);
    }

    public importColumns(columns: Array<IColumn>): Observable<Array<IColumn>> {
        return this._linkRepository.importColumns(columns);
    }

    public importLinks(links: Array<ILink>): Observable<Array<ILink>> {
        return this._linkRepository.importLinks(links);
    }

    public updateLink(link: ILink): Observable<ILink> {
        return this._linkRepository.updateLink(link);
    }

    public deleteLink(identifier: string): Observable<any> {
        return this._linkRepository.deleteLink(identifier);
    }

    public uploadLogo(identifier: string, data: FormData): Observable<string> {
        return this._linkRepository.uploadLogo(identifier, data);
    }

    public createColumn(column: IColumn): Observable<void> {
        return this._linkRepository.createColumn(column);
    }

    public updateColumn(column: IColumn): Observable<void> {
        return this._linkRepository.updateColumn(column);
    }

    public deleteColumn(identifier: string): Observable<void> {
        return this._linkRepository.deleteColumn(identifier);
    }

    public refreshCache(): Observable<void> {
        return this._linkRepository.refreshCache();
    }
}
