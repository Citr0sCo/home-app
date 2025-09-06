import { IReadarrActivity } from './types/readarr-activity.type';

export class ReadarrMapper {

    public static mapActivity(payload: any): IReadarrActivity {
        return {
            totalNumberOfBooks: payload.TotalNumberOfBooks,
            totalNumberOfQueuedBooks: payload.TotalNumberOfQueuedBooks,
            totalMissingBooks: payload.TotalMissingBooks,
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

    public static mapWebsocketActivities(payload: any): IReadarrActivity {
        return this.mapActivity(payload.Response.Data);
    }
}