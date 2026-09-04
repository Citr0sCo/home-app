import { ColumnMapper } from './column.mapper';
import { FolderMapper } from './folder.mapper';
import { LinkMapper } from './link.mapper';
import { IColumn } from './types/column.type';
import { IFolder } from './types/folder.type';
import { ILink } from './types/link.type';

describe('link service mappers', () => {
    const apiLink = {
        Identifier: 'link-id',
        containerName: 'container',
        Name: 'Dashboard',
        Url: 'https://example.com',
        Host: 'example.com',
        Port: 443,
        IconUrl: 'icon.png',
        SortOrder: 2,
        ColumnId: 'column-id',
        FolderId: undefined,
        Category: 'tools',
        LastClickedAt: '2026-08-01T12:00:00Z'
    };

    it('maps links from and to the API shape', () => {
        const mapped = LinkMapper.mapSingle(apiLink);

        expect(mapped.folderId).toBeNull();
        expect(mapped.name).toBe('Dashboard');
        expect(LinkMapper.map([apiLink])).toEqual([mapped]);
        expect(LinkMapper.mapToApiSingle(mapped)).toEqual({
            Identifier: 'link-id',
            ContainerName: 'container',
            Name: 'Dashboard',
            Url: 'https://example.com',
            Host: 'example.com',
            Port: 443,
            IconUrl: 'icon.png',
            SortOrder: 2,
            ColumnId: 'column-id',
            FolderId: null,
            Category: 'tools',
            LastClickedAt: '2026-08-01T12:00:00Z'
        });
    });

    it('maps folders and columns, including nested links', () => {
        const apiFolder = {
            Identifier: 'folder-id',
            Name: 'Tools',
            Icon: 'folder',
            SortOrder: 1,
            ColumnId: 'column-id',
            Links: [apiLink]
        };
        const apiColumn = {
            Identifier: 'column-id',
            Name: 'Services',
            SortOrder: 0,
            Icon: 'server',
            Links: [apiLink],
            Folders: [apiFolder]
        };

        const folder = FolderMapper.mapSingle(apiFolder);
        const column = ColumnMapper.mapSingle(apiColumn);

        expect(folder.links[0].name).toBe('Dashboard');
        expect(FolderMapper.mapToApi([folder])[0].Links[0].Name).toBe('Dashboard');
        expect(column.folders[0].name).toBe('Tools');
        expect(column.links[0].name).toBe('Dashboard');
        expect(ColumnMapper.map([apiColumn])).toEqual([column]);
    });

    it('maps local folders and columns back to API collections', () => {
        const link: ILink = {
            identifier: 'link-id', containerName: null, name: 'Dashboard', url: 'https://example.com',
            host: 'example.com', port: 443, iconUrl: 'icon.png', sortOrder: 0,
            columnId: 'column-id', folderId: null
        };
        const folder: IFolder = {
            identifier: 'folder-id', name: 'Tools', icon: 'folder', sortOrder: 1,
            columnId: 'column-id', links: [link]
        };
        const column: IColumn = {
            identifier: 'column-id', name: 'Services', sortOrder: 0, icon: 'server',
            links: [link], folders: [folder]
        };

        expect(FolderMapper.mapToApi([folder])).toHaveLength(1);
        expect(ColumnMapper.mapToApi([column])).toHaveLength(1);
        expect(ColumnMapper.mapToApiSingle(column).Folders[0].Identifier).toBe('folder-id');
    });
});
