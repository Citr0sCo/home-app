import {ILink} from "./types/link.type";

export class LinkMapper {

    static map(response: any) : Array<ILink> {
        return response.map((link: any) => {
            return {
                identifier: link.identifier,
                name: link.name,
                url: link.url,
                host: link.host,
                port: link.port,
                isSecure: link.isSecure,
                iconUrl: link.iconUrl,
                category: link.category,
                sortOrder: link.sortOrder
            };
        });
    }
}