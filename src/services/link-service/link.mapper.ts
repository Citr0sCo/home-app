import {ILink} from "./types/link.type";

export class LinkMapper {

    static map(response: Array<any>) : Array<ILink> {
        return response.map((link: any) => {
            return {
                name: link.Name,
                url: link.Url,
                host: link.Host,
                port: link.Port,
                isSecure: link.IsSecure,
                iconUrl: link.IconUrl
            };
        });
    }
}