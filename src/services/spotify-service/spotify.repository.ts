import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { SpotifyMapper } from './spotify.mapper';
import { environment } from '../../environments/environment';
import { ISpotifyResponse } from './types/spotify-response.type';

@Injectable()
export class SpotifyRepository {

    private _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public execute(code: string): Observable<ISpotifyResponse> {
        return this._httpClient.post(`${environment.apiBaseUrl}/api/spotify/test`, { Code: code })
            .pipe(
                map((response: any) => {
                    return SpotifyMapper.mapResponse(response);
                })
            );
    }

}