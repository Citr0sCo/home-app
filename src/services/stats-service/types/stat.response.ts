import { IStat } from './stat.type';

export interface IStatResponse {
    cpuUsage: IStat;
    memoryUsage: IStat;
    diskUsage: IStat;

}