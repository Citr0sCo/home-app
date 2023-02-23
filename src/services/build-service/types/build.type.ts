export interface IBuild {
    identifier: string;
    status: string;
    conclusion: string;
    startedAt: Date;
    finishedAt: Date | null;
    githubBuildReference: string;
}