import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PlexRepository } from './plex.repository';
import { IPlexSession } from './types/plex-session.type';

@Injectable()
export class PlexService {

    private _repository: PlexRepository;

    constructor(repository: PlexRepository) {
        this._repository = repository;
    }

    public getActivity(): Observable<Array<IPlexSession>> {
        return this._repository.getActivity();
    }

}