import { IDeploy } from './types/deploy.type';

export class DeployMapper {

    public static map(response: any): Array<IDeploy> {
        return response.map((deploy: any) => this.mapSingle(deploy));
    }

    public static mapSingle(deploy: any): IDeploy {
        return {
            identifier: deploy.identifier,
            commitId: deploy.commitId,
            startedAt: new Date(deploy.startedAt),
            finishedAt: deploy.finishedAt ? new Date(deploy.finishedAt) : null
        };
    }
}