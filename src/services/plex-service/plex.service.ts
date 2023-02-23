import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PlexRepository } from './plex.repository';
import { IPlexSession } from './types/plex-session.type';

@Injectable()
export class PlexService {

    private _statRepository: PlexRepository;

    constructor(deployRepository: PlexRepository) {
        this._statRepository = deployRepository;
    }

    public getActivity(): Observable<Array<IPlexSession>> {
        return this._statRepository.getActivity();
    }

}