import { IDeploy } from './types/deploy.type';

export class DeployMapper {

    static map(response: any): Array<IDeploy> {
        return response.map((deploy: any) => {
            return {
                identifier: deploy.identifier,
                commitId: deploy.commitId,
                startedAt: new Date(deploy.startedAt),
                finishedAt: new Date(deploy.finishedAt)
            };
        });
    }
}