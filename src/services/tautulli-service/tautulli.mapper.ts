import { ITautulliStats } from './types/tautulli-stats.type';

export class TautulliMapper {

    public static mapStats(payload: any): ITautulliStats {
        return {
            identifier: payload.Identifier,
            totalMovies: payload.TotalMovies ?? 0,
            totalShows: payload.TotalShows ?? 0,
            totalUsers: payload.TotalUsers ?? 0
        };
    }

    public static mapActivities(payload: any): Array<ITautulliStats> {
        return payload.Response?.Data?.Activities?.map((activity: any) => this.mapStats(activity)) ?? [];
    }
}
