import { IColumn } from './column.type';

export interface ILink {
    identifier: string | null;
    containerName: string | null;
    name: string;
    url: string;
    host: string;
    port: number;
    iconUrl: string;
    sortOrder: number;
    columnId: string;
    folderId: string | null;
    category?: string | null;
    lastClickedAt?: string | null;
}
