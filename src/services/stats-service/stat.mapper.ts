import { IStatResponse } from './types/stat.response';

export class StatMapper {

    public static map(response: any): IStatResponse {
        return {
            cpuUsage: {
                percentage: response.CpuUsage.Percentage,
                total: response.CpuUsage.Total,
                used: response.CpuUsage.Used
            },
            memoryUsage: {
                percentage: response.MemoryUsage.Percentage,
                total: response.MemoryUsage.Total,
                used: response.MemoryUsage.Used
            },
            diskUsage: {
                percentage: response.DiskUsage.Percentage,
                total: response.DiskUsage.Total,
                used: response.DiskUsage.Used
            }
        };
    }
}