import { IPlexSession } from './types/plex-session.type';

export class PlexMapper {

    public static mapActivity(payload: any): Array<IPlexSession> {
        return payload.response.data.sessions.map((session: any) => {
            return {
                user: session.user,
                duration: session.duration,
                fullTitle: session.fullTitle,
                state: session.state,
                viewOffset: session.viewOffset,
                progressPercentage: session.progressPercentage,
                videoTranscodeDecision: session.videoDecision
            };
        });
    }
}