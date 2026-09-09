import { Injectable } from '@angular/core';
import { ILink } from './types/link.type';
import { Observable, of, tap } from 'rxjs';
import { LinkRepository } from './link.repository';
import { IColumn } from './types/column.type';
import { IFolder } from './types/folder.type';

const COLUMNS_CACHE_KEY = 'cachedColumns';
const LINKS_CACHE_KEY = 'cachedLinks';
const ONE_WEEK_IN_MILLISECONDS = 7 * 24 * 60 * 60 * 1000;
const ONE_MONTH_IN_MILLISECONDS = 30 * 24 * 60 * 60 * 1000;

@Injectable()
export class LinkService {

    private _linkRepository: LinkRepository;

    constructor(linkRepository: LinkRepository) {
        this._linkRepository = linkRepository;
    }

    public getUpdatedLinks(): Observable<Array<ILink>> {
        return this._linkRepository.getAllLinks()
            .pipe(tap((links) => this.cacheLinks(links)));
    }

    public getUpdatedColumns(): Observable<Array<IColumn>> {
        return this._linkRepository.getAllColumns()
            .pipe(tap((columns) => this.cacheColumns(columns)));
    }

    public getAllColumns(): Observable<Array<IColumn>> {
        const columns = this.readCache<Array<IColumn>>(COLUMNS_CACHE_KEY, []);
        return of(columns.map((column) => ({
            ...column,
            links: column.links ?? [],
            folders: (column.folders ?? []).map((folder) => ({
                ...folder,
                links: folder.links ?? []
            }))
        })));
    }

    public getAllLinks(): Observable<Array<ILink>> {
        return of(this.readCache<Array<ILink>>(LINKS_CACHE_KEY, []));
    }

    public cacheColumns(columns: Array<IColumn>): void {
        localStorage.setItem(COLUMNS_CACHE_KEY, JSON.stringify(columns));
    }

    public addLink(link: ILink): Observable<ILink> {
        return this._linkRepository.addLink(link);
    }

    public importColumns(columns: Array<IColumn>): Observable<Array<IColumn>> {
        return this._linkRepository.importColumns(columns)
            .pipe(tap((updatedColumns) => this.cacheColumns(updatedColumns)));
    }

    public importLinks(links: Array<ILink>): Observable<Array<ILink>> {
        return this._linkRepository.importLinks(links)
            .pipe(tap((updatedLinks) => this.cacheLinks(updatedLinks)));
    }

    public updateLink(link: ILink): Observable<ILink> {
        return this._linkRepository.updateLink(link);
    }

    public deleteLink(identifier: string): Observable<any> {
        return this._linkRepository.deleteLink(identifier);
    }

    public recordLinkClick(identifier: string): Observable<ILink> {
        return this._linkRepository.recordLinkClick(identifier);
    }

    public uploadLogo(identifier: string, data: FormData): Observable<string> {
        return this._linkRepository.uploadLogo(identifier, data);
    }

    public createColumn(column: IColumn): Observable<IColumn> {
        return this._linkRepository.createColumn(column);
    }

    public updateColumn(column: IColumn): Observable<IColumn> {
        return this._linkRepository.updateColumn(column);
    }

    public deleteColumn(identifier: string): Observable<void> {
        return this._linkRepository.deleteColumn(identifier);
    }

    public createFolder(folder: IFolder): Observable<IFolder> {
        return this._linkRepository.createFolder(folder);
    }

    public updateFolder(folder: IFolder): Observable<void> {
        return this._linkRepository.updateFolder(folder);
    }

    public deleteFolder(identifier: string): Observable<void> {
        return this._linkRepository.deleteFolder(identifier);
    }

    public refreshCache(): Observable<void> {
        return this._linkRepository.refreshCache();
    }

    public getLastClickedStatus(lastClickedAt: string | null | undefined): 'never' | 'recent' | 'week' | 'month' {
        if (!lastClickedAt) {
            return 'never';
        }

        const timestamp = this.parseTimestamp(lastClickedAt);
        if (timestamp === null) {
            return 'never';
        }

        const elapsed = Math.max(0, Date.now() - timestamp);
        if (elapsed >= ONE_MONTH_IN_MILLISECONDS) {
            return 'month';
        }

        if (elapsed >= ONE_WEEK_IN_MILLISECONDS) {
            return 'week';
        }

        return 'recent';
    }

    private parseTimestamp(value: string): number | null {
        const normalizedValue = /(?:Z|[+-]\d{2}:?\d{2})$/i.test(value)
            ? value
            : `${value}Z`;
        const timestamp = Date.parse(normalizedValue);

        return Number.isNaN(timestamp) ? null : timestamp;
    }

    private cacheLinks(links: Array<ILink>): void {
        localStorage.setItem(LINKS_CACHE_KEY, JSON.stringify(links));
    }

    private readCache<T>(key: string, fallback: T): T {
        const cachedValue = localStorage.getItem(key);

        if (cachedValue === null) {
            return fallback;
        }

        try {
            return JSON.parse(cachedValue) as T;
        } catch {
            localStorage.removeItem(key);
            return fallback;
        }
    }

}
