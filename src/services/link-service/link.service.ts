import { Injectable } from '@angular/core';
import { ILink } from './types/link.type';
import { Observable } from 'rxjs';
import { LinkRepository } from './link.repository';
import { IColumn } from './types/column.type';
import { IFolder } from './types/folder.type';

const ONE_YEAR_IN_MILLISECONDS = 365 * 24 * 60 * 60 * 1000;

@Injectable()
export class LinkService {

    private _linkRepository: LinkRepository;

    constructor(linkRepository: LinkRepository) {
        this._linkRepository = linkRepository;
    }

    public getUpdatedLinks(): Observable<Array<ILink>> {
        return this._linkRepository.getAllLinks();
    }

    public getUpdatedColumns(): Observable<Array<IColumn>> {
        return this._linkRepository.getAllColumns();
    }

    public getAllColumns(): Observable<Array<IColumn>> {
        return this.getUpdatedColumns();
    }

    public getAllLinks(): Observable<Array<ILink>> {
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

    public recordLinkClick(identifier: string): Observable<ILink> {
        return this._linkRepository.recordLinkClick(identifier);
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

    public getLastClickedLabel(lastClickedAt: string | null | undefined): string {
        if (!lastClickedAt) {
            return 'never';
        }

        const timestamp = Date.parse(lastClickedAt);
        if (Number.isNaN(timestamp)) {
            return 'never';
        }

        const elapsedHours = Math.floor(Math.max(0, Date.now() - timestamp) / (1000 * 60 * 60));
        if (elapsedHours < 24) {
            return `${elapsedHours} hour${elapsedHours === 1 ? '' : 's'} ago`;
        }

        const elapsedDays = Math.floor(elapsedHours / 24);
        return `${elapsedDays} day${elapsedDays === 1 ? '' : 's'} ago`;
    }

    public isLastClickedHighlighted(lastClickedAt: string | null | undefined): boolean {
        if (!lastClickedAt) {
            return false;
        }

        const timestamp = Date.parse(lastClickedAt);
        return !Number.isNaN(timestamp) && Date.now() - timestamp >= ONE_YEAR_IN_MILLISECONDS;
    }

}
