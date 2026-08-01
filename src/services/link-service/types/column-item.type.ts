import { ILink } from './link.type';
import { IFolder } from './folder.type';

export type IColumnItem =
    { kind: 'link'; link: ILink } |
    { kind: 'folder'; folder: IFolder };
