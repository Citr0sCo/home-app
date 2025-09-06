export interface ILidarrActivity {
    totalNumberOfMovies: number;
    totalNumberOfQueuedMovies: number;
    totalMissingMovies: number;
    health: Array<ILidarrHealth>;
}

export interface ILidarrHealth {
    type: string;
    message: string;
    wikiUrl: string;
    source: string;
}