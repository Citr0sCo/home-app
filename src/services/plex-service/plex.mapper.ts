import { IPlexSession } from './types/plex-session.type';

export class PlexMapper {

    public static mapActivity(payload: any): Array<IPlexSession> {
        return payload.Response.Data.Sessions.map((session: any) => {
            const duration = this.toNumberOrNull(session.Duration);
            const viewOffset = this.toNumberOrNull(session.ViewOffset);
            const isLiveTv = session.Live !== false && duration === null && viewOffset === null;
            const serverProgress = this.toNumberOrNull(session.ProgressPercentage) ?? 0;
            const progressPercentage = isLiveTv
                ? 100
                : duration !== null && duration > 0 && viewOffset !== null
                    ? this.toPercentage(viewOffset / duration * 100)
                    : this.toPercentage(serverProgress);

            return {
                user: session.User,
                duration,
                fullTitle: session.FullTitle,
                state: String(session.State ?? '').trim().toLowerCase(),
                viewOffset,
                progressPercentage,
                videoTranscodeDecision: session.VideoDecision,
                isLiveTv
            };
        });
    }

    private static toNumberOrNull(value: unknown): number | null {
        if (value === null || value === undefined || value === '') {
            return null;
        }

        const number = Number(value);
        return Number.isFinite(number) ? number : null;
    }

    private static toPercentage(value: number): number {
        return Math.min(Math.max(value, 0), 100);
    }
}