import { IBuild } from './types/build.type';

export class BuildMapper {

    public static map(payload: any): Array<IBuild> {
        return payload.builds.map((build: any) => this.mapSingle(build));
    }

    public static mapSingle(build: any): IBuild {
        return {
            identifier: build.identifier,
            status: build.status,
            conclusion: build.conclusion,
            startedAt: new Date(build.startedAt),
            finishedAt: build.finishedAt !== null ? new Date(build.finishedAt) : null,
            githubBuildReference: build.githubBuildReference
        };
    }
}