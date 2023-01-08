import { Injectable } from "@angular/core";
import { ILink } from "./types/link.type";
import { Observable, of } from "rxjs";

@Injectable()
export class LinkService {

    public _mediaLinks: Array<ILink> = [
        {
            name: 'Plex',
            url: 'plex.lan/',
            iconUrl: './assets/plex-logo.png'
        },
        {
            name: 'Overseerr',
            url: 'overseerr.lan/',
            iconUrl: './assets/overseerr-logo.png'
        },
        {
            name: 'Radarr',
            url: 'radarr.lan/',
            iconUrl: './assets/radarr-logo.png'
        },
        {
            name: 'Sonarr',
            url: 'sonarr.lan/',
            iconUrl: './assets/sonarr-logo.png'
        },
        {
            name: 'xTeve',
            url: 'xteve.lan/web/',
            iconUrl: './assets/xteve-logo.png'
        }
    ];

    public _systemLinks: Array<ILink> = [
        {
            name: 'UniFi Controller',
            url: 'unifi.lan/',
            iconUrl: './assets/unifi-logo.png'
        },
        {
            name: 'Synology Dashboard',
            url: 'synology-nas.lan:5000/',
            iconUrl: './assets/synology-logo.png'
        },
        {
            name: 'Prowlarr',
            url: 'prowlarr.lan/',
            iconUrl: './assets/prowlarr-logo.png'
        },
        {
            name: 'PiHole',
            url: 'pihole.lan/admin/',
            iconUrl: './assets/pihole-logo.png'
        }
    ];

    public _productivityLinks: Array<ILink> = [
        {
            name: 'Mealie',
            url: 'mealie.lan/',
            iconUrl: './assets/mealie-logo.png'
        },
        {
            name: 'Planka',
            url: 'planka.lan/',
            iconUrl: './assets/planka-logo.png'
        },
        {
            name: 'Trilium',
            url: 'trilium.lan/',
            iconUrl: './assets/trilium-logo.png'
        },
        {
            name: 'OctoPi',
            url: 'octopi.lan/',
            iconUrl: './assets/octopi-logo.png'
        }
    ];

    public _toolsLinks: Array<ILink> = [
        {
            name: 'Portainer',
            url: 'portainer.lan/',
            iconUrl: './assets/portainer-logo.png'
        },
        {
            name: 'Flood',
            url: 'flood.lan/',
            iconUrl: './assets/flood-logo.png'
        },
        {
            name: 'Nginx Proxy Manager',
            url: 'ngnix-proxy-manager.lan/',
            iconUrl: './assets/nginx-proxy-manager-logo.png'
        },
        {
            name: 'Homebridge',
            url: 'homebridge.lan:8581/',
            iconUrl: './assets/homebridge-logo.png'
        },
        {
            name: 'Phoscon',
            url: 'phoscon.lan:8080/',
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
