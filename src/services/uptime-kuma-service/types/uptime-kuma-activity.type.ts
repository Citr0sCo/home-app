export interface IUptimeKumaActivity {
    metrics: Array<IUptimeKumaMetric>;
}

export interface IUptimeKumaMetric {
    name: string;
    isUp: boolean;
}