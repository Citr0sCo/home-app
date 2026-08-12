import { IColumn } from '../../services/link-service/types/column.type';
import { ColumnItems } from '../../services/link-service/column-items';
import { IFolder } from '../../services/link-service/types/folder.type';
import { ILink } from '../../services/link-service/types/link.type';

export interface ILinkSearchResult {
    link: ILink | null;
    columnName: string | null;
    folderName: string | null;
    isGoogleSearch: boolean;
}

export function flattenLinks(columns: Array<IColumn>): Array<ILinkSearchResult> {
    return columns.flatMap((column) => ColumnItems.sort(column).flatMap((item) => {
        if (item.kind === 'link') {
            return createResult(item.link, column.name, null);
        }

        return flattenFolderLinks(column.name, item.folder);
    }));
}

export function findLinkSearchResults(columns: Array<IColumn>, query: string): Array<ILinkSearchResult> {
    const normalizedQuery = normalize(query);
    const links = flattenLinks(columns);

    if (!normalizedQuery) {
        return links;
    }

    return links
        .map((result) => ({ result, score: scoreResult(result, normalizedQuery) }))
        .filter(({ score }) => score > 0)
        .sort((a, b) => b.score - a.score || a.result.link!.name.localeCompare(b.result.link!.name))
        .map(({ result }) => result);
}

function flattenFolderLinks(columnName: string, folder: IFolder): Array<ILinkSearchResult> {
    return folder.links.map((link) => createResult(link, columnName, folder.name));
}

function createResult(link: ILink, columnName: string, folderName: string | null): ILinkSearchResult {
    return { link, columnName, folderName, isGoogleSearch: false };
}

function scoreResult(result: ILinkSearchResult, query: string): number {
    const name = normalize(result.link!.name);
    const url = normalize(result.link!.url);

    if (name === query) {
        return 1000;
    }
    if (name.startsWith(query)) {
        return 800 - (name.length - query.length);
    }
    if (name.includes(query)) {
        return 600 - (name.length - query.length);
    }
    if (url.includes(query)) {
        return 400 - Math.min(url.length - query.length, 300);
    }

    const distance = levenshteinDistance(name, query);
    const threshold = Math.max(2, Math.floor(name.length / 3));
    return distance <= threshold ? 200 - distance : 0;
}

function normalize(value: string): string {
    return value.trim().toLocaleLowerCase();
}

function levenshteinDistance(left: string, right: string): number {
    const previous = Array.from({ length: right.length + 1 }, (value, index) => index);

    for (let leftIndex = 1; leftIndex <= left.length; leftIndex++) {
        let diagonal = previous[0];
        previous[0] = leftIndex;

        for (let rightIndex = 1; rightIndex <= right.length; rightIndex++) {
            const above = previous[rightIndex];
            previous[rightIndex] = left[leftIndex - 1] === right[rightIndex - 1]
                ? diagonal
                : Math.min(previous[rightIndex - 1], above, diagonal) + 1;
            diagonal = above;
        }
    }

    return previous[right.length];
}
