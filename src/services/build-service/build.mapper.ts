import { IBuild } from './types/build.type';

export class BuildMapper {

    public static map(payload: any): Array<IBuild> {
        return payload.builds.map((build: any) => {
            return {
                identifier: build.identifier,
                status: build.status,
                conclusion: build.conclusion,
                startedAt: build.startedAt,
                finishedAt: build.finishedAt,
                githubBuildReference: build.githubBuildReference
            };
        });
    }
}