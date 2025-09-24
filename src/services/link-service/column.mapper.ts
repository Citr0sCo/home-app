import { LinkMapper } from './link.mapper';
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
            links: column.Links.map((link: any) => LinkMapper.mapSingle(link))
        };
    }

    public static mapToApi(columns: Array<IColumn>): any {
        return columns.map((column: any) => ColumnMapper.mapToApiSingle(column));
    }

    public static mapToApiSingle(link: IColumn): any {
        return {
            Identifier: link.identifier,
            Name: link.name,
            SortOrder: link.sortOrder,
            Icon: link.icon,
            Links: LinkMapper.mapToApi(link.links)
        };
    }
}