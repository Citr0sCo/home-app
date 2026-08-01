import { LinkMapper } from './link.mapper';
import { IFolder } from './types/folder.type';

export class FolderMapper {

    public static map(response: any): Array<IFolder> {
        return response.map((folder: any) => FolderMapper.mapSingle(folder));
    }

    public static mapSingle(folder: any): IFolder {
        return {
            identifier: folder.Identifier,
            name: folder.Name,
            icon: folder.Icon,
            sortOrder: folder.SortOrder,
            columnId: folder.ColumnId,
            links: folder.Links.map((link: any) => LinkMapper.mapSingle(link))
        };
    }

    public static mapToApi(folders: Array<IFolder>): any {
        return folders.map((folder: IFolder) => FolderMapper.mapToApiSingle(folder));
    }

    public static mapToApiSingle(folder: IFolder): any {
        return {
            Identifier: folder.identifier,
            Name: folder.name,
            Icon: folder.icon,
            SortOrder: folder.sortOrder,
            ColumnId: folder.columnId,
            Links: LinkMapper.mapToApi(folder.links)
        };
    }
}
