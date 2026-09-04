import { ILink } from './types/link.type';
import { ColumnMapper } from './column.mapper';

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
            iconUrl: link.IconUrl,
            sortOrder: link.SortOrder,
            columnId: link.ColumnId,
            folderId: link.FolderId ?? null,
            category: link.Category,
            lastClickedAt: link.LastClickedAt ?? null
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
            IconUrl: link.iconUrl,
            SortOrder: link.sortOrder,
            ColumnId: link.columnId,
            FolderId: link.folderId,
            Category: link.category,
            LastClickedAt: link.lastClickedAt ?? null
        };
    }
}