export interface ILidarrActivity {
    totalNumberOfTracks: number;
    totalNumberOfQueuedTracks: number;
    totalMissingTracks: number;
    health: Array<ILidarrHealth>;
}

export interface ILidarrHealth {
    type: string;
    message: string;
    wikiUrl: string;
    source: string;
}