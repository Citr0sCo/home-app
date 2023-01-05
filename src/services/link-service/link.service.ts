import { Injectable } from "@angular/core";
import { ILink } from "./types/link.type";
import { Observable, of } from "rxjs";

@Injectable()
export class LinkService {

    public _mediaLinks: Array<ILink> = [
        {
            name: 'Plex',
            url: 'http://192.168.1.25:32400/',
            iconUrl: './assets/plex-logo.png'
        },
        {
            name: 'Overseerr',
            url: 'https://overseerr.miloszdura.com/',
            iconUrl: './assets/overseerr-logo.png'
        },
        {
            name: 'Radarr',
            url: 'http://192.168.1.25:7878/',
            iconUrl: './assets/radarr-logo.png'
        },
        {
            name: 'Sonarr',
            url: 'http://192.168.1.25:8989/',
            iconUrl: './assets/sonarr-logo.png'
        },
        {
            name: 'xTeve',
            url: 'http://192.168.1.25:34400/web/',
            iconUrl: './assets/xteve-logo.png'
        }
    ];

    public _systemLinks: Array<ILink> = [
        {
            name: 'UniFi Controller',
            url: 'https://192.168.1.1/',
            iconUrl: './assets/unifi-logo.png'
        },
        {
            name: 'Synology Dashboard',
            url: 'http://192.168.1.48:5000/',
            iconUrl: './assets/synology-logo.png'
        },
        {
            name: 'Prowlarr',
            url: 'http://192.168.1.25:9696/',
            iconUrl: './assets/prowlarr-logo.png'
        },
        {
            name: 'PiHole',
            url: 'http://192.168.1.50/admin/',
            iconUrl: './assets/pihole-logo.png'
        }
    ];

    public _homeAutomationLinks: Array<ILink> = [
        {
            name: 'Homebridge',
            url: 'http://192.168.1.50:8581/',
            iconUrl: './assets/homebridge-logo.png'
        },
        {
            name: 'Phoscon',
            url: 'http://192.168.1.50:8080/',
            iconUrl: './assets/phoscon-logo.jpeg'
        }
    ];

    public _toolsLinks: Array<ILink> = [
        {
            name: 'Portainer',
            url: 'https://portainer.miloszdura.com/',
            iconUrl: './assets/portainer-logo.png'
        },
        {
            name: 'Mealie',
            url: 'https://mealie.miloszdura.com/',
            iconUrl: './assets/mealie-logo.png'
        },
        {
            name: 'Planka',
            url: 'http://192.168.1.25:9096/',
            iconUrl: './assets/planka-logo.png'
        },
        {
            name: 'Trilium',
            url: 'https://trilium.miloszdura.com/',
            iconUrl: './assets/trilium-logo.png'
        },
        {
            name: 'Flood',
            url: 'http://192.168.1.25:8081/',
            iconUrl: './assets/flood-logo.png'
        },
        {
            name: 'OctoPi',
            url: 'http://192.168.1.35/',
            iconUrl: './assets/octopi-logo.png'
        },
        {
            name: 'Nginx Proxy Manager',
            url: 'http://192.168.1.25:81/',
            iconUrl: './assets/nginx-proxy-manager-logo.png'
        }
    ];

    public getMediaLinks(): Observable<Array<ILink>> {
        return of(this._mediaLinks);
    }

    public getSystemLinks(): Observable<Array<ILink>> {
        return of(this._systemLinks);
    }

    public getHomeAutomationLinks(): Observable<Array<ILink>> {
        return of(this._homeAutomationLinks);
    }

    public getToolsLinks(): Observable<Array<ILink>> {
        return of(this._toolsLinks);
    }
}
