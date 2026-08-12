import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { TautulliMapper } from './tautulli.mapper';
import { ITautulliStats } from './types/tautulli-stats.type';

@Injectable()
export class TautulliRepository {

    private readonly _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public getStats(identifier: string): Observable<ITautulliStats> {
        return this._httpClient.get(`${environment.apiBaseUrl}/api/tautulli/stats?identifier=${identifier}`)
            .pipe(map((response: any) => TautulliMapper.mapStats(response)));
    }
}
