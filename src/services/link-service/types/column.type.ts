import { ILink } from './link.type';

export interface IColumn {
    identifier: string | null;
    name: string;
    sortOrder: number;
    icon: string;
    links: Array<ILink>;
}
