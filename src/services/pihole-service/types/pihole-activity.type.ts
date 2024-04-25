export interface IPiholeActivity {
    identifier: string;
    queriesToday: number;
    blockedToday: number;
    blockedPercentage: number;
    clients: number;
}