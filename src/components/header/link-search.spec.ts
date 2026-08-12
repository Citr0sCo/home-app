import { IColumn } from '../../services/link-service/types/column.type';
import { ILink } from '../../services/link-service/types/link.type';
import { findLinkSearchResults, flattenLinks } from './link-search';

function createLink(name: string, sortOrder = 0, folderId: string | null = null): ILink {
    return {
        identifier: name,
        containerName: null,
        name,
        url: `https://${name}.example.com`,
        host: name,
        port: 443,
        iconUrl: '',
        sortOrder,
        columnId: 'column-1',
        folderId
    };
}

describe('link search', () => {
    const columns: Array<IColumn> = [{
        identifier: 'column-1',
        name: 'Services',
        icon: 'fa-server',
        sortOrder: 0,
        links: [createLink('Plex'), createLink('Pihole', 1)],
        folders: [{
            identifier: 'folder-1',
            name: 'Media',
            icon: 'fa-folder',
            sortOrder: 2,
            columnId: 'column-1',
            links: [createLink('Radarr', 0, 'folder-1')]
        }]
    }];

    it('flattens direct and folder links with their location context', () => {
        expect(flattenLinks(columns).map((result) => [result.link!.name, result.folderName, result.columnName])).toEqual([
            ['Plex', null, 'Services'],
            ['Pihole', null, 'Services'],
            ['Radarr', 'Media', 'Services']
        ]);
    });

    it('returns every link for an empty query', () => {
        expect(findLinkSearchResults(columns, '').map((result) => result.link!.name)).toEqual(['Plex', 'Pihole', 'Radarr']);
    });

    it('ranks exact and partial name matches ahead of other matches', () => {
        expect(findLinkSearchResults(columns, 'pihole').map((result) => result.link!.name)).toEqual(['Pihole']);
        expect(findLinkSearchResults(columns, 'plex').map((result) => result.link!.name)).toEqual(['Plex']);
    });

    it('finds a close link when the query contains a spelling mistake', () => {
        expect(findLinkSearchResults(columns, 'pihol').map((result) => result.link!.name)).toEqual(['Pihole']);
    });

    it('returns no links when the query is not reasonably close', () => {
        expect(findLinkSearchResults(columns, 'calendar')).toEqual([]);
    });
});
