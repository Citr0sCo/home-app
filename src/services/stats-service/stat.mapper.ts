import { IStatResponse } from './types/stat.response';

export class StatMapper {

    public static map(response: any): IStatResponse {
        return {
            cpuUsage: {
                percentage: response.cpuUsage.percentage,
                total: response.cpuUsage.total,
                used: response.cpuUsage.used
            },
            memoryUsage: {
                percentage: response.memoryUsage.percentage,
                total: response.memoryUsage.total,
                used: response.memoryUsage.used
            },
            diskUsage: {
                percentage: response.diskUsage.percentage,
                total: response.diskUsage.total,
                used: response.diskUsage.used
            }
        };
    }
}