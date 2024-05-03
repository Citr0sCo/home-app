import { IRadarrActivity } from './types/radarr-activity.type';

export class RadarrMapper {

    public static mapActivity(payload: any): IRadarrActivity {
        return {
            totalNumberOfMovies: payload.TotalNumberOfMovies,
            totalNumberOfQueuedMovies: payload.TotalNumberOfQueuedMovies,
            totalMissingMovies: payload.TotalMissingMovies
        };
    }

    public static mapWebsocketActivities(payload: any): IRadarrActivity {
        return this.mapActivity(payload.Response.Data);
    }
}