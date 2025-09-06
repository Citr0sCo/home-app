import { ILidarrActivity } from './types/radarr-activity.type';

export class LidarrMapper {

    public static mapActivity(payload: any): ILidarrActivity {
        return {
            totalNumberOfMovies: payload.TotalNumberOfMovies,
            totalNumberOfQueuedMovies: payload.TotalNumberOfQueuedMovies,
            totalMissingMovies: payload.TotalMissingMovies,
            health: payload.Health.map((x: any) => {
                return {
                    type: x.Type,
                    message: x.Message,
                    wikiUrl: x.WwkiUrl,
                    source: x.Source
                };
            })
        };
    }

    public static mapWebsocketActivities(payload: any): ILidarrActivity {
        return this.mapActivity(payload.Response.Data);
    }
}