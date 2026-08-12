import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { QBitTorrentMapper } from './qbittorrent.mapper';
import { IQBitTorrentStats } from './types/qbittorrent-stats.type';

@Injectable()
export class QBitTorrentRepository {

    private readonly _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public getStats(identifier: string): Observable<IQBitTorrentStats> {
        return this._httpClient.get(`${environment.apiBaseUrl}/api/qbittorrent/stats?identifier=${identifier}`)
            .pipe(map((response: any) => QBitTorrentMapper.mapStats(response)));
    }
}
