import { ILidarrActivity } from './types/lidarr-activity.type';

export class LidarrMapper {

    public static mapActivity(payload: any): ILidarrActivity {
        return {
            totalNumberOfTracks: payload.TotalNumberOfTracks,
            totalNumberOfQueuedTracks: payload.TotalNumberOfQueuedTracks,
            totalMissingTracks: payload.TotalMissingTracks,
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