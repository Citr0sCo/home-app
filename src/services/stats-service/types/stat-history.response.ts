export interface IStatHistoryResponse {
    hasError?: boolean;
    from: string;
    to: string;
    samples: Array<IStatHistorySample>;
}

export interface IStatHistorySample {
    recordedAt: string;
    cpuPercentage: number;
    memoryPercentage: number;
    memoryUsed: number;
    memoryTotal: number;
    diskPercentage: number;
    diskUsed: number;
    diskTotal: number;
}
