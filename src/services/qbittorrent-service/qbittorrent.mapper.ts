import { IQBitTorrentStats } from './types/qbittorrent-stats.type';

export class QBitTorrentMapper {

    public static mapStats(payload: any): IQBitTorrentStats {
        return {
            identifier: payload.Identifier,
            totalTorrents: payload.TotalTorrents ?? 0,
            uploadRate: payload.UploadRate ?? 0,
            totalLeeches: payload.TotalLeeches ?? 0
        };
    }

    public static mapActivities(payload: any): Array<IQBitTorrentStats> {
        return payload.Response?.Data?.Activities?.map((activity: any) => this.mapStats(activity)) ?? [];
    }
}
