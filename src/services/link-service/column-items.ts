import { IColumn } from './types/column.type';
import { IColumnItem } from './types/column-item.type';
import { IFolder } from './types/folder.type';
import { ILink } from './types/link.type';

/**
 * Links and folders share a single sort order space within a column so the two can be interleaved.
 * These helpers flatten a column into that ordered sequence and write a reordered sequence back.
 */
export class ColumnItems {

    public static isFolder(item: ILink | IFolder): item is IFolder {
        return (item as IFolder).links !== undefined;
    }

    public static wrap(item: ILink | IFolder): IColumnItem {
        return ColumnItems.isFolder(item)
            ? { kind: 'folder', folder: item }
            : { kind: 'link', link: item };
    }

    public static sort(column: IColumn): Array<IColumnItem> {
        return [
            ...column.links.map((link) => ColumnItems.wrap(link)),
            ...column.folders.map((folder) => ColumnItems.wrap(folder))
        ].sort((a, b) => ColumnItems.sortOrderOf(a) - ColumnItems.sortOrderOf(b));
    }

    public static write(column: IColumn, items: Array<IColumnItem>): void {

        column.links = [];
        column.folders = [];

        items.forEach((item, index) => {

            if (item.kind === 'link') {
                item.link.sortOrder = index;
                item.link.columnId = column.identifier!;
                item.link.folderId = null;
                column.links.push(item.link);
                return;
            }

            item.folder.sortOrder = index;
            item.folder.columnId = column.identifier!;

            item.folder.links.forEach((link, linkIndex) => {
                link.sortOrder = linkIndex;
                link.columnId = column.identifier!;
                link.folderId = item.folder.identifier;
            });

            column.folders.push(item.folder);
        });
    }

    private static sortOrderOf(item: IColumnItem): number {
        return item.kind === 'link' ? item.link.sortOrder : item.folder.sortOrder;
    }
}
