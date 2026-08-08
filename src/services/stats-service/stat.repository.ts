import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { StatMapper } from './stat.mapper';
import { IStatResponse } from './types/stat.response';
import { IStatHistoryResponse } from './types/stat-history.response';

@Injectable()
export class StatRepository {

    private _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public getAll(): Observable<IStatResponse> {
        return this._httpClient.get(`${environment.apiBaseUrl}/api/stats`)
            .pipe(
                map((response: any) => {
                    return StatMapper.map(response);
                })
            );
    }

    public getHistory(hours: number = 24): Observable<IStatHistoryResponse> {
        return this._httpClient.get(`${environment.apiBaseUrl}/api/stats/history?hours=${hours}`)
            .pipe(
                map((response: any) => ({
                    hasError: response.HasError ?? false,
                    from: response.From,
                    to: response.To,
                    samples: Array.isArray(response.Samples)
                        ? response.Samples.map((sample: any) => ({
                            recordedAt: sample.RecordedAt,
                            cpuPercentage: sample.CpuPercentage,
                            memoryPercentage: sample.MemoryPercentage,
                            memoryUsed: sample.MemoryUsed,
                            memoryTotal: sample.MemoryTotal,
                            diskPercentage: sample.DiskPercentage,
                            diskUsed: sample.DiskUsed,
                            diskTotal: sample.DiskTotal
                        }))
                        : []
                }))
            );
    }

}