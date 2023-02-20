import {ILink} from "./types/link.type";

export class LinkMapper {

    static map(response: Array<any>) : Array<ILink> {
        return response.map((link: any) => {
            return {
                name: link.name,
                url: link.url,
                host: link.host,
                port: link.port,
                isSecure: link.isSecure,
                iconUrl: link.iconUrl
            };
        });
    }
}