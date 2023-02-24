import { Injectable } from '@angular/core';
import { ILink } from './types/link.type';
import { Observable, of, tap } from 'rxjs';
import { LinkRepository } from './link.repository';

@Injectable()
export class LinkService {

    private _linkRepository: LinkRepository;
    private _cachedLinks: Array<ILink> = [];

    constructor(linkRepository: LinkRepository) {
        this._linkRepository = linkRepository;
    }

    public getAllLinks(): Observable<Array<ILink>> {
        return this._linkRepository.getAllLinks().pipe(
            tap((links) => {
                this._cachedLinks = links;
            })
        );
    }

    public addLink(link: ILink): Observable<ILink> {
        return this._linkRepository.addLink(link);
    }

    public deleteLink(identifier: string): Observable<any> {
        return this._linkRepository.deleteLink(identifier);
    }

    public getMediaLinks(): Observable<Array<ILink>> {
        return of(this._cachedLinks.filter((link) => link.category === 'media'));
    }

    public getSystemLinks(): Observable<Array<ILink>> {
        return of(this._cachedLinks.filter((link) => link.category === 'system'));
    }

    public getProductivityLinks(): Observable<Array<ILink>> {
        return of(this._cachedLinks.filter((link) => link.category === 'productivity'));
    }

    public getToolsLinks(): Observable<Array<ILink>> {
        return of(this._cachedLinks.filter((link) => link.category === 'tools'));
    }
}
