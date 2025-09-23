import { ILink } from './types/link.type';

export class LinkMapper {

    public static map(response: any): Array<ILink> {
        return response.map((link: any) => LinkMapper.mapSingle(link));
    }

    public static mapSingle(link: any): ILink {
        return {
            identifier: link.Identifier,
            containerName: link.containerName,
            name: link.Name,
            url: link.Url,
            host: link.Host,
            port: link.Port,
            isSecure: link.IsSecure,
            iconUrl: link.IconUrl,
            category: link.Category,
            sortOrder: link.SortOrder
        };
    }

    public static mapToApi(links: Array<ILink>): any {
        return links.map((link: any) => LinkMapper.mapToApiSingle(link));
    }

    public static mapToApiSingle(link: ILink): any {
        return {
            Identifier: link.identifier,
            ContainerName: link.containerName,
            Name: link.name,
            Url: link.url,
            Host: link.host,
            Port: link.port,
            IsSecure: link.isSecure,
            IconUrl: link.iconUrl,
            Category: link.category,
            SortOrder: link.sortOrder
        };
    }
}