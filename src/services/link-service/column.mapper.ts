import { LinkMapper } from './link.mapper';
import { FolderMapper } from './folder.mapper';
import { IColumn } from './types/column.type';

export class ColumnMapper {

    public static map(response: any): Array<IColumn> {
        return response.map((column: any) => ColumnMapper.mapSingle(column));
    }

    public static mapSingle(column: any): IColumn {
        return {
            identifier: column.Identifier,
            name: column.Name,
            sortOrder: column.SortOrder,
            icon: column.Icon,
            links: column.Links.map((link: any) => LinkMapper.mapSingle(link)),
            folders: FolderMapper.map(column.Folders)
        };
    }

    public static mapToApi(columns: Array<IColumn>): any {
        return columns.map((column: any) => ColumnMapper.mapToApiSingle(column));
    }

    public static mapToApiSingle(column: IColumn): any {
        return {
            Identifier: column.identifier,
            Name: column.name,
            SortOrder: column.sortOrder,
            Icon: column.icon,
            Links: LinkMapper.mapToApi(column.links),
            Folders: FolderMapper.mapToApi(column.folders)
        };
    }
}