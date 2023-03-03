import { IDeploy } from './types/deploy.type';

export class DeployMapper {

    public static map(response: any): Array<IDeploy> {
        return response.Deploys.map((deploy: any) => this.mapSingle(deploy));
    }

    public static mapSingle(deploy: any): IDeploy {
        return {
            identifier: deploy.Identifier,
            commitId: deploy.CommitId,
            startedAt: new Date(deploy.startedAt),
            finishedAt: deploy.FinishedAt ? new Date(deploy.FinishedAt) : null
        };
    }
}