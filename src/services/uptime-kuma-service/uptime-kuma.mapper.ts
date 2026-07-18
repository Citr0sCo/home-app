import { IUptimeKumaActivity } from './types/uptime-kuma-activity.type';

export class UptimeKumaMapper {

    public static mapActivity(payload: any): IUptimeKumaActivity {
        return {
            metrics: payload.Metrics.map((metric: any) => {
                return {
                    name: metric.Name,
                    isUp: metric.IsUp
                };
            })
        };
    }

    public static mapActivities(payload: any): Array<IUptimeKumaActivity> {
        return payload.Response.Data.Activities.map(this.mapActivity);
    }
}