import { IPiholeActivity } from './types/pihole-activity.type';

export class PiholeMapper {

    public static mapActivity(payload: any): IPiholeActivity {
        return {
            identifier: payload.Identifier,
            queriesToday: payload.QueriesToday,
            blockedToday: payload.BlockedToday,
            blockedPercentage: payload.BlockedPercentage,
            clients: payload.Clients
        };
    }

    public static mapActivities(payload: any): Array<IPiholeActivity> {
        return payload.Response.Data.Activities.map(this.mapActivity);
    }
}