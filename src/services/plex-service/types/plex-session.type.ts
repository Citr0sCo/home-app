export interface IPlexSession {
    user: string;
    fullTitle: string;
    state: string;
    viewOffset: number | null;
    duration: number | null;
    progressPercentage: number;
    videoTranscodeDecision: string;
    isLiveTv: boolean;
}