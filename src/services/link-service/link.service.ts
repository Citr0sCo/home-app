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
            url: 'http://192.168.1.25:5055/',
            iconUrl: './assets/overseerr-logo.png'
        },
        {
            name: 'Radarr',
            url: 'http://radarr.lan/',
            iconUrl: './assets/radarr-logo.png'
        },
        {
            name: 'Sonarr',
            url: 'http://sonarr.lan/',
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
            url: 'http://unifi.lan/',
            iconUrl: './assets/unifi-logo.png'
        },
        {
            name: 'Synology Dashboard',
            url: 'http://192.168.1.48:5000/',
            iconUrl: './assets/synology-logo.png'
        },
        {
            name: 'Prowlarr',
            url: 'http://prowlarr.lan/',
            iconUrl: './assets/prowlarr-logo.png'
        },
        {
            name: 'PiHole',
            url: 'http://pihole.lan/admin/',
            iconUrl: './assets/pihole-logo.png'
        }
    ];

    public _productivityLinks: Array<ILink> = [
        {
            name: 'Mealie',
            url: 'http://mealie.lan/',
            iconUrl: './assets/mealie-logo.png'
        },
        {
            name: 'Planka',
            url: 'http://planka.lan/',
            iconUrl: './assets/planka-logo.png'
        },
        {
            name: 'Trilium',
            url: 'http://trilium.lan/',
            iconUrl: './assets/trilium-logo.png'
        },
        {
            name: 'OctoPi',
            url: 'http://octopi.lan/',
            iconUrl: './assets/octopi-logo.png'
        }
    ];

    public _toolsLinks: Array<ILink> = [
        {
            name: 'Portainer',
            url: 'https://192.168.1.25:9443/',
            iconUrl: './assets/portainer-logo.png'
        },
        {
            name: 'Flood',
            url: 'http://flood.lan/',
            iconUrl: './assets/flood-logo.png'
        },
        {
            name: 'Nginx Proxy Manager',
            url: 'http://ngnix-proxy-manager.lan/',
            iconUrl: './assets/nginx-proxy-manager-logo.png'
        },
        {
            name: 'Homebridge',
            url: 'http://homebridge.lan:8581/',
            iconUrl: './assets/homebridge-logo.png'
        },
        {
            name: 'Phoscon',
            url: 'http://phoscon.lan:8080/',
            iconUrl: './assets/phoscon-logo.jpeg'
        }
    ];

    public getMediaLinks(): Observable<Array<ILink>> {
        return of(this._mediaLinks);
    }

    public getSystemLinks(): Observable<Array<ILink>> {
        return of(this._systemLinks);
    }

    public getProductivityLinks(): Observable<Array<ILink>> {
        return of(this._productivityLinks);
    }

    public getToolsLinks(): Observable<Array<ILink>> {
        return of(this._toolsLinks);
    }
}
