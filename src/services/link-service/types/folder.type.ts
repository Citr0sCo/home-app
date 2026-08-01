import { ILink } from './link.type';

export interface IFolder {
    identifier: string | null;
    name: string;
    icon: string;
    sortOrder: number;
    columnId: string;
    links: Array<ILink>;
}
