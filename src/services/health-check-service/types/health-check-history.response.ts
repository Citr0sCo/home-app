export interface IHealthCheckHistoryResponse {
    hasError?: boolean;
    from: string;
    to: string;
    links: Array<IHealthCheckLinkHistory>;
}

export interface IHealthCheckLinkHistory {
    identifier: string;
    name: string;
    url: string;
    samples: Array<IHealthCheckHistorySample>;
}

export interface IHealthCheckHistorySample {
    recordedAt: string;
    durationInMilliseconds: number;
    statusCode: number;
    statusDescription: string | null;
}
