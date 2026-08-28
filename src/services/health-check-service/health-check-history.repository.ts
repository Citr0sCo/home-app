import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { mapNetworkError } from '../../core/map-network-error';
import {
    IHealthCheckHistoryResponse,
    IHealthCheckLinkHistory
} from './types/health-check-history.response';

@Injectable()
export class HealthCheckHistoryRepository {
    private readonly _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public getHistory(days: number = 7): Observable<IHealthCheckHistoryResponse> {
        return this._httpClient
            .get(`${environment.apiBaseUrl}/api/healthcheck/history?days=${days}`)
            .pipe(
                mapNetworkError(),
                map((response: any) => ({
                    hasError: response.HasError ?? false,
                    from: response.From,
                    to: response.To,
                    links: Array.isArray(response.Links)
                        ? response.Links.map((link: any): IHealthCheckLinkHistory => ({
                            identifier: link.Identifier,
                            name: link.Name,
                            url: link.Url,
                            samples: Array.isArray(link.Samples)
                                ? link.Samples.map((sample: any) => ({
                                    recordedAt: sample.RecordedAt,
                                    durationInMilliseconds: sample.DurationInMilliseconds,
                                    statusCode: sample.StatusCode,
                                    statusDescription: sample.StatusDescription
                                }))
                                : []
                        }))
                        : []
                }))
            );
    }
}
