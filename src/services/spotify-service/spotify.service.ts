import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SpotifyRepository } from './spotify.repository';
import { ISpotifyResponse } from './types/spotify-response.type';

@Injectable()
export class SpotifyService {

    private _repository: SpotifyRepository;

    constructor(repository: SpotifyRepository) {
        this._repository = repository;
    }

    public execute(code: string): Observable<ISpotifyResponse> {
        return this._repository.execute(code);
    }

}