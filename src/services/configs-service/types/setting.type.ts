export interface ISetting {
    key: string;
    environmentVariable: string;
    label: string;
    description: string;
    value: string;
    isSecret: boolean;
    isConfigured: boolean;
}
