import { ColumnItems } from './column-items';
import { IColumn } from './types/column.type';
import { IFolder } from './types/folder.type';
import { ILink } from './types/link.type';

describe('ColumnItems', () => {
    const link = (name: string, sortOrder: number): ILink => ({
        identifier: `${name}-id`,
        containerName: null,
        name,
        url: `https://${name}.example.com`,
        host: `${name}.example.com`,
        port: 443,
        iconUrl: `${name}.png`,
        sortOrder,
        columnId: 'old-column',
        folderId: 'old-folder'
    });

    const folder = (name: string, sortOrder: number, links: Array<ILink> = []): IFolder => ({
        identifier: `${name}-id`,
        name,
        icon: 'folder',
        sortOrder,
        columnId: 'old-column',
        links
    });

    it('sorts links and folders together by their sort order', () => {
        const column: IColumn = {
            identifier: 'column-id',
            name: 'Services',
            sortOrder: 0,
            icon: 'server',
            links: [link('later', 2), link('first', 0)],
            folders: [folder('middle', 1)]
        };

        expect(ColumnItems.sort(column).map((item) => item.kind === 'link' ? item.link.name : item.folder.name))
            .toEqual(['first', 'middle', 'later']);
        expect(ColumnItems.isFolder(column.folders[0])).toBe(true);
        expect(ColumnItems.isFolder(column.links[0])).toBe(false);
    });

    it('writes reordered items and updates ownership metadata', () => {
        const nestedLink = link('nested', 99);
        const looseLink = link('loose', 99);
        const nestedFolder = folder('nested-folder', 99, [nestedLink]);
        const column: IColumn = {
            identifier: 'column-id',
            name: 'Services',
            sortOrder: 0,
            icon: 'server',
            links: [],
            folders: []
        };

        ColumnItems.write(column, [ColumnItems.wrap(nestedFolder), ColumnItems.wrap(looseLink)]);

        expect(column.folders).toEqual([nestedFolder]);
        expect(column.links).toEqual([looseLink]);
        expect(nestedFolder.sortOrder).toBe(0);
        expect(nestedFolder.columnId).toBe('column-id');
        expect(nestedLink.sortOrder).toBe(0);
        expect(nestedLink.columnId).toBe('column-id');
        expect(nestedLink.folderId).toBe('nested-folder-id');
        expect(looseLink.sortOrder).toBe(1);
        expect(looseLink.columnId).toBe('column-id');
        expect(looseLink.folderId).toBeNull();
    });
});
