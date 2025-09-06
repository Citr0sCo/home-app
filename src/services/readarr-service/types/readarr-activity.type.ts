export interface IReadarrActivity {
    totalNumberOfBooks: number;
    totalNumberOfQueuedBooks: number;
    totalMissingBooks: number;
    health: Array<IReadarrHealth>;
}

export interface IReadarrHealth {
    type: string;
    message: string;
    wikiUrl: string;
    source: string;
}